import {
   stubTrue,
   map,
   head,
   cond,
   trim,
   find
} from 'lodash';
let _UserService, _$q, _ThesaurusService, _vm;

const FIELD_TYPES = {
   'Plain' : 1,
   'Array' : 2,
   'CandidatesProgress' : 3,
   'VacanciesProgress' : 4
};
const EMPTY = '*empty*';
const moment = require('moment');

export default class LogginService {
   constructor(UserService, ThesaurusService, $q) {
      'ngInject';
      _UserService = UserService;
      _$q = $q;
      _ThesaurusService = ThesaurusService;
   }

   toReadableFormat(history, vm) {
      _vm = vm;
      return _$q.all(map(history, convert));
   }
}

function convert(log) {
   return _UserService.getUserById(log.userId).then(user => {
      log.user = user;
   }).then(() => {
      return getAndPerformStrategy(log);
   });
}

function getAndPerformStrategy(log) {
   return cond([
        [isVSICase, fromVSI],
        [isArrayCase, fromArray],
        [stubTrue, fromSimple]
   ])(log);
}

function isVSICase(log) {
   return log.fieldType === FIELD_TYPES.VacanciesProgress || log.fieldType === FIELD_TYPES.CandidatesProgress;
}
function isArrayCase(log) {
   return log.fieldType === FIELD_TYPES.Array;
}
function fromVSI(log) {
   if (log.fieldType === FIELD_TYPES.VacanciesProgress) {
      log.vacancyTitle = getVacancyTitleFrom(log);
      log.stageTitle = getStageTitleFrom(log, 'vacancyId');
      return _$q.when({
         createdOn: log.createdOn,
         user: log.user.lastName,
         field: `${log.vacancyTitle} stage`,
         value: log.stageTitle
      });
   } else {
      log.candidateLastName = getCandidateLastNameFrom(log);
      log.stageTitle = getStageTitleFrom(log, 'candidateId');
      return _$q.when({
         createdOn: log.createdOn,
         user: log.user.lastName,
         field: `${log.candidateLastName} stage`,
         value: log.stageTitle
      });
   }
}
function fromArray(log) {
   if (/cities/i.test(log.field)) {
      return getFieldNameAndValuesFor('city', log.values).then(fieldValue => {
         return Object.assign(fieldValue, {
            createdOn: log.createdOn,
            user: log.user.lastName
         });
      });
   } else if (/levels/i.test(log.field)) {
      return getFieldNameAndValuesFor('level', log.values).then(fieldValue => {
         return Object.assign(fieldValue, {
            createdOn: log.createdOn,
            user: log.user.lastName
         });
      });
   }
}
function getFieldNameAndValuesFor(field, values) {
   return _ThesaurusService.getThesaurusTopics(field).then(thesaurus => {
      let titles = map(values, thesaurusId => {
         return find(thesaurus, ['id', parseInt(thesaurusId)]).title;
      });
      return {
         field: /city/i.test(field) ? 'Cities' : 'Levels',
         value: titles.length ? titles.join(', ') : EMPTY
      };
   });
}
function fromSimple(log) {
   if (/id/i.test(log.field)) {
      log.field = trim(log.field, 'Id');
   }
   return getNewValue(log).then((newValueOfField) => {
      return {
         createdOn: log.createdOn,
         user: log.user.lastName,
         field: log.field,
         value: newValueOfField
      };
   });
}

function getNewValue(log) {
   let deffered = _$q.defer();
   if (/city/i.test(log.field)) {
      let cityId = parseInt(head(log.values));
      _ThesaurusService.getThesaurusTopic('city', cityId).then(city => {
         deffered.resolve(city.title);
      });
   } else if (/department/i.test(log.field)) {
      let departmentId = parseInt(head(log.values));
      _ThesaurusService.getThesaurusTopic('department', departmentId).then(department => {
         deffered.resolve(department.title);
      });
   } else if (/responsible/i.test(log.field)) {
      let responsibleId = parseInt(head(log.values));
      _UserService.getUserById(responsibleId).then(user => {
         deffered.resolve(user.lastName);
      });
   } else {
      if (/date/i.test(log.field)) {
         log.values = map(log.values, val => {
            if (moment(val).isValid()) {
               return moment(val).toISOString();
            } else {
               return val;
            }

         });
      }
      deffered.resolve(head(log.values));
   }
   return deffered.promise;
}


function getCandidateLastNameFrom(log) {
   let vsi = find(_vm.vacancyStageInfosComposedByCandidateIdVacancyId, ['candidateId', parseInt(log.field)]);
   if (!vsi) {
        //TODO: case when user removed appropriate vacancy from
        //candidate vacancies list, so we should get it from VacancyService
   }
   return vsi.candidate.lastName;
}

function getVacancyTitleFrom(log) {
   let vsi = find(_vm.vacancyStageInfosComposedByCandidateIdVacancyId, ['vacancyId', parseInt(log.field)]);
   if (!vsi) {
        //TODO: case when user removed appropriate vacancy from
        //candidate vacancies list, so we should get it from VacancyService
   }
   return vsi.vacancy.title;
}
function getStageTitleFrom(log, entityIdName) {
   let vsi = find(_vm.vacancyStageInfosComposedByCandidateIdVacancyId, [entityIdName, parseInt(log.field)]);
   let stage = find(vsi.stageFlow, ['stage.id', parseInt(head(log.values))]);
   return stage.stage.title;
}
