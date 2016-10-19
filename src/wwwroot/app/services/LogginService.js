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
const VALUES_INDEXES = {
   NEW_VALUE: 0,
   PAST_VALUE: 1
};
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
      return _$q.when({
         createdOn: log.createdOn,
         user: `${log.user.lastName} ${log.user.firstName}`,
         field: `Stage progress of ${log.vacancyTitle}`,
         newValue: getNewTitleFrom(log, 'vacancyId'),
         pastValue: getPastTitleFrom(log, 'vacancyId')
      });
   } else {
      log.candidateLastName = getCandidateLastNameFrom(log);
      return _$q.when({
         createdOn: log.createdOn,
         user: `${log.user.lastName} ${log.user.firstName}`,
         field: `Stage progress of ${log.candidateLastName}`,
         newValue: getNewTitleFrom(log, 'candidateId'),
         pastValue: getPastTitleFrom(log, 'candidateId')
      });
   }
}
function fromArray(log) {
   if (/cities/i.test(log.field)) {
      return getFieldNameAndValuesFor('city', log.newValues, log.pastValues).then(fieldValue => {
         return Object.assign(fieldValue, {
            createdOn: log.createdOn,
            user: `${log.user.lastName} ${log.user.firstName}`
         });
      });
   } else if (/levels/i.test(log.field)) {
      return getFieldNameAndValuesFor('level', log.newValues, log.pastValues).then(fieldValue => {
         return Object.assign(fieldValue, {
            createdOn: log.createdOn,
            user: `${log.user.lastName} ${log.user.firstName}`
         });
      });
   }

}
function getFieldNameAndValuesFor(field, newValues, pastValues) {
   return _ThesaurusService.getThesaurusTopics(field).then(thesaurus => {
      let newTitles = map(newValues, thesaurusId => {
         return find(thesaurus, ['id', parseInt(thesaurusId)]).title;
      });
      let pastTitles = map(pastValues, thesaurusId => {
         return find(thesaurus, ['id', parseInt(thesaurusId)]).title;
      });
      return {
         field: /city/i.test(field) ? 'Cities' : 'Levels',
         newValue: newTitles.length ? newTitles.join(', ') : EMPTY,
         pastValue: pastTitles.length ? pastTitles.join(', ') : EMPTY
      };
   });
}
function fromSimple(log) {
   if (/id/i.test(log.field)) {
      log.field = trim(log.field, 'Id');
   }
   return getNewAndPastValue(log).then((valuesContainer) => {
      return {
         createdOn: log.createdOn,
         user: `${log.user.lastName} ${log.user.firstName}`,
         field: log.field,
         newValue: valuesContainer.newValue,
         pastValue: valuesContainer.pastValue
      };
   });
}

function getThesaurusTitle(thesaurusName, id) {
   return _ThesaurusService.getThesaurusTopic(thesaurusName, id).then(thesaurus => {
      return thesaurus.title;
   });
}
function getNewAndPastForThesaurus(thesaurusName, log) {
   return _$q.all([
      getThesaurusTitle(thesaurusName, parseInt(head(log.newValues))),
      getThesaurusTitle(thesaurusName, parseInt(head(log.pastValues)))
   ]).then(valuesContainer => {
      return {
         newValue: valuesContainer[VALUES_INDEXES.NEW_VALUE],
         pastValue: valuesContainer[VALUES_INDEXES.PAST_VALUE]
      };
   });
}

function getNewAndPastValue(log) {
   if (/city/i.test(log.field)) {
      return getNewAndPastForThesaurus('city', log);
   } else if (/department/i.test(log.field)) {
      return getNewAndPastForThesaurus('department', log);
   } else if (/responsible/i.test(log.field)) {
      let newResponsibleId = parseInt(head(log.values));
      return _UserService.getUserById(newResponsibleId).then(user => {
         return user.lastName;
      });
   } else {
      debugger;
      if (/date/i.test(log.field)) {
         log.newValues = map(log.newValues, val => {
            if (moment(val).isValid()) {
               return moment(val).toISOString();
            } else {
               return val;
            }
         });
         log.pastValues = map(log.pastValues, val => {
            if (moment(val).isValid()) {
               return moment(val).toISOString();
            } else {
               return val;
            }
         });
      }
      return _$q.when({
         newValue: head(log.newValues),
         pastValue: head(log.pastValues)
      });
   }
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

function getNewTitleFrom(log, entityIdName) {
   let vsi = find(_vm.vacancyStageInfosComposedByCandidateIdVacancyId, [entityIdName, parseInt(log.field)]);
   let stage = find(vsi.stageFlow, ['stage.id', parseInt(head(log.newValues))]);
   return stage.stage.title;
}
function getPastTitleFrom(log, entityIdName) {
   let vsi = find(_vm.vacancyStageInfosComposedByCandidateIdVacancyId, [entityIdName, parseInt(log.field)]);
   let stage = find(vsi.stageFlow, ['stage.id', parseInt(head(log.pastValues))]);
   return stage.stage.title;
}
