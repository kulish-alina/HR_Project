import utils  from '../utils.js';
import {
   isNumber,
   filter,
   remove,
   each,
   map,
   union,
   cloneDeep,
   reduce,
   result,
   assignIn
} from 'lodash';

const VACANCY_URL = 'vacancy/';
const DATE_TYPE = ['startDate', 'deadlineDate', 'endDate', 'createdOn'];

const THESAURUS = [
   new ThesaurusHelper('tag',   'tagIds',            'tags',           true),
   new ThesaurusHelper('skill', 'requiredSkillIds',  'requiredSkills', true),

   new ThesaurusHelper('industry',   'industryId',  'industries'),
   new ThesaurusHelper('level',       'levelIds',    'levels'),
   new ThesaurusHelper('city',    'cityIds', 'cities'),
   new ThesaurusHelper('department',  'departmentId', 'departments'),
   new ThesaurusHelper('entityState', 'state',       'entityStates'),
   new ThesaurusHelper('typeOfEmployment', 'typeOfEmployment', 'typesOfEmployment')
];

let _HttpService;
let _ThesaurusService;
let _$q;
let _LoggerService;
let _UserService;
let _VacancyService;
let _CandidateService;

export default class VacancyService {
   constructor(HttpService, ThesaurusService, $q, LoggerService, UserService, CandidateService) {
      'ngInject';
      _HttpService = HttpService;
      _ThesaurusService = ThesaurusService;
      _$q = $q;
      _LoggerService = LoggerService;
      _UserService = UserService;
      _VacancyService = this;
      _CandidateService  = CandidateService;
   }

   getVacancy(id) {
      return _HttpService.get(`${VACANCY_URL}${id}`).then(this.convertFromServerFormat);
   }

   search(condition) {
      _LoggerService.debug('search vacancies', condition);
      return _HttpService.post(`${VACANCY_URL}search`, condition).then(response => {
         return _$q.all(map(response.vacancies, this.convertFromServerFormat)).then((vacancies) => {
            response.vacancies = vacancies;
            return response;
         });
      });
   }

   convertFromServerFormat(vacancy) {
      vacancy = _convertFromServerDates(vacancy);
      _VacancyService._fillVacancyLanguageSkills(vacancy)
      .then((responsePromise) => {
         vacancy.languageSkill.languageLevelObj = responsePromise[0];
         vacancy.languageSkill.languageLevel = toString(vacancy.languageSkill.languageLevel);
         vacancy.languageSkill.language = responsePromise[1];
         vacancy.languageSkill.languageId = toString(vacancy.languageSkill.languageId);
      });
      return _VacancyService._getVacancyFields(vacancy)
      .then((promises) => {
         vacancy.responsible = promises[0];
         vacancy.responsibleId = toString(vacancy.responsibleId);
         assignIn(vacancy, promises[1]);
         vacancy.childVacancies = promises[2];
         vacancy.closingCandidate = promises[3];
         return vacancy;
      });
   }

   remove(vacancy) {
      if (vacancy.id && vacancy.parentVacancyId !== null) {
         let predicate = {id: vacancy.id};
         let parentVacancy = {};
         return _VacancyService.getVacancy(vacancy.parentVacancyId)
         .then((responseVacancy) => {
            parentVacancy = responseVacancy;
            return _HttpService.remove(`${VACANCY_URL}${vacancy.id}`, vacancy);
         })
         .then(() => {
            remove(parentVacancy.childVacancies, predicate);
            parentVacancy.childVacanciesNumber = parentVacancy.childVacancies.length;
            return _VacancyService.save(parentVacancy);
         });
      } else {
         return _HttpService.remove(`${VACANCY_URL}${vacancy.id}`, vacancy);
      }
   }

   save(vacancy) {
      vacancy = cloneDeep(vacancy);
      return _saveNewTopics(vacancy).then((storedTopics) => {
         each(storedTopics, (list, fieldName) => {
            vacancy[fieldName] = union(vacancy[fieldName] || [], list);
         });
         vacancy = _convertThesaurusToIds(vacancy);
         vacancy = _convertToServerDates(vacancy);
         delete vacancy.createdOn;
         delete vacancy.responsible;
         delete vacancy.childVacancies;
         vacancy.languageSkill = vacancy.languageSkill || {};
         if (result(vacancy, 'languageSkill.languageId')) {
            delete vacancy.languageSkill.language;
            delete vacancy.languageSkill.languageLevelObj;
            vacancy.languageSkill.languageLevel = parseInt(vacancy.languageSkill.languageLevel);
            vacancy.languageSkill.languageId = parseInt(vacancy.languageSkill.languageId);
         } else {
            vacancy.languageSkill = null;
         }
         vacancy.responsibleId = parseInt(vacancy.responsibleId);

         if (vacancy.id) {
            return _HttpService.put(`${VACANCY_URL}${vacancy.id}`, vacancy);
         } else {
            return _HttpService.post(VACANCY_URL, vacancy);
         }
      }).then(this.convertFromServerFormat);
   }

   _getVacancyFields(vacancy) {
      let userPromise = _VacancyService._getUser(vacancy);
      let thesaurusesPromises = _VacancyService._getThesauruses(vacancy);
      let childVacancyPromises = _VacancyService._getChildVacancies(vacancy);
      let closingCandidatePromise = _VacancyService._getClosingCandidate(vacancy);
      return _$q.all([userPromise, thesaurusesPromises, childVacancyPromises, closingCandidatePromise]);
   }

   _getUser(vacancy) {
      return _UserService.getUserById(vacancy.responsibleId);
   }

   _getChildVacancies(vacancy) {
      if (vacancy.childVacanciesIds.length) {
         let promises = map(vacancy.childVacanciesIds, (childVacancyId) => {
            return _VacancyService.getVacancy(childVacancyId);
         });
         return _$q.all(promises);
      }
   }

   _getClosingCandidate(vacancy) {
      if (vacancy.closingCandidateId) {
         return _CandidateService.getCandidate(vacancy.closingCandidateId);
      }
   }

   _getThesauruses(vacancy) {
      let promises = reduce(THESAURUS, (memo, {thesaurusName, serverField, clientField}) => {
         memo[clientField] = _ThesaurusService.getThesaurusTopicsByIds(thesaurusName, vacancy[serverField]);
         vacancy[serverField] = _convertIdsToString(vacancy[serverField]);
         return memo;
      }, {});
      return _$q.all(promises);
   }

   _fillVacancyLanguageSkills(vacancy) {
      vacancy.languageSkill = vacancy.languageSkill || {};
      let languageLevelPromise = _ThesaurusService.getThesaurusTopic('languageLevel',
                                                                     vacancy.languageSkill.languageLevel);
      let languagePromise = _ThesaurusService.getThesaurusTopic('language', vacancy.languageSkill.languageId);
      return _$q.all([languageLevelPromise, languagePromise]);
   }
}

function _convertFromServerDates(vacancy) {
   each(DATE_TYPE, (type) => {
      vacancy[type] = utils.formatDateFromServer(vacancy[type]);
   });
   return vacancy;
}

function _convertToServerDates(vacancy) {
   each(DATE_TYPE, (type) => {
      if (vacancy[type]) {
         vacancy[type] = utils.formatDateToServer(vacancy[type]);
      };
   });
   return vacancy;
}

function _convertIdsToString(data) {
   return isNumber(data) ? toString(data) : data;
}

function toString(data) {
   return data ? `${data}` : null;
}

function _convertThesaurusToIds(vacancy) {
   each(THESAURUS, ({serverField, clientField, needConvertForServer}) => {
      if (needConvertForServer) {
         vacancy[serverField] = map(vacancy[clientField], 'id');
      }
      delete vacancy[clientField];
   });
   return vacancy;
}

function _saveNewTopics(vacancy) {
   let promises = reduce(THESAURUS, (memo, helper) => {
      let list = filter(vacancy[helper.clientField], {id: undefined});

      if (list.length) {
         remove(vacancy[helper.clientField], {id: undefined});
         memo[helper.clientField] = _ThesaurusService.saveThesaurusTopics(helper.thesaurusName, list);
      }

      return memo;
   }, {});

   return _$q.all(promises);
}

function ThesaurusHelper(thesaurusName, serverField, clientField, needConvertForServer) {
   this.thesaurusName   = thesaurusName;
   this.serverField     = serverField;
   this.clientField     = clientField;
   this.needConvertForServer = needConvertForServer;
}
