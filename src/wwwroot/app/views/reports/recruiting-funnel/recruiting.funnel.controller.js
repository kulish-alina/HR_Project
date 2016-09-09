import {
   set,
   each,
   assign,
   remove
} from 'lodash';

export default function RecruitingFunnelController(
   $scope,
   $state,
   ThesaurusService,
   VacancyService
) {
   'ngInject';

   const vm                                       = $scope;
   vm.genereteRecruitingFunnel                    = genereteRecruitingFunnel;
   vm.viewVacancy                                 = viewVacancy;
   vm.viewCandidate                               = viewCandidate;
   vm.clear                                       = clear;
   vm.candidatesGropedByStage                     = {};
   vm.selectVacancy                               = selectVacancy;
   vm.addStageFilter                              = addStageFilter;
   vm.selectedStageIds                            = [];
   vm.vacancySearchConditions                     = {};
   vm.vacancySearchConditions.current             = 0;
   vm.vacancySearchConditions.size                = 20;
   vm.vacancies                                   = [];


   (function init() {
      VacancyService.search(vm.vacancySearchConditions).then(response => vm.vacancies = response.vacancies);
      ThesaurusService.getThesaurusTopics('stage').then(topic => {
         set(vm, 'stages', topic);
         each(vm.stages, (stage) => {
            assign(stage, {_isPressed: true});
            vm.selectedStageIds.push(stage.id);
         });
      });
   }());

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
      genereteRecruitingFunnel();
   }

   function genereteRecruitingFunnel() {
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
         stage._isPressed = true;
         vm.selectedStageIds.push(stage.id);
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
