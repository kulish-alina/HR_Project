import {
   set,
   each,
   assign,
   remove,
   groupBy,
   map,
   last,
   max,
   isEmpty
} from 'lodash';

export default function RecruitingFunnelController(
   $scope,
   $state,
   ThesaurusService,
   VacancyService
) {
   'ngInject';

   const vm                                       = $scope;
   vm.selectedVacancy                             = $state.previous.params.vacancyGoBack || {};
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
      if (!isEmpty(vm.selectedVacancy)) {
         genereteReportForSelectedVacancy();
      }
   }());

   function addStageFilter(stage) {
      if (stage._isPressed) {
         stage._isPressed = false;
         remove(vm.selectedStageIds, (stageId) =>  stageId === stage.id);
      } else {
         stage._isPressed = true;
         vm.selectedStageIds.push(stage.id);
      }
   }

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
      $state.go('vacancyView', {_data: vm.selectedVacancy, vacancyId: vm.selectedVacancy.id,
                                vacancyGoBack: vm.selectedVacancy});
   }

   function viewCandidate(selectedCandidate) {
      $state.go('candidateProfile', {_data: selectedCandidate, candidateId: selectedCandidate.id,
                                     vacancyGoBack: vm.selectedVacancy});
   }

   function _groupCandidatesInProgressByStages() {
      let userGroupObject = groupBy(vm.selectedVacancy.candidatesProgress, x => x.candidateId);
      let cleanedFromDuplicatesUserGroup = map(userGroupObject, userGroup => {
         return last(userGroup);
      });
      set(vm, 'candidatesGropedByStage', groupBy(cleanedFromDuplicatesUserGroup, x => x.stageId));
   }

   function _setTableRows() {
      let candidatesGropesLength = map(vm.candidatesGropedByStage, candidatesGroupe => {
         return candidatesGroupe.length;
      });
      let countTableRows = max(candidatesGropesLength);
      for (let i = 0; i < countTableRows; i++) {
         vm.tableRows.push(i);
      }
   }

   function _genereteRecruitingFunnel() {
   }
}
