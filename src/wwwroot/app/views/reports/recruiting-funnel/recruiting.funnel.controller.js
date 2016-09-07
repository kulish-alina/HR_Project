import {
   set,
   each
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
   vm.clear                                       = clear;
   vm.candidatesGropedByStage                     = {};

   (function init() {
      ThesaurusService.getThesaurusTopics('stage').then(topic => set(vm, 'stages', topic));
   }());

   function formingRecruitingFunnelReport() {
      _formingDataForRecruitingFunnelReport();
   }

   function _formingDataForRecruitingFunnelReport() {
      each(vm.selectedVacancy.candidatesProgress, (candidateStage) => {
         if (vm.candidatesGropedByStage[candidateStage.stageId]) {
            vm.candidatesGropedByStage[candidateStage.stageId].push(candidateStage.candidate);
         } else if (!vm.candidatesGropedByStage[candidateStage.stageId]) {
            vm.candidatesGropedByStage[candidateStage.stageId] = [];
            vm.candidatesGropedByStage[candidateStage.stageId].push(candidateStage.candidate);
         }
      });
   }

   function clear() {
      vm.selectedVacancy = {};
   }

   function viewVacancy() {
      $state.go('vacancyView', {_data: null, vacancyId: vm.selectedVacancy.id});
   }
}
