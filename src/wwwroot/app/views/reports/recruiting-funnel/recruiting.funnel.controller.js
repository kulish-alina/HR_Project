import {
   set,
   each,
   assign,
   remove
} from 'lodash';

export default function RecruitingFunnelController(
   $scope,
   $state,
   ThesaurusService
) {
   'ngInject';

   const vm                                       = $scope;
   vm.formingRecruitingFunnelReport               = formingRecruitingFunnelReport;
   vm.viewVacancy                                 = viewVacancy;
   vm.viewCandidate                               = viewCandidate;
   vm.clear                                       = clear;
   vm.candidatesGropedByStage                     = {};
   vm.selectVacancy                               = selectVacancy;
   vm.addStageFilter                              = addStageFilter;
   vm.selectedStageIds                            = [];

   (function init() {
      ThesaurusService.getThesaurusTopics('stage').then(topic => {
         set(vm, 'stages', topic);
         each(vm.stages, (stage) => {
            assign(stage, {_isPressed: true});
            vm.selectedStageIds.push(stage.id);
         });
      });
   }());

   function formingRecruitingFunnelReport() {
   }

   function selectVacancy() {
      vm.candidatesGropedByStage = {};
      each(vm.selectedVacancy.candidatesProgress, (candidateStage) => {
         if (vm.candidatesGropedByStage[candidateStage.stageId]) {
            vm.candidatesGropedByStage[candidateStage.stageId].push(candidateStage.candidate);
         } else if (!vm.candidatesGropedByStage[candidateStage.stageId]) {
            vm.candidatesGropedByStage[candidateStage.stageId] = [];
            vm.candidatesGropedByStage[candidateStage.stageId].push(candidateStage.candidate);
         }
      });
   }

   function addStageFilter(stage) {
      if (stage._isPressed) {
         stage._isPressed = false;
         remove(vm.selectedStageIds, (stageId) =>  stageId === stage.id);
      } else {
         stage._isPressed = true;
         vm.selectedStageIds.push(stage.id);
      }
   }

   function clear() {
      vm.selectedVacancy = {};
      vm.selectedStageIds = [];
      each(vm.stages, (stage) => {
         stage._isPressed = false;
      });
      vm.candidatesGropedByStage = {};
   }

   function viewVacancy() {
      $state.go('vacancyView', {_data: null, vacancyId: vm.selectedVacancy.id});
   }

   function viewCandidate(selectedCandidateId) {
      $state.go('candidateProfile', {_data: null, candidateId: selectedCandidateId});
   }
}
