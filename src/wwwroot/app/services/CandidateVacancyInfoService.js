import {
   map,
   forEach,
   find,
   filter
} from 'lodash';

let _$q, _VacancyService, _CandidateService, _parentEntity, _translate;

export default class CandidateVacancyInfoService {
   constructor($state, $translate, $q, VacancyService, CandidateService) {
      'ngInject';
      _$q = $q;
      _VacancyService = VacancyService;
      _CandidateService = CandidateService;
      _translate = $translate;
   }

   compose(vacancyStageInfos, parentEntity) {
      _parentEntity = parentEntity;
      return _recompose(vacancyStageInfos).then((recomposed) => {
         return _fillWithCandidates(recomposed);
      }).then((recomposedAndFilledWithCandidates) => {
         return _fillWithVacancies(recomposedAndFilledWithCandidates);
      }).then((vacancyStagesObject) => {
         return vacancyStagesObject;
      }).catch((err) => {
         console.log(err, 'ошибка');
      });
   }


   getCommentByStageId (stages, stageId, candidateId) {
      stageId = parseInt(stageId);
      let vacancyStageInfo = find(stages, { candidateId, stageId });
      if (vacancyStageInfo) {
         if (vacancyStageInfo.comment) {
            return vacancyStageInfo.comment.message;
         } else {
            return 'button';
         }
      }
      return 'no vsi for this stage';
   }

   passStageToNext (candidateStagesObject) {
      candidateStagesObject.selectedStageId = candidateStagesObject.selectedStageId + 1;
      changeStage(candidateStagesObject).then(() => {
         _handleStageChanged(candidateStagesObject, candidateStagesObject.selectedStageId);
      }).catch((currentStage) => {
         notifySelectedStageChangingBack();
         candidateStagesObject.selectedStageId = currentStage.id;
      });
   }

   onStageChange (stagesObject) {
      changeStage(stagesObject).then(() => {
         _handleStageChanged(stagesObject, stagesObject.selectedStageId);
      }).catch((currentStage) => {
         notifySelectedStageChangingBack();
         stagesObject.selectedStageId = currentStage.id;
      });
   }

   changeStage(stagesObject) {
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

   getCurrentAndSelectedStages(stagesObject) {
      let deffer = $q.defer();
      let vacancyStages = vm.vacancyStages ? vm.vacancyStages : stagesObject.stageFlow;
      let currentStage = find(vacancyStages, { id: stagesObject.currentStageId });
      let selectedStage = find(vacancyStages, { id: stagesObject.selectedStageId });
      deffer.resolve({currentStage, selectedStage});
      return deffer.promise;
   };

   goToStage (stagesObject, currentStage, selectedStage) {
      let deffered = $q.defer();
      if (_isSelectedIsNextToCurrent(selectedStage, currentStage)) {
         handleCurrentToNextAction(stagesObject, currentStage).then((actionResultObject) => {
            return _createOrUpdateVSIWith(stagesObject, currentStage, actionResultObject.comment, actionResultObject.isPassed);  // eslint-disable-line max-len
         }).then(() => {
            return _createOrUpdateVSIWith(stagesObject, selectedStage);
         }).then(() => {
            return setCurrentStageBySelected(stagesObject, selectedStage);
         }).then(() => {
            deffered.resolve(selectedStage);
         }).catch(() => {
            deffered.reject(currentStage);
         });
      } else if (selectedStage.id > currentStage.id) {
         debugger;
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

   _isSelectedIsNextToCurrent(selectedStage, currentStage) {
      return currentStage.order + 1 === selectedStage.order;
   }

   handleCurrentToNextAction(stagesObject, currentStage) {
      let deffered = $q.defer();
      if (currentStage.isCommentRequired) {
         showSingleCommentDialog().then((dialogTransferObject) => {
            dialogTransferObject.isPassed = true;
            deffered.resolve({ comment: dialogTransferObject.comment, isPassed: dialogTransferObject.isPassed });
         }).catch(() => {
            deffered.reject();
         });
      } else {
         deffered.resolve({ comment: null, isPassed: true });
      }
      return deffered.promise;
   }

   showSingleCommentDialog() {
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

   _createOrUpdateVSIWith(stagesObject, stage, comment, isPassed) {
      let deffer = $q.defer();
      let VSI = find(stagesObject.stages, { stageId: stage.id });
      let VSIinVSIs = find(vm.vacancyStageInfos, { stageId: stage.id});
      if (VSI) {
         VSI.comment = { message: comment };
         VSI.isPassed = true;
         VSIinVSIs.comment = { message: comment };
         VSIinVSIs.isPassed = true;
         console.log(VSI, 'vsi added');
      } else {
         _addVSI(stagesObject, stage, comment, isPassed);
      }
      deffer.resolve();
      return deffer.promise;
   }

   setCurrentStageBySelected(candidateStagesObject, selectedStage) {
      let deffered = $q.defer();
      candidateStagesObject.currentStageId = selectedStage.id;
      deffered.resolve();
      return deffered.promise;
   }

   showManyCommentDialog(stagesBetweenCurrentAndSelected) {
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

   _handleStageChanged(candidateStagesObject, selectedStageId) {
      let VSI = find(candidateStagesObject.stages, { stageId: selectedStageId });
      if (VSI) {
         candidateStagesObject.selectedStageIsPassed = VSI.isPassed;
      } else {
         candidateStagesObject.selectedStageIsPassed = false;
      }
      console.log(candidateStagesObject);
   }


   notifySelectedStageChangingBack() {
      UserDialogService.notification('Stage passing is cancelled', 'notification');
   }

   notifyCurrentStageChangingForward() {
      UserDialogService.notification('Stage was succesfully changed', 'notification');
   }

   _findStagesAndRecomposeBy(stagesObject) {
      return findStagesBetween(stagesObject).then((stages) => {
         return recomposeStagesBetween(stages);
      });
   }

   findStagesBetween(stagesObject) {
      let deffered = $q.defer();
      let vacancyStages = vm.vacancyStages ? vm.vacancyStages : stagesObject.stageFlow;
      let stagesBetweenCurrentAndSelected = filter(vacancyStages, (stage) => {
         if (stagesObject.currentStageId <= stage.id && stage.id <= stagesObject.selectedStageId) {
            return stage;
         }
      });
      console.log(stagesBetweenCurrentAndSelected, 'stages between current');
      deffered.resolve(stagesBetweenCurrentAndSelected);
      return deffered.promise;
   }

   recomposeStagesBetween(stages) {
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


   _addVSI(candidateStagesObject, stage, comment, isPassed) {
      let newVSI = {
         vacancyId: candidateStagesObject.stages[0].vacancyId,
         candidateId: candidateStagesObject.stages[0].candidateId,
         comment: { message: comment } ,
         isPassed: isPassed ? isPassed : false,
         stageId: stage.id,
         createdOn: (new Date()).toISOString()
      };
      candidateStagesObject.stages.push(newVSI);
      vm.vacancyStageInfos.push(newVSI);
   }

   getDateByStageId (stages, stageId, candidateId) {
      stageId = parseInt(stageId);
      let vacancyStageInfo = find(stages, { candidateId, stageId });
      if (vacancyStageInfo) {
         if (vacancyStageInfo.createdOn) {
            return vacancyStageInfo.createdOn;
         } else {
            return 'no date';
         }
      }
      return 'no vsi for this stage';
   }


   _getTitle() {
      if (_parentEntity === 'candidate') {
         return _translate.instant('COMMON.VACANCIES');
      }
      if (_parentEntity === 'vacancy') {
         return _translate.instant('COMMON.CANDIDATES');
      }
   }

   goCandidate (candidateId) {
      $state.go('candidateProfile', {_data: null, candidateId});
   };
}

function _recompose(vacancyStageInfos) {
   let deffered = _$q.defer();
   let composedBy = [];
   if (_parentEntity === 'vacancy') {
      forEach(vacancyStageInfos, (vsi) => {
         let composedEntity = find(composedBy, { candidateId: vsi.candidateId });
         if (composedEntity) {
            composedEntity.stages.push(vsi);
         } else {
            composedBy.push({
               candidateId: vsi.candidateId,
               vacancyId: vsi.vacancyId,
               stages: [ vsi ]
            }
            );
         }
      });
   } else {
      forEach(vacancyStageInfos, (vsi) => {
         let composedEntity = find(composedBy, { vacancyId: vsi.vacancyId });
         if (composedEntity) {
            composedEntity.stages.push(vsi);
         } else {
            composedBy.push({
               candidateId: vsi.candidateId,
               vacancyId: vsi.vacancyId,
               stages: [ vsi ]
            }
            );
         }
      });
   }
   let composedWithStages = map(composedBy, (obj) => {
      let currentStage = find(obj.stages, { isPassed: false });
      if (currentStage) {
         obj.currentStageId = currentStage.stageId;
         obj.selectedStageId = currentStage.stageId;
         obj.selectedStageIsPassed = currentStage.isPassed;
      } else {
         obj.currentStageId = 1;
         obj.selectedStageId = 1;
         obj.selectedStageIsPassed = false;
      }
      return obj;
   });
   deffered.resolve(composedWithStages);
   return deffered.promise;
}

function _fillWithCandidates(recomposed) {
   let deffered = _$q.defer();
   deffered.resolve(map(recomposed, _loadCandidates));
   return deffered.promise;
}

function _fillWithVacancies(recomposed) {
   let deffered = _$q.defer();
   if (_parentEntity === 'candidate') {
      deffered.resolve(map(recomposed, _loadVacancies));
   } else {
      deffered.resolve(recomposed);
   }
   return deffered.promise;
}

function _loadCandidates(candidateStagesObject) {
   let stagesObjectWithCandidate = candidateStagesObject;
   _CandidateService.getCandidate(candidateStagesObject.candidateId).then(value => {
      stagesObjectWithCandidate.candidate = value;
   });
   return stagesObjectWithCandidate;
}

function  _loadVacancies(vacanciesStagesObject) {
   let stagesObjectWithCandidate = vacanciesStagesObject;
   _VacancyService.getVacancy(vacanciesStagesObject.vacancyId).then(value => {
      stagesObjectWithCandidate.vacancy = value;
      stagesObjectWithCandidate.stageFlow = value.stageFlow;
   });
   return stagesObjectWithCandidate;
}
