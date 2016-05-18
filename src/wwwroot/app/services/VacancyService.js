import utils  from '../utils.js';
import {
   filter,
   remove,
   each,
   map,
   mapValues,
   concat,
   clone
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
         return each(vacancies, (vacancy) => {
            each(dateFields, (type) => {
               vacancy[type] = utils.formatDateFromServer(vacancy[type]);
            });
            return this._convertIdsToEntities(vacancy);
         });
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
      console.log(entity);
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
            each(MAP_LIST_THESAURUS, (thesaurusKey, thesaurusName) => {
               delete entity[thesaurusName];
            });
            return _HttpService.put(additionalUrl, entity);
         } else {
            return _HttpService.post(VACANCY_URL, entity);
         }
      }).then((_entity) => this._changeFormateToFrontend(_entity)).catch(this._onError);
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
      _UserService.getUser(entity.responsibleId).then((user) => entity.responsible = user);
      each(MAP_LIST_THESAURUS, (thesaurusKey, thesaurusName) => {
         _ThesaurusService.getThesaurusTopicsByIds(thesaurusName, entity[thesaurusKey]).then((promise) => {
            entity[thesaurusName] = promise;
         });
      }
      );
      return entity;
   }

   _addCreatedOnDate(dates) {
      return dates.concat([ 'createdOn' ]);
   }

   _changeFormateToFrontend(_entity) {
      each(DATE_TYPE, (type) => {
         _entity[type] = utils.formatDateFromServer(_entity[type]);
      });
      console.log('this._convertIdsToEntities(_entity)', this._convertIdsToEntities(_entity));
      return this._convertIdsToEntities(_entity);
   }
}

