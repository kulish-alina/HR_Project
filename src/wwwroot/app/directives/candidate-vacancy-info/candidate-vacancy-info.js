import template from './candidate-vacancy-info.directive.html';
import './candidate-vacancy-info.scss';
import manyStageCommentDialogTemplate from './many-stage-comment-adding-dialog.template.html';

import {
   map,
   filter,
   find,
   curry,
   cloneDeep,
   reduce,
   maxBy
} from 'lodash';

export default class CandidateVacancyInfoDirective {
   constructor() {
      this.restrict = 'E';
      this.template = template;
      this.scope = {
         parentEntity: '@',
         directiveType: '@',
         vacancyStages: '=',
         promise: '=',
         composedby: '='
      };
      this.controller = CandidateVacancyInfoController;
   }
   static createInstance() {
      'ngInject';
      CandidateVacancyInfoDirective.instance = new CandidateVacancyInfoDirective();
      return CandidateVacancyInfoDirective.instance;
   }
}

function CandidateVacancyInfoController($scope, $translate, $q, $state, UserDialogService) {
   UserDialogService,
   $q
   'ngInject';
   const vm = $scope;
   vm.candObjectsToShow = vm.composedby;
   console.log(vm.composedby);
   vm.stageQueries = [];
   const STAGE_STATES = {
      'Inactive': 0,
      'Active': 1,
      'Passed' : 2,
      'NotPassed' : 3
   };
   let curriedUpdateCandidateStage = curry(updateCandidateStages, 2);
               stageToPass  });
   function _init() {
      if (vm.parentEntity === 'vacancy') {
         vm.vacancyStages = calculateVacancyStagesCandidatesCount(vm.vacancyStages, vm.composedby);
      }
   _init();
   function calculateVacancyStagesCandidatesCount (vacancyStages, composedBy) {
      return map(vacancyStages, (extStage) => {
         let stageCandCount = reduce(composedBy, (count, vsi) => {
            return vsi.currentStageId === extStage.stage.id ? count + 1 : count + 0;
         }, 0);
         return Object.assign(extStage, {
            candidateCount: stageCandCount,
            _hasCandidates: stageCandCount !== 0,
            _isPressed: false
         });
         dialogTransferObject
      };
      let buttons = [
         {
            name: $translate.instant('COMMON.CANCEL'),
            func: () => {
               dialogResult.reject();
               dialogResult.resolve(dialogTransferObject);
         }
      ];
   vm.callStagesDialog = (entityStageObject) => {
      let stageFlow = entityStageObject.stageFlow ? entityStageObject.stageFlow : vm.vacancyStages;
      showManyStagesDialogFor(stageFlow, entityStageObject)
         .then(curriedUpdateCandidateStage(entityStageObject))
         .then(notifySuccess)
         .catch((result) => {
            curriedUpdateCandidateStage(entityStageObject)(result)
               .then(notifyFailure);
         });
   };

   function updateCandidateStages(entityStageObject, result) {
      entityStageObject.stages = map(filter(result.vacancyStagesCandidatesVSIs, (stageVsi) => {
         if (stageVsi.vsi) {
            return true;
            VSI.isPassed = isPassed;
         }
      }), (stageVsi) => {
         return stageVsi.vsi;
      });
      recalculateCurrentStage(entityStageObject);
      vm.vacancyStages = calculateVacancyStagesCandidatesCount(vm.vacancyStages, vm.composedby);
      }
      deffer.resolve(stagesObject, stageToPass);
   function recalculateCurrentStage(entityStageObject) {
      entityStageObject.currentStageId = currentStageOf(entityStageObject);
      return $q.when();
   }

   vm.candidateCount = (extStage) => {
      return extStage.candidates.length ? extStage.candidates.length : '';

   vm.queryByStage = (extStage) => {
      if (extStage) {
         vm.candObjectsToShow = extStage.candidates;
      } else {
         vm.candObjectsToShow = vm.composedby;
         if (vacancyStageInfo.comment) {
         }
      }
   };

   vm.callRejectButton = (candidateStage) => {
      candidateStage.rejectButtonClicked = !!!candidateStage.rejectButtonClicked;
         _handleStageChanged(candidateStagesObject, candidateStagesObject.selectedStageId);
      }).catch((currentStage) => {
         notifySelectedStageChangingBack();
         candidateStagesObject.selectedStageId = currentStage.id;
   };

   vm.onStageChange = (stagesObject) => {
      changeStage(stagesObject).then(() => {
         _handleStageChanged(stagesObject, stagesObject.selectedStageId);
      }).catch((currentStage) => {
         notifySelectedStageChangingBack();
         stagesObject.selectedStageId = currentStage.id;

   function showManyStagesDialogFor(vacancyStages, entityStageObject) {
      let dialogResult = $q.defer();
      let dialogTransferObject = {};
      let vacancyStagesCandidatesVSIs = cloneDeep(map(vacancyStages, (vacancyStage) => {
         let showCommentArea = false;
         let stageVsi =  find(entityStageObject.stages, { stageId: vacancyStage.stage.id });
         let stageState = STAGE_STATES.Inactive;
         let vsiClass = '';
         if (stageVsi) {
            stageVsi.stage = vacancyStage.stage;
            switch (stageVsi.stageState) {
               case STAGE_STATES.Active: {
                  stageState = STAGE_STATES.Active;
                  vsiClass = 'active';
                  break;
               }
               case STAGE_STATES.Passed: {
                  stageState = STAGE_STATES.Passed;
                  vsiClass = 'passed';
                  break;
               }
               case STAGE_STATES.NotPassed: {
                  stageState = STAGE_STATES.NotPassed;
                  vsiClass = 'not-passed';
                  break;
      deffer.resolve({currentStage, selectedStage});
      return deffer.promise;
   };

   function goToStage (stagesObject, currentStage, selectedStage) {
      let deffered = $q.defer();
      if (_isSelectedIsNextToCurrent(selectedStage, currentStage)) {
         handleStagePassing(stagesObject, currentStage).then((actionResultObject) => {
            return _createOrUpdateVSIWith(stagesObject, currentStage, actionResultObject.comment, actionResultObject.isPassed);  // eslint-disable-line max-len
         }).then(() => {
            return _createOrUpdateVSIWith(stagesObject, selectedStage, null, false);
         }).then(() => {
            return setCurrentStageBySelected(stagesObject, selectedStage);
         }).then(() => {
            deffered.resolve(selectedStage);
         }).catch(() => {
            deffered.reject(currentStage);
         });
      } else if (selectedStage.id > currentStage.id) {
         _findStagesAndRecomposeBy(stagesObject).then((recomposed) => {
            return showManyCommentDialog(recomposed);
         }).then((dialogTransferObject) => {
            forEach(dialogTransferObject.stagesToPass, (stageObj) => {
               _createOrUpdateVSIWith(stagesObject, stageObj.stage, stageObj.comment, stageObj.isPassed);
            });
            return setCurrentStageBySelected(stagesObject, selectedStage);
         }).then(() => {
            deffered.resolve(selectedStage);
         })
         .catch(() => {
            deffered.reject(currentStage);
         });
      } else {
         deffered.resolve();
               }
               default: {
                  stageState = STAGE_STATES.Inactive;
                  vsiClass = 'inactive';
                  break;
               }
            }
            if (stageVsi.stage.isCommentRequired) {
               showCommentArea = true;
      candidateStagesObject.currentStageId = selectedStage.id;
      deffered.resolve();
      return deffered.promise;
            }
         }
         return {
            stage: vacancyStage,
            vsi: find(entityStageObject.stages, { stageId: vacancyStage.stage.id }),
            showCommentArea,
            stageState,
            class: vsiClass
         };
      }));
      dialogTransferObject.vacancyStagesCandidatesVSIs = cloneDeep(vacancyStagesCandidatesVSIs);
      let scope = {
         dialogResult,
         dialogTransferObject,
         stageClick: vm.stageClick,
         entityStageObject
      };
      let buttons = [
         {
            name: $translate.instant('COMMON.CANCEL'),
            func: () => {
               dialogResult.reject({ vacancyStagesCandidatesVSIs });
            }
         },
         {
            name: $translate.instant('COMMON.APLY'),
            func: () => {
               dialogResult.resolve(dialogTransferObject);
            }
         }
      ];
      UserDialogService.dialog($translate.instant('Candidate stages'),
         manyStageCommentDialogTemplate, buttons, scope);
      return dialogResult.promise;
   }

   vm.goCandidate = (candidateId) => {
      $state.go('candidateProfile', {_data: null, candidateId});

   function notifySelectedStageChangingBack() {
      UserDialogService.notification('Stage passing is cancelled', 'warning');
   }

   function notifyCurrentStageChangingForward() {
      UserDialogService.notification('Stage was succesfully changed', 'notification');
   }

   function _findStagesAndRecomposeBy(stagesObject) {
      return findStagesBetween(stagesObject).then((stages) => {
         return recomposeStagesBetween(stages);
      });
   }

   function findStagesBetween(stagesObject) {
      let deffered = $q.defer();
      let vacancyStages = vm.vacancyStages ? vm.vacancyStages : stagesObject.stageFlow;
      let stagesBetweenCurrentAndSelected = filter(vacancyStages, (stage) => {
         if (stagesObject.currentStageId <= stage.id && stage.id <= stagesObject.selectedStageId) {
            let stageBetween = find(stagesObject.stages, { stageId: stage.id, isPassed: true });
            if (!stageBetween) {
               return stage;
            }
         }
      });
      deffered.resolve(stagesBetweenCurrentAndSelected);
      return deffered.promise;
   }

   function recomposeStagesBetween(stages) {
      let deffered = $q.defer();
      let recomposed = map(stages, (stage) => {
         return {
            stage,
            comment: null,
            isPassed: false
   };

   vm.filterByStage = (selectedStage) => {
      selectedStage._isPressed = !selectedStage._isPressed;
      if (selectedStage._isPressed) {
         vm.stageQueries = [...vm.stageQueries, selectedStage];
      } else {
         vm.stageQueries = filter(vm.stageQueries, (extStage) => {
            return extStage.stage.id !== selectedStage.stage.id;
         });
      }
      if (vm.stageQueries.length) {
         vm.candObjectsToShow = filter(vm.composedby, (entityStageObject) => {
            return filter(vm.stageQueries, (extStage) => {
               return extStage.stage.id === entityStageObject.currentStageId;
            }).length;
         });
      } else {
         vm.candObjectsToShow = vm.composedby;
         stageId: stage.id,
         createdOn: (new Date()).toISOString()
      };
      candidateStagesObject.stages.push(newVSI);
      }
   };

   function getCurrentStageAndVsi(vacancyStagesCandidatesVSIs, currentStageId) {
      return $q.when(filter(vacancyStagesCandidatesVSIs, (stageAndVsi) => {
         return stageAndVsi.stage.stage.id === currentStageId;
      })[0]);
            return vacancyStageInfo.createdOn;
         } else {
            return '';
   }

   function validateCurrentComment(currentStageAndVsi) {
      let deffered = $q.defer();
      if (currentStageAndVsi.stage.stage.isCommentRequired && !currentStageAndVsi.vsi.comment.message) {
         deffered.reject(currentStageAndVsi);
      }
      deffered.resolve(currentStageAndVsi);
      }
      return '';
   };

   function setCurrentToPassed(currentStageAndVsi) {
      currentStageAndVsi.areaIsValid = true;
      currentStageAndVsi.stageState = STAGE_STATES.Passed;
      currentStageAndVsi.class = 'passed';
      currentStageAndVsi.vsi.stageState = STAGE_STATES.Passed;
      return $q.when();
   }

   function setSelectedToActive (selectedStageAndVsi, candObject) {
      selectedStageAndVsi.stageState = STAGE_STATES.Active;
      selectedStageAndVsi.class = 'active';
      selectedStageAndVsi.showCommentArea = selectedStageAndVsi.stage.stage.isCommentRequired && true;
      selectedStageAndVsi.vsi = {
         vacancyId: candObject.vacancyId,
         candidateId: candObject.candidateId,
         stageId: selectedStageAndVsi.stage.stage.id,
         comment: selectedStageAndVsi.showCommentArea ? { message: ''} : null,
         stageState: STAGE_STATES.Active,
         createdOn: (new Date()).toISOString()
      };
      return $q.when();
   }

   function setSelectedToInactive(selectedStageAndVsi) {
      selectedStageAndVsi.stageState = STAGE_STATES.Inactive;
      selectedStageAndVsi.class = 'inactive';
      selectedStageAndVsi.showCommentArea = false;
      selectedStageAndVsi.vsi = null;
      return $q.when();
   }

   function setLatestPassedStageToActive(vacancyStagesCandidatesVSIs) {
      let latestPassedStageAndVsi;
      let passedStages = filter(vacancyStagesCandidatesVSIs, (stageAndVsi) => {
         return stageAndVsi.stageState === STAGE_STATES.Passed;
      });
      if (passedStages.length) {
         latestPassedStageAndVsi = maxBy(passedStages, 'stage.order');
      }
      if (latestPassedStageAndVsi) {
         latestPassedStageAndVsi.stageState = STAGE_STATES.Active;
         latestPassedStageAndVsi.class = 'active';
         latestPassedStageAndVsi.vsi.stageState = STAGE_STATES.Active;
      }
      return $q.when();
   }

   vm.stageClick = (selectedStageAndVsi, candObject, vacancyStagesCandidatesVSIs) => {
      let currentExtStage = filter(vacancyStagesCandidatesVSIs, (stageAndVsi) => {
         return stageAndVsi.stageState === STAGE_STATES.Active;
      })[0].stage;
      let currentStageId = currentExtStage.stage.id;
      if (selectedStageAndVsi.stageState === STAGE_STATES.Inactive) {
         getCurrentStageAndVsi(vacancyStagesCandidatesVSIs, currentStageId)
            .then(validateCurrentComment)
            .then(setCurrentToPassed)
            .then(setSelectedToActive(selectedStageAndVsi, candObject))
            .catch((currentStageAndVsi) => {
               currentStageAndVsi.areaIsValid = false;
            });
      } else if (selectedStageAndVsi.stageState === STAGE_STATES.Active && currentExtStage.order !== 1) {
         setSelectedToInactive(selectedStageAndVsi)
            .then(setLatestPassedStageToActive(vacancyStagesCandidatesVSIs));
      }
   };

   vm.getStageTitle = (latestStageId, entityStageObject) => {
      let stageFlow = vm.vacancyStages;
      if (entityStageObject && entityStageObject.stageFlow) {
         stageFlow = entityStageObject.stageFlow;
      }
      let stage = filter(stageFlow, (extStage) => {
         return extStage.stage.id === latestStageId;
      })[0];
      return stage.stage.title;
   };

   function notifySuccess() {
      UserDialogService.notification('Stage succesfully passed', 'notification');
      return $q.when();
   }

   function notifyFailure() {
      UserDialogService.notification('Stage passing is cancelled', 'warning');
   }

   function currentStageOf(entityStageObject) {
      let currentStageId = filter(entityStageObject.stages, (vsi) => {
         return vsi.stageState === STAGE_STATES.Active;
      })[0].stageId;
      return currentStageId;
   }

   vm.getDate = (composedStageObject) => {
      let vacancyStageInfo = find(composedStageObject.stages, {stageId: composedStageObject.latestStageId});
      let date = '';
      if (vacancyStageInfo) {
         date = vacancyStageInfo.createdOn;
      }
      return date;
   };

   vm.getComment = (composedStageObject) => {
      let vacancyStageInfo = find(composedStageObject.stages, {stageId: composedStageObject.latestStageId});
      let comment = '';
      if (vacancyStageInfo && vacancyStageInfo.comment) {
         comment = vacancyStageInfo.comment.message;
      }
      return comment;
   };
}
