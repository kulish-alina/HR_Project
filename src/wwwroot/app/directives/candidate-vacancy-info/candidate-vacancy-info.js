import template from './candidate-vacancy-info.directive.html';
import './candidate-vacancy-info.scss';
import manyStageCommentDialogTemplate from './many-stage-comment-adding-dialog.template.html';
import hireDateDialogTemplate from './hire-date-dialog.template.html';

import {
      map,
      filter,
      find,
      cloneDeep,
      reduce,
      maxBy,
      some,
      head,
      isNil
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
         vacancyStageInfosComposedByCandidateIdVacancyId: '=',
         closevacancy: '='
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
      $state,
      $q
) {
   'ngInject';
   const vm = $scope;
   const STAGE_STATES = {
      'Inactive': 0,
      'Active': 1,
      'Passed': 2,
      'NotPassed': 3
   };
   const STAGE_TYPES = {
      'MainStage' : 1,
      'HireStage' : 2,
      'RejectStage' : 3
   };
   vm.stagesToShow = [];
   vm.vacancyStageInfoComposedObjectsToShow = vm.vacancyStageInfosComposedByCandidateIdVacancyId;
   vm.stageQueries = [];

   function _init() {
      if (vm.parentEntity === 'vacancy') {
         vm.rejectStages = filter(vm.vacancyStages, ['stage.stageType', STAGE_TYPES.RejectStage]);
         vm.stagesToShow = calculateVacancyStagesEntitiesCount(
               vm.vacancyStageInfosComposedByCandidateIdVacancyId);
      } else if (vm.parentEntity === 'candidate') {
         vm.vacancyStageInfosComposedByCandidateIdVacancyId = map(
               vm.vacancyStageInfosComposedByCandidateIdVacancyId, findAndSetRejectStagesFor);
         if (some(vm.vacancyStageInfosComposedByCandidateIdVacancyId)) {
            //TODO intersect of stages
            vm.stagesToShow = calculateVacancyStagesEntitiesCount(vm.vacancyStageInfosComposedByCandidateIdVacancyId);
         }
      }
   }

   function findAndSetRejectStagesFor(vacancyStage)  {
      return Object.assign (vacancyStage, {
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
   _init();

   vm.callStagesDialog = (entityStageObject) => {
      let stageFlow = entityStageObject.stageFlow ? entityStageObject.stageFlow : vm.vacancyStages;
      showStagesFlowDialogFor(entityStageObject, stageFlow)
         .then(changedVSIs => updateCandidateStagesForWith(entityStageObject, changedVSIs))
         .then(notifySuccess)
         .catch(notChangedVSIs => updateCandidateStagesForWith(entityStageObject, notChangedVSIs));
   };

   function showStagesFlowDialogFor(entityStageObject, vacancyStages) {
      let dialogResult = $q.defer();
      let mainStages = filter(vacancyStages, ['stage.stageType', STAGE_TYPES.MainStage]);
      let vacancyStagesEntitiesVSIs = getVacancyStageInfosToEdit(entityStageObject, mainStages);
      let rejectVacancyStageInfosContainer = keepRejectVSIs(entityStageObject, vacancyStages);
      let hireVacancyStageInfoContainer = keepHireVSI(entityStageObject, vacancyStages);
      let scope = {
         vacancyStagesEntitiesVSIs,
         stageClick: vm.stageClick,
         entityStageObject
      };
      let buttons = [
         {
            name: $translate.instant('COMMON.CANCEL'),
            func: () => {
               vacancyStagesEntitiesVSIs = [
                  ...vacancyStagesEntitiesVSIs,
                  ...rejectVacancyStageInfosContainer,
                  hireVacancyStageInfoContainer
               ];
               dialogResult.reject(vacancyStagesEntitiesVSIs);
            }
         },
         {
            name: $translate.instant('COMMON.APLY'),
            func: () => {
               let updatedStagesWithRejectedAndHire = [
                  ...vacancyStagesEntitiesVSIs,
                  ...rejectVacancyStageInfosContainer,
                  hireVacancyStageInfoContainer
               ];
               dialogResult.resolve(updatedStagesWithRejectedAndHire);
            }
         }
      ];
      UserDialogService.dialog($translate.instant('Candidate stages'),
                  manyStageCommentDialogTemplate, buttons, scope);
      return dialogResult.promise;
   }

   function getVacancyStageInfosToEdit(entityStageObject, mainStages) {
      return cloneDeep(map(mainStages, (vacancyStage) => {
         let showCommentArea = false;
         let stageVacancyStageInfo = find(entityStageObject.vacancyStageInfos, { stageId: vacancyStage.stage.id });
         let stageState = STAGE_STATES.Inactive;
         let vsiClass = '';
         if (stageVacancyStageInfo) {
            stageVacancyStageInfo.stage = vacancyStage.stage;
            switch (stageVacancyStageInfo.stageState) {
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
               }
               default: {
                  stageState = STAGE_STATES.Inactive;
                  vsiClass = 'inactive';
                  break;
               }
            }
            if (stageVacancyStageInfo.stage.isCommentRequired) {
               showCommentArea = true;
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

   function updateCandidateStagesForWith(entityStageObject, vacancyStagesEntitiesVSIs) {
      entityStageObject.vacancyStageInfos = map(filter(vacancyStagesEntitiesVSIs, stageVsi =>
            !isNil(stageVsi.vsi)),
      stageVsi => stageVsi.vsi);
      recalculateCurrentStageId(entityStageObject);
      vm.stagesToShow = calculateVacancyStagesEntitiesCount(vm.vacancyStageInfosComposedByCandidateIdVacancyId);
      return $q.when();
   }

   function keepRejectVSIs(entityStageObject, vacancyStages) {
      let rejectStages = filter(vacancyStages, ['stage.stageType', STAGE_TYPES.RejectStage]);
      return map(rejectStages, (extStage) => {
         return {
            stage: extStage,
            vsi: find(entityStageObject.vacancyStageInfos, { stageId: extStage.stage.id })
         };
      });
   }

   function keepHireVSI(entityStageObject, vacancyStages) {
      let hireStage = find(vacancyStages, ['stage.stageType', STAGE_TYPES.HireStage]);
      return {
         stage: hireStage,
         vsi: find(entityStageObject.vacancyStageInfos, { stageId: hireStage.stage.id })
      };
   }


   vm.hire = (entityStageObject) => {
      callDatepickDialogFor(entityStageObject)
           .then(hireDate => {
              updateCurrentStage(entityStageObject);
              createHireStage(entityStageObject, hireDate);
              recalculateCurrentStageId(entityStageObject);
              vm.stagesToShow = calculateVacancyStagesEntitiesCount(vm.vacancyStageInfosComposedByCandidateIdVacancyId);
           })
           .then(() => {
              if (vm.parentEntity === 'vacancy') {
                 vm.closevacancy(entityStageObject.candidate);
              } else {
                 entityStageObject.vacancy.closingCandidateId = entityStageObject.candidate.id;
                 entityStageObject.vacancy.closingCandidate = entityStageObject.candidate;
              }
           });
   };
   function callDatepickDialogFor(candidateStage) {
      let dialogResult = $q.defer();
      let dialogTransferObject = {};
      let scope = {
         dialogTransferObject,
         candidateStage
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
               let chosenDate = new Date(Date.parse(dialogTransferObject.hireDate));
               let todayDate = new Date();
               if (chosenDate > todayDate) {
                     //TODO: reject 'cause chosen date can't be later than today
               }
               dialogResult.resolve(chosenDate);
            }
         }
      ];
      UserDialogService.dialog($translate.instant('Hiring'),
                  hireDateDialogTemplate, buttons, scope);
      return dialogResult.promise;
   }

   function updateCurrentStage(candidateStage) {
      let currentVacancyStageInfo = find(candidateStage.vacancyStageInfos, {stageId: candidateStage.currentStageId});
      currentVacancyStageInfo.stageState = STAGE_STATES.Passed;
      currentVacancyStageInfo.dateOfPass = new Date();
   }

   function createHireStage(entityStageObject, hireDate) {
      let hireStage = find(entityStageObject.stageFlow, ['stage.stageType', STAGE_TYPES.HireStage]);
      let hiredVacancyStageInfo = {
         vacancyId: entityStageObject.vacancyId,
         candidateId: entityStageObject.candidateId,
         stage: hireStage,
         stageId: hireStage.stage.id,
         comment: { message: '' },
         stageState: STAGE_STATES.Active,
         createdOn: hireDate.toISOString()
      };
      entityStageObject.vacancyStageInfos.push(hiredVacancyStageInfo);
   }

   vm.reject = (candidateStage, rejectStage) => {
      let latestActiveVacancyStageInfo = find(candidateStage.vacancyStageInfos, { stageState: STAGE_STATES.Active});
      if (latestActiveVacancyStageInfo) {
         latestActiveVacancyStageInfo.stageState = STAGE_STATES.NotPassed;
      }
      let rejectedVsi = {
         vacancyId: candidateStage.vacancyId,
         candidateId: candidateStage.candidateId,
         stage: rejectStage.stage,
         stageId: rejectStage.stage.id,
         //TODO: open dialog to write a comment
         comment: { message: 'rejected' },
         stageState: STAGE_STATES.Active,
         createdOn: (new Date()).toISOString()
      };
      candidateStage.vacancyStageInfos.push(rejectedVsi);
      recalculateCurrentStageId(candidateStage);
      vm.stagesToShow = calculateVacancyStagesEntitiesCount(vm.vacancyStageInfosComposedByCandidateIdVacancyId);
   };

   function recalculateCurrentStageId(entityStageObject) {
      entityStageObject.currentStageId = currentStageOf(entityStageObject);
   }

   vm.candidateCount = (extStage) => {
      return extStage.candidates.length ? extStage.candidates.length : '';
   };

   vm.queryByStage = (extendedStage) => {
      if (extendedStage) {
         vm.vacancyStageInfoComposedObjectsToShow = extendedStage.candidates;
      } else {
         vm.vacancyStageInfoComposedObjectsToShow = vm.vacancyStageInfosComposedByCandidateIdVacancyId;
      }
   };

   vm.callRejectButton = (candidateStage) => {
      candidateStage.rejectButtonClicked = !candidateStage.rejectButtonClicked;
   };

   vm.goCandidate = (candidateId) => {
      $state.go('candidateProfile', { _data: null, candidateId });
   };

   vm.goVacancy = (vacancyId) => {
      $state.go('vacancyView', { _data: null, vacancyId });
   };

   vm.filterByStage = (selectedStage) => {
      selectedStage._isPressed = !selectedStage._isPressed;
      if (selectedStage._isPressed) {
         addToQuery(selectedStage);
      } else {
         removeFromQuery(selectedStage);
      }
      vm.vacancyStageInfoComposedObjectsToShow = needToPerformQuery() ?
            performQuery() : vm.vacancyStageInfosComposedByCandidateIdVacancyId;
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

   vm.stageClick = (selectedStageAndVsi, entityStageObject, vacancyStagesEntitiesVSIs) => {
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
   };

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
      return !(currentStageAndVsi.stage.stage.isCommentRequired && !currentStageAndVsi.vsi.comment.message);
   }

   function setCurrentToPassed(currentStageAndVsi) {
      currentStageAndVsi.areaIsValid = true;
      currentStageAndVsi.stageState = STAGE_STATES.Passed;
      currentStageAndVsi.class = 'passed';
      currentStageAndVsi.vsi.stageState = STAGE_STATES.Passed;
      currentStageAndVsi.vsi.dateOfPass = (new Date()).toISOString();
   }

   function setSelectedStageToActive(selectedStageAndVsi, candObject) {
      selectedStageAndVsi.stageState = STAGE_STATES.Active;
      selectedStageAndVsi.class = 'active';
      selectedStageAndVsi.showCommentArea = selectedStageAndVsi.stage.stage.isCommentRequired && true;
      selectedStageAndVsi.vsi = {
         vacancyId: candObject.vacancyId,
         candidateId: candObject.candidateId,
         stage: selectedStageAndVsi.stage,
         stageId: selectedStageAndVsi.stage.stage.id,
         comment: selectedStageAndVsi.showCommentArea ? { message: '' } : null,
         stageState: STAGE_STATES.Active
      };
   }

   function setSelectedToInactive(selectedStageAndVsi) {
      selectedStageAndVsi.stageState = STAGE_STATES.Inactive;
      selectedStageAndVsi.class = 'inactive';
      selectedStageAndVsi.showCommentArea = false;
      selectedStageAndVsi.vsi = null;
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
         latestPassedStageAndVsi.vsi.dateOfPass = null;
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
