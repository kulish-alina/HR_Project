import utils from '../utils.js';
import {
   filter,
   remove,
   each,
   map
//   split,
//   trimEnd
} from 'lodash';

const VACANCY_URL = 'vacancies';
const DATE_TYPE = ['startDate', 'deadlineDate', 'endDate'];
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
      let newDates = [];
      each(DATE_TYPE, (type) => {
         newDates.push(utils.formatDateToServer(entity[type]));
         each(newDates, (date) => {
            entity[type] = date;
         });
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
            return _HttpService.put(additionalUrl, entity)
               .then(_changeFormateToFrontend)
               .catch(() => _changeFormateToFrontend(entity));
         } else {
            return _HttpService.post(VACANCY_URL, entity)
               .then(_changeFormateToFrontend)
               .catch(() => _changeFormateToFrontend(entity));
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

function _changeFormateToFrontend(_entity) {
   console.log(_entity);
   let oldDates = [];
   each(DATE_TYPE, (type) => {
      oldDates.push(utils.formatDateFromServer(_entity[type]));
      each(oldDates, (date) => {
         _entity[type] = date;
      });
   });
   return _entity;
}
