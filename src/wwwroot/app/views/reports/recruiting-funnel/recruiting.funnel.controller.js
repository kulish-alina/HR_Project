import {
   set,
   each,
   assign,
   remove,
   groupBy,
   map,
   last,
   max
} from 'lodash';

export default function RecruitingFunnelController(
   $scope,
   $state,
   ThesaurusService,
   VacancyService
) {
   'ngInject';

   const vm                                       = $scope;
   vm.viewVacancy                                 = viewVacancy;
   vm.viewCandidate                               = viewCandidate;
   vm.clear                                       = clear;
   vm.candidatesGropedByStage                     = {};
   vm.genereteReportForSelectedVacancy            = genereteReportForSelectedVacancy;
   vm.filterCandidatesGropedByStageByCandidateId  = filterCandidatesGropedByStageByCandidateId;
   vm.tableRows                                   = [];
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

   function genereteReportForSelectedVacancy() {
      vm.tableRows = [];
      _groupCandidatesInProgressByStages();
      _setTableRows();
      _genereteRecruitingFunnel();
   }

   function filterCandidatesGropedByStageByCandidateId(group, num) {
      if (group) {
         if (group[num] !== undefined && group[num].candidate !== undefined) {
            return `${group[num].candidate.lastName} ${group[num].candidate.firstName}`;
         }
      }
   }

   function _groupCandidatesInProgressByStages() {
      let userGroupObject = groupBy(vm.selectedVacancy.candidatesProgress, x => x.candidateId);
      vm.cleanedUserGroup = map(userGroupObject, userGroup => {
         return last(userGroup);
      });
      set(vm, 'candidatesGropedByStage', groupBy(vm.cleanedUserGroup, x => x.stageId));
   }

   function _setTableRows() {
      let arr = map(vm.candidatesGropedByStage, candidatesGroupe => {
         return candidatesGroupe.length;
      });
      for (let i = 0; i < max(arr); i++) {
         vm.tableRows.push(i);
      }
   }

   function _genereteRecruitingFunnel() {
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
      vm.tableRows = [];
   }

   function viewVacancy() {
      $state.go('vacancyView', {_data: null, vacancyId: vm.selectedVacancy.id});
   }

   function viewCandidate(selectedCandidateId) {
      $state.go('candidateProfile', {_data: null, candidateId: selectedCandidateId});
   }
}
