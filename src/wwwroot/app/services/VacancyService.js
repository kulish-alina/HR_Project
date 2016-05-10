import utils from '../utils.js';
import {
   filter,
   remove,
   each,
   map,
   concat
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
      map(data, (list, thesaurusName) => {
         promises[thesaurusName] = _ThesaurusService.saveThesaurusTopics(thesaurusName, list);
      });
      return _$q.all(promises);
   }

   _onError(err) {
      _LoggerService.error(err);
      return _$q.reject(err);
   }

   saveVacancy(entity) {
      each(DATE_TYPE, (type) => {
         entity[type] = utils.formatDateToServer(entity[type]);
      });
      let mapEntity = {
         'skills' : 'requiredSkills',
         'tags': 'tags'
      };

      let data = {};
      map(mapEntity, (vacancyKey, thesaurusName) => {
         data[thesaurusName] = filter(entity[vacancyKey], {id: undefined});
      });

      return this._saveNewTopicsToThesaurus(data).then((promises) => {
         each(mapEntity, (vacancyKey, thesaurusName) => {
            remove(entity[vacancyKey], {id: undefined});
            entity[vacancyKey] = concat(entity[vacancyKey], promises[thesaurusName]);
            entity[vacancyKey] = map(entity[vacancyKey], item => item.id);
         });

         if (entity.id) {
            const additionalUrl = VACANCY_URL + entity.id;
            return _HttpService.put(additionalUrl, entity);
         } else {
            return _HttpService.post(VACANCY_URL, entity);
         }
      }).then(_changeFormateToFrontend).catch(this._onError);
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
   each(DATE_TYPE, (type) => {
      _entity[type] = utils.formatDateFromServer(_entity[type]);
   });
   return _entity;
}
