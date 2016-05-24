import utils  from '../utils.js';
import {
   assign,
   isNumber,
   filter,
   remove,
   each,
   map,
   union,
   cloneDeep,
   first,
   reduce,
   result
} from 'lodash';
const VACANCY_URL = 'vacancies/';
const DATE_TYPE = ['startDate', 'deadlineDate', 'endDate', 'createdOn'];

const THESAURUS = [
   new ThesaurusHelper('tags',   'tagIds',            'tags',           true),
   new ThesaurusHelper('skills', 'requiredSkillIds',  'requiredSkills', true),

   new ThesaurusHelper('industries',   'industryId',  'industries'),
   new ThesaurusHelper('levels',       'levelIds',    'levels'),
   new ThesaurusHelper('locations',    'locationIds', 'locations'),
   new ThesaurusHelper('departments',  'departmentId', 'departments'),
   new ThesaurusHelper('entityStates', 'state',       'entityStates'),
   new ThesaurusHelper('typesOfEmployment', 'typeOfEmployment', 'typesOfEmployment')
];

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

   search(condition) {
      _LoggerService.debug('search vacancies', condition);
      const searchUrl = 'search';
      const additionalUrl = VACANCY_URL + searchUrl;
      return _HttpService.post(additionalUrl, condition).then(response => {
         return _$q.all(map(response.vacancies, this.convertFromServerFormat)).then((vacancies) => {
            response.vacancies = vacancies;
            return response;
         });
      });
   }

   convertFromServerFormat(vacancy) {
      vacancy = _convertFromServerDates(vacancy);
      vacancy.responsibleId = toString(vacancy.responsibleId);
      return _$q.all([_fillThesauruses(vacancy), _fillUser(vacancy)]).then(first);
   }

   remove(vacancy) {
      if (vacancy.id) {
         const additionalUrl = VACANCY_URL + vacancy.id;
         return _HttpService.remove(additionalUrl, vacancy);
      } else {
         _LoggerService.debug('Can\'t remove new vacancy', vacancy);
         return _$q.when(true);
      }
   }

   save(vacancy) {
      console.log('vacancy', vacancy);
      vacancy = cloneDeep(vacancy);

      return _saveNewTopics(vacancy).then((storedTopics) => {
         each(storedTopics, (list, fieldName) => {
            vacancy[fieldName] = union(vacancy[fieldName] || [], list);
         });
         vacancy = _convertThesaurusToIds(vacancy);
         vacancy = _convertToServerDates(vacancy);

         delete vacancy.createdOn;
         delete vacancy.responsible;
         vacancy.languageSkill = vacancy.languageSkill || {};
         console.log('result', result(vacancy, 'languageSkill.languageId'));
         if (result(vacancy, 'languageSkill.languageId')) {
//            toNumber(result(vacancy, 'languageSkill.languageId')) === 0 ||
//            isNaN(toNumber(result(vacancy, 'languageSkill.languageId')))
            vacancy.languageSkill.languageLevel = parseInt(vacancy.languageSkill.languageLevel);
            vacancy.languageSkill.languageId = parseInt(vacancy.languageSkill.languageId);
         } else {
            vacancy.languageSkill = null;
         }
         vacancy.responsibleId = parseInt(vacancy.responsibleId);

         if (vacancy.id) {
            const additionalUrl = VACANCY_URL + vacancy.id;
            return _HttpService.put(additionalUrl, vacancy);
         } else {
            return _HttpService.post(VACANCY_URL, vacancy);
         }
      }).then(this.convertFromServerFormat);
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

function _fillUser(vacancy) {
   return _UserService.getUserById(vacancy.responsibleId).then((user) => vacancy.responsible = user);
}

function _fillThesauruses(vacancy) {
   let promises = reduce(THESAURUS, (memo, {thesaurusName, serverField, clientField}) => {
      memo[clientField] = _ThesaurusService.getThesaurusTopicsByIds(thesaurusName, vacancy[serverField]);
      vacancy[serverField] = _convertIdsToString(vacancy[serverField]);
      return memo;
   }, {});

   vacancy.languageSkill = vacancy.languageSkill || {};
   vacancy.languageSkill.languageLevel = toString(vacancy.languageSkill.languageLevel);
   vacancy.languageSkill.languageId = toString(vacancy.languageSkill.languageId);

   return _$q.all(promises).then(data => assign(vacancy, data));
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
