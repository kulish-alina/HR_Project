import utils  from '../utils.js';
import {
   filter,
   remove,
   each,
   map,
   mapValues,
   concat,
   clone,
   isObject
} from 'lodash';
const VACANCY_URL = 'vacancies/';
const DATE_TYPE = ['startDate', 'deadlineDate', 'endDate'];
const MAP_LIST_THESAURUS = {
   'industries' : 'industryId',
   'levels': 'levelIds',
   'locations': 'locationIds',
   'typesOfEmployment': 'typeOfEmployment',
   'entityStates': 'state'
};
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

   searchVacancies(entity) {
      const dateFields = this._addCreatedOnDate(DATE_TYPE);
      const searchUrl = 'search';
      const additionalUrl = VACANCY_URL + searchUrl;
      entity = clone(entity);
      each(entity, (property, key) => {
         if (property === undefined) {
            delete entity[key];
         }
      });
      return _HttpService.post(additionalUrl, entity).then((vacancies) => {
         return _$q.all(map(vacancies, (vacancy) => {
            each(dateFields, (type) => {
               vacancy[type] = utils.formatDateFromServer(vacancy[type]);
            });
            return this._convertIdsToEntities(vacancy).then(() => vacancy);
         }));
      });
   }

   getVacancies() {
      const dateFields = this._addCreatedOnDate(DATE_TYPE);
      return _HttpService.get(VACANCY_URL).then((vacancies) => {
         let allVacancies = vacancies.queryResult;
         allVacancies.countOfPages = vacancies.totalPages;
         return each(allVacancies, (vacancy) => {
            each(dateFields, (type) => {
               vacancy[type] = utils.formatDateFromServer(vacancy[type]);
            });
            return this._convertIdsToEntities(vacancy);
         });
      });
   }

   getVacancy(vacancyId) {
      const additionalUrl = VACANCY_URL + vacancyId;
      const dateFields = this._addCreatedOnDate(DATE_TYPE);
      return _HttpService.get(additionalUrl).then((vacancy) => {
         each(dateFields, (type) => {
            vacancy[type] = utils.formatDateFromServer(vacancy[type]);
         });
         return vacancy = this._convertIdsToEntities(vacancy);
      });
   }

   _saveNewTopicsToThesaurus(data) {
      let promises = mapValues(data, (list, thesaurusName) => {
         return _ThesaurusService.saveThesaurusTopics(thesaurusName, list);
      });
      return _$q.all(promises);
   }

   _onError(err) {
      _LoggerService.error(err);
      return _$q.reject(err);
   }

   saveVacancy(entity) {
      entity = clone(entity);
      each(DATE_TYPE, (type) => {
         entity[type] = utils.formatDateToServer(entity[type]);
      });
      let mapEntity = {
         'skills' : 'requiredSkills',
         'tags': 'tags'
      };
      let mapEntity2 = {
         requiredSkills: 'requiredSkillIds',
         tags: 'tagIds'
      };

      let data = mapValues(mapEntity, (vacancyKey) => {
         return filter(entity[vacancyKey], {id: undefined});
      });
      return this._saveNewTopicsToThesaurus(data).then((promises) => {
         each(mapEntity, (vacancyKey, thesaurusName) => {
            remove(entity[vacancyKey], {id: undefined});
            entity[vacancyKey] = concat(entity[vacancyKey], promises[thesaurusName]);
            entity[mapEntity2[vacancyKey]] = map(entity[vacancyKey], 'id');
            delete entity[vacancyKey];
         });
         if (entity.id) {
            const additionalUrl = VACANCY_URL + entity.id;
            delete entity.createdOn;
            delete entity.responsible;
            each(MAP_LIST_THESAURUS, (thesaurusKey, thesaurusName) => {
               delete entity[thesaurusName];
            });
            return _HttpService.put(additionalUrl, entity);
         } else {
            return _HttpService.post(VACANCY_URL, entity);
         }
      }).then((_entity) => {
         return this._changeDatesFormateToFrontend(_entity);
      }).then((_entity) => this.convertIdsToString(_entity)).catch(this._onError);
   }

   deleteVacancy(entity) {
      if (entity.id === undefined) {
         throw new Error('Id should be specified');
      } else {
         const additionalUrl = VACANCY_URL + entity.id;
         return _HttpService.remove(additionalUrl, entity);
      }
   }

   _convertIdsToEntities(entity) {
      let promises = map(MAP_LIST_THESAURUS, (thesaurusKey, thesaurusName) => {
         return _ThesaurusService.getThesaurusTopicsByIds(thesaurusName, entity[thesaurusKey]).then((promise) => {
            entity[thesaurusName] = promise;
         });
      });
      promises.push(_UserService.getUser(entity.responsibleId).then((user) => entity.responsible = user));
      return _$q.all(promises);
   }

   _addCreatedOnDate(dates) {
      return dates.concat([ 'createdOn' ]);
   }

   _changeDatesFormateToFrontend(_entity) {
      each(DATE_TYPE, (type) => {
         _entity[type] = utils.formatDateFromServer(_entity[type]);
      });
      return this._convertIdsToEntities(_entity);
   }

   convertIdsToString(_entity) {
      let listProperties = ['industryId', 'responsibleId', 'departmentId',
                            'typeOfEmployment', 'state', 'languageSkill', 'requiredSkillIds'];
      let listOfDeepProperties = ['languageId', 'languageLevel'];
      each(listProperties, (entityProperty) => {
         if (isObject(_entity[entityProperty])) {
            each(listOfDeepProperties, property => {
               _entity[entityProperty][property] += '';
            });
         } else {
            _entity[entityProperty] += '';
         }
      });
      return _entity;
   }
}
