import template from './candidate-vacancy-info.directive.html';
import './candidate-vacancy-info.scss';
import manyStageCommentDialogTemplate from './many-stage-comment-adding-dialog.template.html';
import muiltiPassDialogTemplate from './multi-pass-vacancy-selector.template.html';
import hireDateDialogTemplate from './hire-date-dialog.template.html';
let moment = require('moment');
import {
   map,
   filter,
   find,
   cloneDeep,
   reduce,
   maxBy,
   some,
   head,
   isNil,
   take,
   each,
   uniq,
   assign,
   includes
} from 'lodash';
import utils from '../../utils';

export default class CandidateVacancyInfoDirective {
   constructor() {
      this.restrict = 'E';
      this.template = template;
      this.scope = {
         parentEntity: '@',
         directiveType: '@',
         vacancyStages: '=',
         promise: '=',
         vacancyStageInfosComposedByCandidateIdVacancyId: '=',
         closevacancy: '=',
         currentUser: '='
      };
      this.controller = CandidateVacancyInfoController;
   }
   static createInstance() {
      'ngInject';
      CandidateVacancyInfoDirective.instance = new CandidateVacancyInfoDirective();
      return CandidateVacancyInfoDirective.instance;
   }
}

function CandidateVacancyInfoController($scope, // eslint-disable-line max-statements
   UserDialogService,
   $translate,
   $q,
   $timeout,
   TransitionsService
) {
   'ngInject';
   const vm = $scope;
   const STAGE_STATES = {
      'Inactive': 0,
      'Active': 1,
      'Passed': 2,
      'NotPassed': 3
   };
   vm.stageStates = STAGE_STATES;
   const STAGE_TYPES = {
      'MainStage': 1,
      'HireStage': 2,
      'RejectStage': 3
   };
   const VALIDATION_ERROR = {
      'Comment' : 1,
      'NotPassed' : 2
   };
   const ENTITY_STATES = {
      Inactive     : 1,
      Active       : 2,
      Verfied      : 3,
      Unverified   : 4,
      Pending      : 5,
      Open         : 6,
      Processing   : 7,
      Closed       : 8,
      Cancelled    : 9
   };
   const SHOW_LIMIT = 7;
   const DATE_FORMAT = 'DD-MM-YYYY';
   const PARENT_VACANCY = 'vacancy';
   const PARENT_CANDIDATE = 'candidate';

   vm.vacancyStageInfoComposedObjectsToShow = [];
   vm.stagesToShow = [];
   vm.latestQuery = [];
   let factor = 1;

   splitForDisplay(vm.vacancyStageInfosComposedByCandidateIdVacancyId)
      .then(splittedArrayOfObjectsToShow => {
         vm.latestQuery = vm.vacancyStageInfosComposedByCandidateIdVacancyId;
         vm.vacancyStageInfoComposedObjectsToShow = splittedArrayOfObjectsToShow;
      });
   vm.stageQueries = [];

   (() => {
      if (vm.parentEntity === PARENT_VACANCY) {
         vm.rejectStages = filter(vm.vacancyStages, ['stage.stageType', STAGE_TYPES.RejectStage]);
         vm.stagesToShow = calculateVacancyStagesEntitiesCount(
            vm.vacancyStageInfosComposedByCandidateIdVacancyId);
      } else if (vm.parentEntity === PARENT_CANDIDATE) {
         vm.vacancyStageInfosComposedByCandidateIdVacancyId = map(
            vm.vacancyStageInfosComposedByCandidateIdVacancyId, findAndSetRejectStagesFor);
         if (some(vm.vacancyStageInfosComposedByCandidateIdVacancyId)) {
            //TODO intersect of stages
            vm.stagesToShow = calculateVacancyStagesEntitiesCount(vm.vacancyStageInfosComposedByCandidateIdVacancyId);
         }
      }
   })();

   function splitForDisplay(objectsToShow, multiplicateFactor = 1) {
      return $q.when(take(objectsToShow, SHOW_LIMIT * multiplicateFactor));
   }
   vm.isThereMore = () => {
      return vm.vacancyStageInfoComposedObjectsToShow.length !== vm.latestQuery.length;
   };
   vm.showMoreAttachedEntities = () => {
      splitForDisplay(vm.latestQuery, ++factor)
         .then(splittedArrayOfObjectsToShow => {
            vm.vacancyStageInfoComposedObjectsToShow = splittedArrayOfObjectsToShow;
         });
   };

   function findAndSetRejectStagesFor(vacancyStage) {
      return Object.assign(vacancyStage, {
         rejectStages: filter(vacancyStage.stageFlow, ['stage.stageType', STAGE_TYPES.RejectStage])
      });
   }

   function calculateVacancyStagesEntitiesCount(vacancyStageInfosComposedByCandidateIdVacancyId) {
      let stagesFlow = vm.vacancyStages || head(vacancyStageInfosComposedByCandidateIdVacancyId).stageFlow;
      let withoutHireStage = filter(stagesFlow, extStage => extStage.stage.stageType !== STAGE_TYPES.HireStage);
      return map(withoutHireStage, (extStage) => {
         let stageEntitiesCount = reduce(vacancyStageInfosComposedByCandidateIdVacancyId, (count, vsi) => {
            return vsi.currentStageId === extStage.stage.id ? count + 1 : count;
         }, 0);
         return Object.assign(extStage, {
            entitiesCount: stageEntitiesCount,
            _hasEntities: stageEntitiesCount !== 0,
            _isPressed: false
         });
      });
   }


   vm.getCommentForStageId = (stageObject, stageId) => {
      return find(stageObject.vacancyStageInfos, ['stageId', stageId]).comment.message;
   };

   vm.isCommentOfStageIdAvalaible = (stageObject, stageId) => {
      return some(stageObject.vacancyStageInfos, vsi =>
         vsi.stageId === stageId &&
         vsi.comment &&
         vsi.comment.message);
   };

   vm.callStagesDialog = (entityStageObject) => {
      let stageFlow = entityStageObject.stageFlow ? entityStageObject.stageFlow : vm.vacancyStages;
      showStagesFlowDialogFor(entityStageObject, stageFlow)
         .then(notifySuccess);
   };

   function updateCandidateStagesForWith(entityStageObject, vacancyStagesEntitiesVSIs) {
      if (vacancyStagesEntitiesVSIs) {
         entityStageObject.vacancyStageInfos = map(filter(vacancyStagesEntitiesVSIs, stageVsi =>
               !isNil(stageVsi.vsi)),
            stageVsi => stageVsi.vsi);
      }
      recalculateCurrentStageId(entityStageObject);
      vm.stagesToShow = calculateVacancyStagesEntitiesCount(vm.vacancyStageInfosComposedByCandidateIdVacancyId);
      return $q.when(entityStageObject);
   }

   function getComposedThatCanBeMultiPassed(sampleStageObject) {
      return filter(vm.vacancyStageInfosComposedByCandidateIdVacancyId, x => {
         return x.vacancyId === sampleStageObject.vacancyId ?
            false :
            getVSIsToPass(x, sampleStageObject.vacancyStageInfos).length;
      });
   }

   function showVacancySelectorDialog(sampleStageObject) {
      let multiPassDeffered = $q.defer();
      let canMultiPass = getComposedThatCanBeMultiPassed(sampleStageObject);
      UserDialogService.dialog($translate.instant('COMMON.VACANCY_SELECTY_MULTIPASS_DIALOG_TITLE'),
         muiltiPassDialogTemplate, [{
            name: $translate.instant('COMMON.CANCEL'),
            func: () => {
               clearToogle(canMultiPass);
               multiPassDeffered.reject();
            }
         }, {
            name: $translate.instant('COMMON.APLY'),
            func: () => {
               let toMultiPass = getObjectsToPass(canMultiPass);
               each(toMultiPass, composedObjectToPass => {
                  composedObjectToPass.vacancyStageInfos = intersectOldWithSample(composedObjectToPass,
                     sampleStageObject.vacancyStageInfos);
               });
               clearToogle(canMultiPass);
               multiPassDeffered.resolve(toMultiPass);
            }
         }], {
            canMultiPass,
            toogleComposedObject: (composedObject) => {
               composedObject.toogled = !composedObject.toogled;
            }
         });
      return multiPassDeffered.promise;
   }

   function createNewVsiForComposed(vsiToPass, composedObjectToPass) {
      let newVsi = assign({}, vsiToPass, {
         vacancyId: composedObjectToPass.vacancyId
      });
      delete newVsi.id;
      return newVsi;
   }

   function intersectOldWithSample(composedObjectToPass, sampleVacancyStageInfos) {
      let newVSIs = map(getVSIsToPass(composedObjectToPass, sampleVacancyStageInfos), vsiToPass =>
         createNewVsiForComposed(vsiToPass, composedObjectToPass)
      );
      let combinedVSIs = intersectOldAndNewVsis(composedObjectToPass, newVSIs);
      if (!isThereActiveStage(combinedVSIs)) {
         combinedVSIs.push(getActiveStage(combinedVSIs, composedObjectToPass));
      }
      return combinedVSIs;
   }

   function intersectOldAndNewVsis(composedObject, newVSIs) {
      let newAndOld = [...composedObject.vacancyStageInfos, ...newVSIs];
      let uniqStages = uniq(map(newAndOld, 'stageId'));
      let intersection = map(uniqStages, stageId => {
         let newVSI = find(newVSIs, ['stageId', stageId]);
         let oldVSI = find(composedObject.vacancyStageInfos, ['stageId', stageId]);
         if (!newVSI) {
            return oldVSI;
         }
         if (oldVSI) {
            return assign({}, oldVSI, {
               vacancyId: composedObject.vacancyId,
               stageState: newVSI.stageState,
               comment: newVSI.comment
            });
         }
         let newObjectOfnewVSI = assign({}, newVSI, {
            vacancyId: composedObject.vacancyId
         });
         delete newObjectOfnewVSI.id;
         return newObjectOfnewVSI;
      });
      return intersection;
   }

   function getObjectsToPass(objectThatCanBeMultiPassed) {
      return filter(objectThatCanBeMultiPassed, 'toogled');
   }

   function clearToogle(objectThatCanBeMultiPassed) {
      each(objectThatCanBeMultiPassed, x => delete x.toogled);
   }

   function isThereActiveStage(combinedVSIs) {
      return find(combinedVSIs, ['stageState', STAGE_STATES.Active]);
   }

   function getActiveStage(combinedVSIs, composedObject) {
      let latestStageId = reduce(combinedVSIs, (max, vsi) => {
         return max < vsi.stageId && vsi.stageState === STAGE_STATES.Passed ?
            vsi.stageId :
            max;
      }, 0);
      let currentStageId = latestStageId + 1;
      let currentStage = find(composedObject.stageFlow, ['stage.id', currentStageId]);
      return {
         vacancyId: composedObject.vacancyId,
         candidateId: composedObject.candidateId,
         stage: currentStage,
         stageId: currentStageId,
         stageState: STAGE_STATES.Active
      };
   }

   function getVSIsToPass(checkingStageObject, sampleVacancyStageInfos) {
      let passedVSIsOfSampleObject = filter(sampleVacancyStageInfos, ['stageState', STAGE_STATES.Passed]);
      let passedVSIsOfCheckingObject = filter(checkingStageObject.vacancyStageInfos,
         ['stageState', STAGE_STATES.Passed]);
      return filter(passedVSIsOfSampleObject, (VSIsThatCanBePassed, vsi) => {
         let foundedVsi = find(passedVSIsOfCheckingObject, ['stageId', vsi.stageId]);
         return !foundedVsi || foundedVsi.stageState !== STAGE_STATES.Passed;
      });
   }

   function showStagesFlowDialogFor(entityStageObject, vacancyStages) {
      let stagesDeffered = $q.defer();
      let mainStages = filter(vacancyStages, ['stage.stageType', STAGE_TYPES.MainStage]);
      let vacancyStagesEntitiesVSIs = getVacancyStageInfosToEdit(entityStageObject, mainStages);
      let backupVSI = cloneDeep(vacancyStagesEntitiesVSIs);
      let rejectVacancyStageInfoesContainer = keepRejectVSIs(entityStageObject, vacancyStages);
      let hireVacancyStageInfoContainer = keepHireVSI(entityStageObject, vacancyStages);
      let scope = {
         vacancyStagesEntitiesVSIs,
         stageClick: vm.stageClick,
         entityStageObject
      };
      let buttons = [];
      buttons.push({
         name: $translate.instant('COMMON.CANCEL'),
         func: () => {
            vacancyStagesEntitiesVSIs = [
               ...(map(backupVSI, extVsi => {
                  if (extVsi.vsi && extVsi.vsi.dateOfPass) {
                     extVsi.vsi.dateOfPass = utils.formatDateToServer(extVsi.vsi.dateOfPass);
                  }
                  return extVsi;
               })),
               ...rejectVacancyStageInfoesContainer,
               hireVacancyStageInfoContainer
            ];
            updateCandidateStagesForWith(entityStageObject, vacancyStagesEntitiesVSIs).then(() => {
               stagesDeffered.reject();
            });
         }
      });
      if (vm.parentEntity === 'candidate') {
         buttons.push({
            name: $translate.instant('Pass on multiple vacancies'),
            func: () => {
               let updatedStagesWithRejectedAndHire = [
                  ...(map(vacancyStagesEntitiesVSIs, extVsi => {
                     if (extVsi.vsi && extVsi.vsi.dateOfPass) {
                        extVsi.vsi.dateOfPass = utils.formatDateToServer(extVsi.vsi.dateOfPass);
                     }
                     return extVsi;
                  })),
                  ...rejectVacancyStageInfoesContainer,
                  hireVacancyStageInfoContainer
               ];
               updateCandidateStagesForWith(entityStageObject, updatedStagesWithRejectedAndHire)
                  .then(newStageObject => {
                     showVacancySelectorDialog(newStageObject).then(updatedComposedVSIs => {
                        each(updatedComposedVSIs, composedVsi => updateCandidateStagesForWith(composedVsi));
                        stagesDeffered.resolve();
                     })
                        .catch(() => {
                           vacancyStagesEntitiesVSIs = [
                              ...(map(backupVSI, extVsi => {
                                 if (extVsi.vsi && extVsi.vsi.dateOfPass) {
                                    extVsi.vsi.dateOfPass = utils.formatDateToServer(extVsi.vsi.dateOfPass);
                                 }
                                 return extVsi;
                              })),
                              ...rejectVacancyStageInfoesContainer,
                              hireVacancyStageInfoContainer
                           ];
                           updateCandidateStagesForWith(entityStageObject, vacancyStagesEntitiesVSIs);
                        });
                  });
            }
         });
      }
      buttons.push({
         name: $translate.instant('COMMON.APLY'),
         func: () => {
            let updatedStagesWithRejectedAndHire = [
               ...(map(vacancyStagesEntitiesVSIs, extVsi => {
                  if (extVsi.vsi && extVsi.vsi.dateOfPass) {
                     extVsi.vsi.dateOfPass = utils.formatDateToServer(extVsi.vsi.dateOfPass);
                  }
                  return extVsi;
               })),
               ...rejectVacancyStageInfoesContainer,
               hireVacancyStageInfoContainer
            ];
            updateCandidateStagesForWith(entityStageObject, updatedStagesWithRejectedAndHire).then(() => {
               stagesDeffered.resolve();
            });
         }
      });
      UserDialogService.dialog($translate.instant('Candidate stages'),
         manyStageCommentDialogTemplate, buttons, scope, false);
      return stagesDeffered.promise;
   }

   function getVacancyStageInfosToEdit(entityStageObject, mainStages) {
      return cloneDeep(map(mainStages, (vacancyStage) => {
         let showCommentArea = false;
         let stageVacancyStageInfo = find(entityStageObject.vacancyStageInfos, {
            stageId: vacancyStage.stage.id
         });
         let stageState = STAGE_STATES.Inactive;
         let vsiClass = '';
         if (stageVacancyStageInfo) {
            stageVacancyStageInfo.stage = vacancyStage.stage;
            switch (stageVacancyStageInfo.stageState) {
               case STAGE_STATES.Active:
                  {
                     stageState = STAGE_STATES.Active;
                     vsiClass = 'active';
                     break;
                  }
               case STAGE_STATES.Passed:
                  {
                     stageState = STAGE_STATES.Passed;
                     vsiClass = 'passed';
                     break;
                  }
               case STAGE_STATES.NotPassed:
                  {
                     stageState = STAGE_STATES.NotPassed;
                     vsiClass = 'not-passed';
                     break;
                  }
               default:
                  {
                     stageState = STAGE_STATES.Inactive;
                     vsiClass = 'inactive';
                     break;
                  }
            }
            if (stageVacancyStageInfo.stage.commentField &&
            stageVacancyStageInfo.stageState !== STAGE_STATES.Inactive) {
               showCommentArea = true;
            }
            if (stageVacancyStageInfo.dateOfPass) {
               stageVacancyStageInfo.dateOfPass = moment(stageVacancyStageInfo.dateOfPass).format(DATE_FORMAT);
            } else {
               stageVacancyStageInfo.dateOfPass = moment().format(DATE_FORMAT);
            }
         }
         return {
            stage: vacancyStage,
            vsi: stageVacancyStageInfo,
            showCommentArea,
            stageState,
            class: vsiClass
         };
      }));
   }


   function keepRejectVSIs(entityStageObject, vacancyStages) {
      let rejectStages = filter(vacancyStages, ['stage.stageType', STAGE_TYPES.RejectStage]);
      return map(rejectStages, (extStage) => {
         return {
            stage: extStage,
            vsi: find(entityStageObject.vacancyStageInfos, {
               stageId: extStage.stage.id
            })
         };
      });
   }

   function keepHireVSI(entityStageObject, vacancyStages) {
      let hireStage = find(vacancyStages, ['stage.stageType', STAGE_TYPES.HireStage]);
      return {
         stage: hireStage,
         vsi: find(entityStageObject.vacancyStageInfos, {
            stageId: hireStage.stage.id
         })
      };
   }

   function getMainRequiredStagesIds (entityStageObject) {
      return map(filter(entityStageObject.stageFlow, extStage => {
         return extStage.stage.isRequired &&
            extStage.stage.stageType === STAGE_TYPES.MainStage;
      }), 'stage.id');
   }

   function mapStageToErrorObject(extStage, errorType) {
      return {
         errorType,
         stage: extStage.stage || extStage
      };
   }

   function getStage(vsi) {
      let stage = vsi.stage;
      if (stage) {
         if (stage.stage) {
            stage = vsi.stage.stage;
         }
      }
      return stage;
   }

   /**
    * Validates if candidate can be hired
    * @param  {Object} entityStageObject Candidate/Vacancy working object
    * @return {Object} returns the object with .valid error data or type 'valid' with empty data
    */
   function validateCandidateHiring (entityStageObject) {
      let requiredStages       = getMainRequiredStagesIds(entityStageObject);
      let requiredVSIs         = filter(entityStageObject.vacancyStageInfos, vsi =>
                                   includes(requiredStages, vsi.stageId));

      let passed               = filter(requiredVSIs, vsi =>
                                   vsi.stageState === STAGE_STATES.Passed ||
                                   vsi.stageState === STAGE_STATES.Active); //consider current stage is passed

      let stageWithOutComment  = map(filter(passed, vsi =>
                                    getStage(vsi).isCommentRequired &&
                                    vsi.comment && vsi.comment.message.length === 0), 'stage');
      let passedErrorData      = map(stageWithOutComment, stage =>
                                    mapStageToErrorObject(stage, VALIDATION_ERROR.Comment));

      let notPassed            = filter(requiredStages, stageId => !includes(map(passed, 'stageId'), stageId));
      let notPassedStages      = map(notPassed, stageId => find(entityStageObject.stageFlow, ['stage.id', stageId]));
      let notPassedErrorData   = map(notPassedStages, stage =>
                                    mapStageToErrorObject(stage, VALIDATION_ERROR.NotPassed));
      return {
         valid: !passedErrorData.length && !notPassedErrorData.length,
         data: [...passedErrorData, ...notPassedErrorData]
      };
   }

   let errorMessage = (dataObject) => {
      switch (dataObject.errorType) {
         case VALIDATION_ERROR.Comment:
            return `${dataObject.stage.title} left without comment`;
            break;
         case VALIDATION_ERROR.NotPassed:
            return `${dataObject.stage.title} left not passed`;
            break;
         default:
            return 'You face an unknown error, please, contact you personal programmist';
            break;
      }
   };

   vm.hire = (entityStageObject) => {
      let validationResult = validateCandidateHiring(entityStageObject);
      if (!validationResult.valid) {
         UserDialogService.notification(map(validationResult.data, errorMessage).join('\n'), 'warning');
         return;
      }
      callDatepickDialogFor(entityStageObject)
         .then(transferObject => {
            updateCurrentStage(entityStageObject);
            createHireStage(entityStageObject, transferObject.hireDateISO, transferObject.comment);
            recalculateCurrentStageId(entityStageObject);
            vm.stagesToShow = calculateVacancyStagesEntitiesCount(vm.vacancyStageInfosComposedByCandidateIdVacancyId);
         })
         .then(() => {
            if (vm.parentEntity === PARENT_VACANCY) {
               vm.closevacancy(entityStageObject.candidate);
            } else {
               entityStageObject.vacancy.closingCandidateId = entityStageObject.candidate.id;
               entityStageObject.vacancy.closingCandidate   = entityStageObject.candidate;
               entityStageObject.vacancy.state              = ENTITY_STATES.Closed;
            }
         });
   };

   function callDatepickDialogFor(candidateStage) {
      let dialogResult = $q.defer();
      let dialogTransferObject = {
         comment: ''
      };
      let scope = {
         dialogTransferObject,
         candidateStage,
         getTodayDate: () => {
            return moment().format(DATE_FORMAT);
         }
      };
      let buttons = [{
         name: $translate.instant('COMMON.CANCEL'),
         func: () => {
            dialogResult.reject();
         }
      }, {
         needValidate: true,
         name: $translate.instant('COMMON.APLY'),
         func: () => {
            let chosenDate = moment(dialogTransferObject.hireDate, DATE_FORMAT);
            dialogResult.resolve({
               hireDateISO: chosenDate.toISOString(),
               comment: dialogTransferObject.comment
            });
         }
      }];
      UserDialogService.dialog($translate.instant('Hiring'),
         hireDateDialogTemplate, buttons, scope);
      return dialogResult.promise;
   }

   function updateCurrentStage(candidateStage) {
      let currentVacancyStageInfo = find(candidateStage.vacancyStageInfos, {
         stageId: candidateStage.currentStageId
      });
      currentVacancyStageInfo.stageState = STAGE_STATES.Passed;
      currentVacancyStageInfo.dateOfPass = moment();
   }

   function createHireStage(entityStageObject, hireDateISO, comment) {
      let hireStage = find(entityStageObject.stageFlow, ['stage.stageType', STAGE_TYPES.HireStage]);
      let hiredVacancyStageInfo = {
         vacancyId: entityStageObject.vacancyId,
         candidateId: entityStageObject.candidateId,
         stage: hireStage,
         stageId: hireStage.stage.id,
         comment: {
            message: comment,
            authorId: vm.currentUser.id
         },
         stageState: STAGE_STATES.Active,
         createdOn: hireDateISO
      };
      entityStageObject.vacancyStageInfos.push(hiredVacancyStageInfo);
   }

   function callCommentDialogForReject () {
      let dialogResult = $q.defer();
      let dialogTransferObject = {
         comment: ''
      };
      let scope = {
         dialogTransferObject
      };
      let buttons = [{
         name: $translate.instant('COMMON.CANCEL'),
         func: () => dialogResult.reject()
      }, {
         needValidate: true,
         name: $translate.instant('COMMON.APLY'),
         func: () => dialogResult.resolve(dialogTransferObject.comment)
      }];
      UserDialogService.dialog($translate.instant('COMMON.COMMENT_FOR_REJECTING'),
         `<div><textarea ng-model="dialogTransferObject.comment" placeholder="Comment.." 
         validator="required" name="comment">
            </textarea></div>`, buttons, scope);
      return dialogResult.promise;
   }

   vm.reject = (candidateStage, rejectStage) => {
      let latestActiveVacancyStageInfo = find(candidateStage.vacancyStageInfos, {
         stageState: STAGE_STATES.Active
      });
      if (latestActiveVacancyStageInfo) {
         callCommentDialogForReject().then(comment => {
            latestActiveVacancyStageInfo.stageState = STAGE_STATES.NotPassed;
            let rejectedVsi = {
               vacancyId: candidateStage.vacancyId,
               candidateId: candidateStage.candidateId,
               stage: rejectStage.stage,
               stageId: rejectStage.stage.id,
               comment: {
                  message: comment,
                  authorId: vm.currentUser.id
               },
               stageState: STAGE_STATES.Active,
               createdOn: (new Date()).toISOString()
            };
            candidateStage.vacancyStageInfos.push(rejectedVsi);
            recalculateCurrentStageId(candidateStage);
            vm.stagesToShow = calculateVacancyStagesEntitiesCount(vm.vacancyStageInfosComposedByCandidateIdVacancyId);
         });
      }
   };

   function recalculateCurrentStageId(entityStageObject) {
      entityStageObject.currentStageId = currentStageOf(entityStageObject);
   }

   vm.candidateCount = (extStage) => {
      return extStage.candidates.length ? extStage.candidates.length : '';
   };

   vm.callRejectButton = (candidateStage) => {
      candidateStage.rejectButtonClicked = !candidateStage.rejectButtonClicked;
   };

   vm.goCandidate = (candidateId) => {
      TransitionsService.go('candidateProfile', {
         candidateId
      });
   };

   vm.goVacancy = (vacancyId) => {
      TransitionsService.go('vacancyView', {
         vacancyId
      });
   };

   vm.filterByStage = (selectedStage) => {
      selectedStage._isPressed = !selectedStage._isPressed;
      if (selectedStage._isPressed) {
         addToQuery(selectedStage);
      } else {
         removeFromQuery(selectedStage);
      }
      let queryResult = needToPerformQuery() ?
         performQuery() :
         vm.vacancyStageInfosComposedByCandidateIdVacancyId;
      vm.latestQuery = queryResult;
      splitForDisplay(queryResult).then(splittedArrayToShow => {
         vm.vacancyStageInfoComposedObjectsToShow = splittedArrayToShow;
      });
   };

   function performQuery() {
      return filter(vm.vacancyStageInfosComposedByCandidateIdVacancyId, (entityStageObject) => {
         return filter(vm.stageQueries, (extStage) => {
            return extStage.stage.id === entityStageObject.currentStageId;
         }).length;
      });
   }

   function needToPerformQuery() {
      return vm.stageQueries.length;
   }

   function addToQuery(selectedStage) {
      vm.stageQueries = [...vm.stageQueries, selectedStage];
   }

   function removeFromQuery(selectedStage) {
      vm.stageQueries = filter(vm.stageQueries, extStage => extStage.stage.id !== selectedStage.stage.id);
   }

   vm.stageClick = (selectedStageAndVsi, entityStageObject, vacancyStagesEntitiesVSIs, index) => {
      if (vm.isHiredOrRejected(entityStageObject)) {
         return;
      }
      let currentExtendedStage = getCurrentExtendedStage(vacancyStagesEntitiesVSIs);
      if (isSelectedStageInactive(selectedStageAndVsi)) {
         let currentStageAndVsi = getCurrentStageAndVsi(vacancyStagesEntitiesVSIs, currentExtendedStage);
         if (isCurrentCommentValid(currentStageAndVsi)) {
            setCurrentToPassed(currentStageAndVsi);
            setSelectedStageToActive(selectedStageAndVsi, entityStageObject);
         } else {
            addValidationError(currentStageAndVsi);
         }
      } else if (isSelectedStageIsActiveAndNotFirst(selectedStageAndVsi, currentExtendedStage)) {
         setSelectedToInactive(selectedStageAndVsi);
         setLatestPassedStageToActive(vacancyStagesEntitiesVSIs);
      }

      if (selectedStageAndVsi.showCommentArea) {
         findAndFocusOnTextArea(index);
      }
   };

   function findAndFocusOnTextArea (index) {
      $timeout(() => {
         let textarea = findVSICommentArea(index);
         textarea.focus();
      });
   }

   function findVSICommentArea (index) {
      return document.getElementById(`comment-area-${index}`);
   }

   function addValidationError(currentStageAndVsi) {
      currentStageAndVsi.areaIsValid = false;
   }

   function isSelectedStageIsActiveAndNotFirst(selectedStageAndVsi, currentExtStage) {
      return selectedStageAndVsi.stageState === STAGE_STATES.Active && currentExtStage.order !== 1;
   }

   function getCurrentExtendedStage(vacancyStagesEntitiesVSIs) {
      return find(vacancyStagesEntitiesVSIs, ['stageState', STAGE_STATES.Active]).stage;
   }

   function isSelectedStageInactive(selectedStageAndVsi) {
      return selectedStageAndVsi.stageState === STAGE_STATES.Inactive;
   }

   function getCurrentStageAndVsi(vacancyStagesEntitiesVSIs, currentStage) {
      return find(vacancyStagesEntitiesVSIs, ['stage.stage.id', currentStage.stage.id]);
   }

   function isCurrentCommentValid(currentStageAndVsi) {
      if (currentStageAndVsi.stage.stage.isCommentRequired) {
         return !!currentStageAndVsi.vsi.comment.message;
      }
      return true;
   }

   function setCurrentToPassed(currentStageAndVsi) {
      currentStageAndVsi.areaIsValid = true;
      currentStageAndVsi.stageState = STAGE_STATES.Passed;
      currentStageAndVsi.class = 'passed';
      currentStageAndVsi.vsi.stageState = STAGE_STATES.Passed;
      currentStageAndVsi.vsi.dateOfPass = moment(currentStageAndVsi.vsi.dateOfPass, DATE_FORMAT).format(DATE_FORMAT);
   }

   function setSelectedStageToActive(selectedStageAndVsi, candObject) {
      selectedStageAndVsi.stageState = STAGE_STATES.Active;
      selectedStageAndVsi.class = 'active';
      selectedStageAndVsi.showCommentArea = selectedStageAndVsi.stage.stage.commentField && true;
      selectedStageAndVsi.vsi = assign(selectedStageAndVsi.vsi || {}, {
         vacancyId: candObject.vacancyId,
         candidateId: candObject.candidateId,
         dateOfPass: moment().format(DATE_FORMAT),
         stage: selectedStageAndVsi.stage,
         stageId: selectedStageAndVsi.stage.stage.id,
         comment: selectedStageAndVsi.showCommentArea ? {
            message: '',
            authorId: vm.currentUser.id
         } : null,
         stageState: STAGE_STATES.Active
      });
   }

   function setSelectedToInactive(selectedStageAndVsi) {
      selectedStageAndVsi.stageState = STAGE_STATES.Inactive;
      selectedStageAndVsi.class = 'inactive';
      selectedStageAndVsi.showCommentArea = false;
      selectedStageAndVsi.vsi = assign(selectedStageAndVsi.vsi || {}, { stageState: STAGE_STATES.Inactive });
   }

   function setLatestPassedStageToActive(vacancyStagesEntitiesVSIs) {
      let latestPassedStageAndVsi;
      let passedStages = filter(vacancyStagesEntitiesVSIs, (stageAndVsi) => {
         return stageAndVsi.stageState === STAGE_STATES.Passed;
      });
      if (passedStages.length) {
         latestPassedStageAndVsi = maxBy(passedStages, 'stage.order');
      }
      if (latestPassedStageAndVsi) {
         latestPassedStageAndVsi.stageState = STAGE_STATES.Active;
         latestPassedStageAndVsi.class = 'active';
         latestPassedStageAndVsi.vsi.stageState = STAGE_STATES.Active;
         latestPassedStageAndVsi.vsi.dateOfPass = moment(latestPassedStageAndVsi.vsi.dateOfPass, DATE_FORMAT)
            .format(DATE_FORMAT);
      }
   }

   //TODO: make one filter (sum) loop
   vm.isHiredOrRejected = (entityStageObject) => {
      let stageFlow = entityStageObject.stageFlow || vm.vacancyStages;
      let hireStage = find(stageFlow, ['stage.stageType', STAGE_TYPES.HireStage]);
      let rejectedOrHired = filter(entityStageObject.vacancyStageInfos, stageAndVsi =>
         stageAndVsi.stageState === STAGE_STATES.NotPassed || stageAndVsi.stageId === hireStage.stage.id);
      return rejectedOrHired.length;
   };

   vm.getStageTitle = (currentStageId, entityStageObject) => {
      let stageFlow = vm.vacancyStages;
      if (!stageFlow) {
         stageFlow = entityStageObject.stageFlow;
      }
      if (entityStageObject && entityStageObject.stageFlow) {
         stageFlow = entityStageObject.stageFlow;
      }
      let stage = find(stageFlow, ['stage.id', currentStageId]);
      return stage.stage.title;
   };

   function notifySuccess() {
      UserDialogService.notification('Stage succesfully passed', 'notification');
      return $q.when();
   }

   function currentStageOf(entityStageObject) {
      let currentStageId = find(entityStageObject.vacancyStageInfos, ['stageState', STAGE_STATES.Active]).stageId;
      return currentStageId;
   }
}
