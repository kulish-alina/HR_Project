import {
   filter,
   remove,
   each,
   map,
   split,
   trimEnd
} from 'lodash';

const VACANCY_URL = 'vacancies';
let _HttpService;
let _ThesaurusService;
let _$q;
let _LoggerService;

export default class VacancyService {
   constructor(HttpService, ThesaurusService, $q, LoggerService) {
      'ngInject';
      _HttpService = HttpService;
      _ThesaurusService = ThesaurusService;
      _$q = $q;
      _LoggerService = LoggerService;
   }

   getVacancies() {
      return _HttpService.get(VACANCY_URL);
   }

   getVacancy(vacancyId) {
      const additionalUrl = VACANCY_URL + vacancyId;
      return _HttpService.get(additionalUrl);
   }

   _saveNewTopicsToThesaurus(data) {
      let promises = {};
      each(data, (list, thesaurusName) => {
         promises[thesaurusName] = _ThesaurusService.saveThesaurusTopics(thesaurusName, list);
      });
      return _$q.all(promises);
   }

   _onError() {
      _LoggerService.error();
   }

   saveVacancy(entity) {
      let dateType = ['startDate', 'deadlineDate', 'endDate'];
      each(dateType, (type) => {
         let partsOfDate = split(entity[type], '.');
         let indexOfParts = [2, 0, 1];
         let newDate = '';
         each(indexOfParts, (index) => {
            newDate += `${partsOfDate[index]}-`;
         });
         entity[type] = `${trimEnd(newDate, '-')}T00:00:00`;
      });
      let mapEntity = {
         'skills' : 'requiredSkills',
         'tags': 'tags'
      };

      let data = {};
      each(mapEntity, (vacancyKey, thesaurusName) => {
         data[thesaurusName] = filter(entity[vacancyKey], {id: undefined});
      });

      this._saveNewTopicsToThesaurus(data).then((promises) => {
         each(mapEntity, (vacancyKey, thesaurusName) => {
            remove(entity[vacancyKey], {id: undefined});
            each(promises[thesaurusName], topic => {
               entity[vacancyKey].push(topic);
            });
            entity[vacancyKey] = map(entity[vacancyKey], item => item.id);
         });

         if (entity.id) {
            const additionalUrl = VACANCY_URL + entity.id;
            return _HttpService.put(additionalUrl, entity);
         } else {
            return _HttpService.post(VACANCY_URL, entity);
         }
      }).catch(this._onError);
   }

   deleteVacancy(entity) {
      if (entity.id === undefined) {
         throw new Error('Id should be specified');
      } else {
         const additionalUrl = VACANCY_URL + entity.id;
         _HttpService.remove(additionalUrl, entity);
      }
   }
}
