import {
   stubTrue,
   map,
   head,
   cond,
   trim,
   find,
   assign
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

export default class UserHistoryService {
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
   return (() => {
      if (log.fieldType === FIELD_TYPES.VacanciesProgress) {
         return _$q.when({
            entityName: getVacancyTitleFrom(log),
            newValue: getStageTitle(log.field, log.newValues, 'vacancyId'),
            pastValue: getStageTitle(log.field, log.pastValues, 'vacancyId')
         });
      } else {
         return _$q.when({
            entityName: getCandidateLastNameFrom(log),
            newValue: getStageTitle(log.field, log.newValues, 'candidateId'),
            pastValue: getStageTitle(log.field, log.pastValues, 'candidateId')
         });
      }
   })().then((nameValuesContainer) => {
      return {
         createdOn: log.createdOn,
         user: `${log.user.lastName} ${log.user.firstName}`,
         field: `Stage progress of ${nameValuesContainer.entityName}`,
         newValue: `${nameValuesContainer.newValue}`,
         pastValue: `${nameValuesContainer.pastValue}`
      };
   });
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
function fromArray(log) {
   return (() => {
      if (/cities/i.test(log.field)) {
         return getFieldNameAndValuesFor('city', log.newValues, log.pastValues);
      } else if (/levels/i.test(log.field)) {
         return getFieldNameAndValuesFor('level', log.newValues, log.pastValues);
      }
   })().then(nameValuesContainer => {
      return assign(nameValuesContainer, {
         createdOn: log.createdOn,
         user: `${log.user.lastName} ${log.user.firstName}`
      });
   });
}
function getFieldNameAndValuesFor(field, newValues, pastValues) {
   return _ThesaurusService.getThesaurusTopics(field).then(thesaurus => {
      return _$q.all([
         map(newValues, thesaurusId => {
            return find(thesaurus, ['id', parseInt(thesaurusId)]).title;
         }),
         map(pastValues, thesaurusId => {
            return find(thesaurus, ['id', parseInt(thesaurusId)]).title;
         })
      ]).then((promiseArray) => {
         return {
            field: /city/i.test(field) ? 'Cities' : 'Levels',
            newValue:  promiseArray[VALUES_INDEXES.NEW_VALUE].length ?
               promiseArray[VALUES_INDEXES.NEW_VALUE].join(', ') :
               EMPTY,
            pastValue: promiseArray[VALUES_INDEXES.PAST_VALUE].length ?
               promiseArray[VALUES_INDEXES.PAST_VALUE].join(', ') :
               EMPTY
         };
      });
   });
}
function fromSimple(log) {
   if (containsId(log.field)) {
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
function getNewAndPastValue(log) {
   if (containsCity(log.field)) {
      return getNewAndPastForThesaurus('city', log);
   } else if (containsDepartment(log.field)) {
      return getNewAndPastForThesaurus('department', log);
   } else if (containsResponsible(log.field)) {
      return _$q.all(
         [_UserService.getUserById(parseInt(head(log.newValues))).then(user => {
            return user ? user.lastName : EMPTY;
         }),
          _UserService.getUserById(parseInt(head(log.pastValues))).then(user => {
             return user ? user.lastName : EMPTY;
          })
         ]).then(valuesContainer => {
            return {
               newValue: valuesContainer[VALUES_INDEXES.NEW_VALUE],
               pastValue: valuesContainer[VALUES_INDEXES.PAST_VALUE]
            };
         });
   } else {
      if (containsDate(log.field)) {
         log.newValues = getDateValues(log.newValues);
         log.pastValues = getDateValues(log.pastValues);
      }
      return _$q.when({
         newValue: head(log.newValues),
         pastValue: head(log.pastValues)
      });
   }
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
function getThesaurusTitle(thesaurusName, id) {
   return _ThesaurusService.getThesaurusTopic(thesaurusName, id).then(thesaurus => {
      return thesaurus ? thesaurus.title : EMPTY;
   });
}
function getDateValues(values) {
   return map(values, val => {
      return moment(val).isValid() ? moment(val).toISOString() : val;
   });
}
function getStageTitle(field, values, entityIdName) {
   let vsi = find(_vm.vacancyStageInfosComposedByCandidateIdVacancyId, [entityIdName, parseInt(field)]);
   let extendedStage = find(vsi.stageFlow, ['stage.id', parseInt(head(values))]);
   return extendedStage ? extendedStage.stage.title : head(values);
}
function containsId(field) {
   return /id/i.test(field);
}
function containsCity(field) {
   return /city/i.test(field);
}
function containsDepartment(field) {
   return /department/i.test(field);
}
function containsResponsible(field) {
   return /responsible/i.test(field);
}
function containsDate(field) {
   return /date/i.test(field);
}
