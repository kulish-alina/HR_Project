import template from './candidate-vacancy-info.directive.html';
import './candidate-vacancy-info.scss';
import manyStageCommentDialogTemplate from './many-stage-comment-adding-dialog.template.html';
import hireDateDialogTemplate from './hire-date-dialog.template.html';

import {
      map,
      filter,
      find,
      curry,
      cloneDeep,
      reduce,
      maxBy,
      union,
      forEach
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
         composedby: '=',
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

function CandidateVacancyInfoController($scope,
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
   vm.objectsToShow = vm.composedby;
   vm.stageQueries = [];

   let curriedUpdateCandidateStage = curry(updateCandidateStages, 2);

   function _init() {
      if (vm.parentEntity === 'vacancy') {
         vm.rejectStages = filter(vm.vacancyStages, (extStage) => {
            return extStage.stage.stageType === STAGE_TYPES.RejectStage;
         });
         let withoutHireStage = filter(vm.vacancyStages, (extStage) => {
            return extStage.stage.stageType !== STAGE_TYPES.HireStage;
         });
         vm.stagesToShow = calculateVacancyStagesCandidatesCount(withoutHireStage, vm.composedby);
      } else if (vm.parentEntity === 'candidate') {
         forEach(vm.composedby, (vacancyStage) => {
            vacancyStage.rejectStages = filter(vacancyStage.stageFlow, (extStage) => {
               return extStage.stage.stageType === STAGE_TYPES.RejectStage;
            });
         });
         if (vm.composedby[0]) {
            //TODO intersect of stages
            let vacanciesFlowIntersection = vm.composedby[0].stageFlow;
            let withoutHireStage = filter(vacanciesFlowIntersection, (extStage) => {
               return extStage.stage.stageType !== STAGE_TYPES.HireStage;
            });
            vm.stagesToShow = calculateVacancyStagesCandidatesCount(withoutHireStage, vm.composedby);
         }
      }
   }
   _init();

   vm.reject = (candidateStage, rejectStage) => {
      let latestActiveVsi = find(candidateStage.stages, { stageState: STAGE_STATES.Active});
      if (latestActiveVsi) {
         latestActiveVsi.stageState = STAGE_STATES.NotPassed;
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
      candidateStage.stages.push(rejectedVsi);
      recalculateCurrentStageId(candidateStage);
      let stageFlow = vm.vacancyStages;
      if (!stageFlow) {
         stageFlow = candidateStage.stageFlow;
      }
      let withoutHireStage = filter(stageFlow, (extStage) => {
         return extStage.stage.stageType !== STAGE_TYPES.HireStage;
      });
      vm.stagesToShow = calculateVacancyStagesCandidatesCount(withoutHireStage, vm.composedby);
   };

   function calculateVacancyStagesCandidatesCount(vacancyStages, composedBy) {
      return map(vacancyStages, (extStage) => {
         let stageCandCount = reduce(composedBy, (count, vsi) => {
            return vsi.currentStageId === extStage.stage.id ? count + 1 : count + 0;
         }, 0);
         return Object.assign(extStage, {
            entitiesCount: stageCandCount,
            _hasEntities: stageCandCount !== 0,
            _isPressed: false
         });
      });
   }


   function updateCandidateStages(entityStageObject, result) {
      entityStageObject.stages = map(filter(result.vacancyStagesCandidatesVSIs, (stageVsi) => {
         if (stageVsi.vsi) {
            return true;
         }
      }), (stageVsi) => {
         return stageVsi.vsi;
      });
      recalculateCurrentStageId(entityStageObject);
      let stageFlow = vm.vacancyStages;
      if (!stageFlow) {
         stageFlow = entityStageObject.stageFlow;
      }
      let withoutHireStage = filter(stageFlow, (extStage) => {
         return extStage.stage.stageType !== STAGE_TYPES.HireStage;
      });
      vm.stagesToShow = calculateVacancyStagesCandidatesCount(withoutHireStage, vm.composedby);
      return $q.when();
   }

   function recalculateCurrentStageId(entityStageObject) {
      entityStageObject.currentStageId = currentStageOf(entityStageObject);
      return $q.when();
   }

   vm.candidateCount = (extStage) => {
      return extStage.candidates.length ? extStage.candidates.length : '';
   };

   vm.queryByStage = (extStage) => {
      if (extStage) {
         vm.objectsToShow = extStage.candidates;
      } else {
         vm.objectsToShow = vm.composedby;
      }
   };

   vm.callRejectButton = (candidateStage) => {
      candidateStage.rejectButtonClicked = !!!candidateStage.rejectButtonClicked;
   };

   vm.callStagesDialog = (entityStageObject) => {
      let stageFlow = entityStageObject.stageFlow ? entityStageObject.stageFlow : vm.vacancyStages;
      showManyStagesDialogFor(stageFlow, entityStageObject)
         .then(curriedUpdateCandidateStage(entityStageObject))
         .then(notifySuccess)
         .catch((result) => {
            curriedUpdateCandidateStage(entityStageObject)(result);
         });
   };

   function showManyStagesDialogFor(vacancyStages, entityStageObject) {
      let dialogResult = $q.defer();
      let dialogTransferObject = {};
      let mainStages = filter(vacancyStages, (extStage) => {
         return extStage.stage.stageType === STAGE_TYPES.MainStage;
      });
      let vacancyStagesCandidatesVSIs = cloneDeep(map(mainStages, (vacancyStage) => {
         let showCommentArea = false;
         let stageVsi = find(entityStageObject.stages, { stageId: vacancyStage.stage.id });
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
               }
               default: {
                  stageState = STAGE_STATES.Inactive;
                  vsiClass = 'inactive';
                  break;
               }
            }
            if (stageVsi.stage.isCommentRequired) {
               showCommentArea = true;
            }
         }
         return {
            stage: vacancyStage,
            vsi: stageVsi,
            showCommentArea,
            stageState,
            class: vsiClass
         };
      }));
      let rejectStages = filter(vacancyStages, (extStage) => {
         return extStage.stage.stageType === STAGE_TYPES.RejectStage;
      });
      let hireStage = filter(vacancyStages, (extStage) => {
         return extStage.stage.stageType === STAGE_TYPES.HireStage;
      })[0];
      let rejectVsisBuff = map(rejectStages, (extStage) => {
         return {
            stage: extStage,
            vsi: find(entityStageObject.stages, { stageId: extStage.stage.id })
         };
      });
      let hireVsisBuff = {
         stage: hireStage,
         vsi: find(entityStageObject.stages, { stageId: hireStage.stage.id })
      };

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
               vacancyStagesCandidatesVSIs = union(vacancyStagesCandidatesVSIs, rejectVsisBuff);
               vacancyStagesCandidatesVSIs = [...vacancyStagesCandidatesVSIs, hireVsisBuff];
               dialogResult.reject({ vacancyStagesCandidatesVSIs });
            }
         },
         {
            name: $translate.instant('COMMON.APLY'),
            func: () => {
               let updatedStagesWithRejected = union(dialogTransferObject.vacancyStagesCandidatesVSIs, rejectVsisBuff);
               updatedStagesWithRejected = [...updatedStagesWithRejected, hireVsisBuff];
               dialogResult.resolve({ vacancyStagesCandidatesVSIs: updatedStagesWithRejected });
            }
         }
      ];
      UserDialogService.dialog($translate.instant('Candidate stages'),
                  manyStageCommentDialogTemplate, buttons, scope);
      return dialogResult.promise;
   }

   vm.goCandidate = (candidateId) => {
      $state.go('candidateProfile', { _data: null, candidateId });
   };

   vm.goVacancy = (vacancyId) => {
      $state.go('vacancyView', { _data: null, vacancyId });
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
         vm.objectsToShow = filter(vm.composedby, (entityStageObject) => {
            return filter(vm.stageQueries, (extStage) => {
               return extStage.stage.id === entityStageObject.currentStageId;
            }).length;
         });
      } else {
         vm.objectsToShow = vm.composedby;
      }
   };

   vm.hire = (entityStageObject) => {
      callHireDialog(entityStageObject)
           .then(hireDate => {
              updateCurrentStage(entityStageObject);
              setHireActive(entityStageObject, hireDate);
              recalculateCurrentStageId(entityStageObject);
              let stageFlow = vm.vacancyStages;
              if (!stageFlow) {
                 stageFlow = entityStageObject.stageFlow;
              }
              let withoutHireStage = filter(stageFlow, (extStage) => {
                 return extStage.stage.stageType !== STAGE_TYPES.HireStage;
              });
              vm.stagesToShow = calculateVacancyStagesCandidatesCount(withoutHireStage, vm.composedby);
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
   function callHireDialog(candidateStage) {
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
      let currentVsi = find(candidateStage.stages, {stageId: candidateStage.currentStageId});
      currentVsi.stageState = STAGE_STATES.Passed;
      currentVsi.dateOfPass = new Date();
   }

   function setHireActive(entityStageObject, hireDate) {
      let hireStage = filter(entityStageObject.stageFlow, (extStage) => {
         return extStage.stage.stageType === STAGE_TYPES.HireStage;
      })[0];
      let hiredVsi = {
         vacancyId: entityStageObject.vacancyId,
         candidateId: entityStageObject.candidateId,
         stage: hireStage,
         stageId: hireStage.stage.id,
         comment: { message: '' },
         stageState: STAGE_STATES.Active,
         createdOn: hireDate.toISOString()
      };
      entityStageObject.stages.push(hiredVsi);
   }

   function getCurrentStageAndVsi(vacancyStagesCandidatesVSIs, currentStageId) {
      return $q.when(filter(vacancyStagesCandidatesVSIs, (stageAndVsi) => {
         return stageAndVsi.stage.stage.id === currentStageId;
      })[0]);
   }

   function isCurrentCommentValid(currentStageAndVsi) {
      if (currentStageAndVsi.stage.stage.isCommentRequired && !currentStageAndVsi.vsi.comment.message) {
         return false;
      }
      return true;
   }

   function setCurrentToPassed(currentStageAndVsi) {
      currentStageAndVsi.areaIsValid = true;
      currentStageAndVsi.stageState = STAGE_STATES.Passed;
      currentStageAndVsi.class = 'passed';
      currentStageAndVsi.vsi.stageState = STAGE_STATES.Passed;
      currentStageAndVsi.vsi.dateOfPass = (new Date()).toISOString();
      return $q.when();
   }

   function setSelectedToActive(selectedStageAndVsi, candObject) {
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
         latestPassedStageAndVsi.vsi.dateOfPass = null;
      }
      return $q.when();
   }

   vm.isHiredOrRejected = (entityStageObject) => {
      let notPassed = filter(entityStageObject.stages, (stageAndVsi) => {
         return stageAndVsi.stageState === STAGE_STATES.NotPassed;
      });
      let stageFlow = entityStageObject.stageFlow || vm.vacancyStages;
      let hireStage = filter(stageFlow, (extStage) => {
         return extStage.stage.stageType === STAGE_TYPES.HireStage;
      })[0];
      let hired = filter(entityStageObject.stages, (vsi) => {
         return vsi.stageId === hireStage.stage.id;
      });
      let result = !!(notPassed.length || hired.length);
      return result;
   };

   vm.stageClick = (selectedStageAndVsi, entityStageObject, vacancyStagesCandidatesVSIs) => {
      let notPassed = filter(vacancyStagesCandidatesVSIs, (stageAndVsi) => {
         return stageAndVsi.stageState === STAGE_STATES.NotPassed;
      });
      let stageFlow = entityStageObject.stageFlow || vm.vacancyStages;
      let hireStage = filter(stageFlow, (extStage) => {
         return extStage.stage.stageType === STAGE_TYPES.HireStage;
      })[0];
      let hired = filter(entityStageObject.stages, (vsi) => {
         return vsi.stageId === hireStage.stage.id;
      });
      if (hired.length || notPassed.length) {
         return;
      }
      let currentExtStage = filter(vacancyStagesCandidatesVSIs, (stageAndVsi) => {
         return stageAndVsi.stageState === STAGE_STATES.Active;
      })[0].stage;
      let currentStageId = currentExtStage.stage.id;
      if (selectedStageAndVsi.stageState === STAGE_STATES.Inactive) {
         getCurrentStageAndVsi(vacancyStagesCandidatesVSIs, currentStageId)
            .then(currentStageAndVsi => {
               if (isCurrentCommentValid(currentStageAndVsi)) {
                  setCurrentToPassed(currentStageAndVsi)
                     .then(setSelectedToActive(selectedStageAndVsi, entityStageObject));
               } else {
                  currentStageAndVsi.areaIsValid = false;
               }
            });
      } else if (selectedStageAndVsi.stageState === STAGE_STATES.Active && currentExtStage.order !== 1) {
         setSelectedToInactive(selectedStageAndVsi)
            .then(setLatestPassedStageToActive(vacancyStagesCandidatesVSIs));
      }
   };

   vm.getStageTitle = (currentStageId, entityStageObject) => {
      let stageFlow = vm.vacancyStages;
      if (!stageFlow) {
         stageFlow = entityStageObject.stageFlow;
      }
      if (entityStageObject && entityStageObject.stageFlow) {
         stageFlow = entityStageObject.stageFlow;
      }
      let stage = filter(stageFlow, (extStage) => {
         return extStage.stage.id === currentStageId;
      })[0];
      return stage.stage.title;
   };

   function notifySuccess() {
      UserDialogService.notification('Stage succesfully passed', 'notification');
      return $q.when();
   }

   function currentStageOf(entityStageObject) {
      let currentStageId = filter(entityStageObject.stages, (vsi) => {
         return vsi.stageState === STAGE_STATES.Active;
      })[0].stageId;
      return currentStageId;
   }

   vm.getDate = (composedStageObject) => {
      let vacancyStageInfo = find(composedStageObject.stages, { stageId: composedStageObject.latestStageId });
      let date = '';
      if (vacancyStageInfo) {
         date = vacancyStageInfo.createdOn;
      }
      return date;
   };

   vm.getComment = (composedStageObject) => {
      let vacancyStageInfo = find(composedStageObject.stages, { stageId: composedStageObject.latestStageId });
      let comment = '';
      if (vacancyStageInfo && vacancyStageInfo.comment) {
         comment = vacancyStageInfo.comment.message;
      }
      return comment;
   };
}
