import template from './candidate-vacancy-info.directive.html';
import './candidate-vacancy-info.scss';
import oneStageCommentDialogTemplate from './one-stage-comment-adding-dialog.template.html';
import manyStageCommentDialogTemplate from './many-stage-comment-adding-dialog.template.html';

import {
   map,
   forEach,
   find,
   filter
} from 'lodash';

export default class CandidateVacancyInfoDirective {
   constructor() {
      this.restrict = 'E';
      this.template = template;
      this.scope = {
         parentEntity: '@',
         directiveType: '@',
         vacancyStages: '=',
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
   'ngInject';
   const vm = $scope;

   function handleStagePassing(stagesObject, stageToPass) {
      let deffered = $q.defer();
      if (stageToPass.isCommentRequired) {
         showSingleCommentDialog().then((dialogTransferObject) => {
            dialogTransferObject.isPassed = true;
            deffered.resolve({ comment: dialogTransferObject.comment,
               isPassed: dialogTransferObject.isPassed,
               stagesObject,
               stageToPass });
         }).catch(() => {
            deffered.reject();
         });
      } else {
         deffered.resolve({ comment: null,
            isPassed: true,
               stagesObject,
               stageToPass  });
      }
      return deffered.promise;
   }

   function showSingleCommentDialog() {
      let dialogResult = $q.defer();
      let dialogTransferObject = { comment: ' ' };
      let scope = {
         dialogResult,
         dialogTransferObject
      };
      let buttons = [
         {
            name: $translate.instant('COMMON.CANCEL'),
            func: () => {
               dialogResult.reject();
            }
         },
         {
            name: $translate.instant('COMMON.APLY'),
            func: () => {
               dialogResult.resolve(dialogTransferObject);
            }
         }
      ];
      UserDialogService.dialog($translate.instant('COMMON.COMMENTS'), oneStageCommentDialogTemplate, buttons, scope);
      return dialogResult.promise;
   }

   function _createOrUpdateVSIWith(stagesObject, stageToPass, comment, isPassed) {
      let deffer = $q.defer();
      let VSI = find(stagesObject.stages, { stageId: stageToPass.id });
      if (VSI) {
         if (!VSI.isPassed) {
            VSI.comment = { message: comment };
            VSI.isPassed = isPassed;
         }
      } else {
         _addVSI(stagesObject, stageToPass, comment, isPassed);
      }
      deffer.resolve(stagesObject, stageToPass);
      return deffer.promise;
   }

   vm.getCommentByStageId = (stages, stageId, candidateId) => {
      stageId = parseInt(stageId);
      let vacancyStageInfo = find(stages, { candidateId, stageId });
      if (vacancyStageInfo) {
         if (vacancyStageInfo.comment) {
            return vacancyStageInfo.comment.message;
         } else {
            return '';
         }
      }
      return 'no vsi for this stage';
   };

   vm.passStageToNext = (candidateStagesObject) => {
      let candidateCurrentStageBuff = candidateStagesObject.currentStageId;
      candidateStagesObject.currentStageId = candidateStagesObject.selectedStageId;
      candidateStagesObject.selectedStageId = candidateStagesObject.selectedStageId + 1;
      changeStage(candidateStagesObject).then(() => {
         _handleStageChanged(candidateStagesObject, candidateStagesObject.selectedStageId);
         candidateStagesObject.currentStageId = candidateCurrentStageBuff;
      }).catch((currentStage) => {
         notifySelectedStageChangingBack();
         candidateStagesObject.selectedStageId = currentStage.id;
      });
   };

   vm.onStageChange = (stagesObject) => {
      changeStage(stagesObject).then(() => {
         _handleStageChanged(stagesObject, stagesObject.selectedStageId);
      }).catch((currentStage) => {
         notifySelectedStageChangingBack();
         stagesObject.selectedStageId = currentStage.id;
      });
   };

   function changeStage(stagesObject) {
      let deffered = $q.defer();
      getCurrentAndSelectedStages(stagesObject).then((stages) => {
         return goToStage(stagesObject, stages.currentStage, stages.selectedStage);
      }).then((selectedStage) => {
         if (selectedStage) {
            deffered.resolve(selectedStage);
            notifyCurrentStageChangingForward();
         }
         deffered.resolve();
      }).catch((currentStage) => {
         deffered.reject(currentStage);
      });
      return deffered.promise;
   }

   function getCurrentAndSelectedStages(stagesObject) {
      let deffer = $q.defer();
      let vacancyStages = vm.vacancyStages ? vm.vacancyStages : stagesObject.stageFlow;
      let currentStage = find(vacancyStages, { id: stagesObject.currentStageId });
      let selectedStage = find(vacancyStages, { id: stagesObject.selectedStageId });
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
      return deffered.promise;
   }

   function _isSelectedIsNextToCurrent(selectedStage, currentStage) {
      return currentStage.order + 1 === selectedStage.order;
   }

   function setCurrentStageBySelected(candidateStagesObject, selectedStage) {
      let deffered = $q.defer();
      candidateStagesObject.currentStageId = selectedStage.id;
      deffered.resolve();
      return deffered.promise;
   }

   function showManyCommentDialog(stagesBetweenCurrentAndSelected) {
      let dialogResult = $q.defer();
      let dialogTransferObject = { stagesToPass: stagesBetweenCurrentAndSelected };
      let scope = {
         dialogResult,
         dialogTransferObject
      };
      let buttons = [
         {
            name: $translate.instant('COMMON.CANCEL'),
            func: () => {
               dialogResult.reject();
            }
         },
         {
            name: $translate.instant('COMMON.APLY'),
            func: () => {
               dialogResult.resolve(dialogTransferObject);
            }
         }
      ];
      UserDialogService.dialog($translate.instant('COMMON.COMMENTS'), manyStageCommentDialogTemplate, buttons, scope);
      return dialogResult.promise;
   }

   function _handleStageChanged(candidateStagesObject, selectedStageId) {
      let VSI = find(candidateStagesObject.stages, { stageId: selectedStageId });
      if (VSI) {
         candidateStagesObject.selectedStageIsPassed = VSI.isPassed;
      } else {
         candidateStagesObject.selectedStageIsPassed = false;
      }
   }


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
      });
      deffered.resolve(recomposed);
      return deffered.promise;
   }


   function _addVSI(candidateStagesObject, stage, comment, isPassed) {
      let newVSI = {
         vacancyId: candidateStagesObject.stages[0].vacancyId,
         candidateId: candidateStagesObject.stages[0].candidateId,
         comment: comment ? { message: comment } : null,
         isPassed: isPassed ? isPassed : false,
         stageId: stage.id,
         createdOn: (new Date()).toISOString()
      };
      candidateStagesObject.stages.push(newVSI);
   }

   vm.getDateByStageId = (stages, stageId, candidateId) => {
      stageId = parseInt(stageId);
      let vacancyStageInfo = find(stages, { candidateId, stageId });
      if (vacancyStageInfo) {
         if (vacancyStageInfo.createdOn) {
            return vacancyStageInfo.createdOn;
         } else {
            return '';
         }
      }
      return '';
   };

   vm.goCandidate = (candidateId) => {
      $state.go('candidateProfile', {_data: null, candidateId});
   };
}
