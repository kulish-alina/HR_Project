import utils from '../utils.js';
import {
   filter,
   remove,
   each,
   map,
   mapValues,
   concat
} from 'lodash';
const VACANCY_URL = 'vacancies/';
const DATE_TYPE = ['startDate', 'deadlineDate', 'endDate'];
let _HttpService;
let _ThesaurusService;
let _$q;
let _LoggerService;
let _UserService;

export default class VacancyService {
   constructor(HttpService, ThesaurusService, $q, LoggerService, UserService) {
      'ngInject';
      _HttpService = HttpService;
      _ThesaurusService = ThesaurusService;
      _$q = $q;
      _LoggerService = LoggerService;
      _UserService = UserService;
   }

   getVacancies() {
      return _HttpService.get(VACANCY_URL).then((vacancies) => {
         each(vacancies, (vacancy) => {
            return this._convertIdsToEntities(vacancy);
         });
      });
   }

   getVacancy(vacancyId) {
      const additionalUrl = VACANCY_URL + vacancyId;
      return _HttpService.get(additionalUrl).then((vacancy) => {
         return vacancy = this._convertIdsToEntities(vacancy);
      });
   }

   _saveNewTopicsToThesaurus(data) {
      let promises = map(data, (list, thesaurusName) => {
         return _ThesaurusService.saveThesaurusTopics(thesaurusName, list);
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

      let data = mapValues(mapEntity, (vacancyKey) => {
         return filter(entity[vacancyKey], {id: undefined});
      });

      return this._saveNewTopicsToThesaurus(data).then((promises) => {
         each(mapEntity, (vacancyKey, thesaurusName) => {
            remove(entity[vacancyKey], {id: undefined});
            entity[vacancyKey] = concat(entity[vacancyKey], promises[thesaurusName]);
            entity[vacancyKey] = map(entity[vacancyKey], 'id');
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

   _convertIdsToEntities(entity) {
      each(DATE_TYPE, (type) => {
         entity[type] = utils.formatDateFromServer(entity[type]);
      });
      _UserService.getUser(entity.ResponsibleId).then((user) => entity.responsible = user);
      let mapListThesauruses = {
         'industries' : 'IndustryId',
         'levels': 'LevelIds',
         'locations': 'LocationIds',
         'typesOfEmployment': 'TypeOfEmployment',
         'entityStates': 'State'
      };
      each(mapListThesauruses, (thesaurusKey, thesaurusName) => {
         _ThesaurusService.getThesaurusTopicsByIds(thesaurusName, entity[thesaurusKey]).then((promise) => {
            console.log('promise', promise);
            entity[thesaurusName] = promise;
         });
      }
      );
      return entity;
   }
}

function _changeFormateToFrontend(_entity) {
   each(DATE_TYPE, (type) => {
      _entity[type] = utils.formatDateFromServer(_entity[type]);
   });
   return _entity;
}
