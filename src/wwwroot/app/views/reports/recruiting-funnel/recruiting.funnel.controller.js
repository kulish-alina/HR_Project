import {
   set,
   each,
   remove,
   groupBy,
   map,
   last,
   max,
   isEmpty,
   reduce,
   toArray
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
   vm.stageSwitch                                 = stageSwitch;
   vm.selectedStageIds                            = [];
   vm.vacancySearchConditions                     = {};
   vm.vacancySearchConditions.current             = 0;
   vm.vacancySearchConditions.size                = 20;

   (function init() {
      VacancyService.search(vm.vacancySearchConditions)
         .then(response => set(vm, 'vacancies', response.vacancies));
      ThesaurusService.getThesaurusTopics('stage')
         .then(topic => {
            set(vm, 'stages', topic);
            _addDefaultPropertyToStages(vm.stages);
         });
      if (!isEmpty(vm.selectedVacancy)) {
         genereteReportForSelectedVacancy();
      }
   }());

   function stageSwitch(stage) {
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
      if (group && group[num] && group[num].candidate) {
         return `${group[num].candidate.lastName} ${group[num].candidate.firstName}`;
      }
   }

   function clear() {
      vm.selectedVacancy = {};
      vm.candidatesGropedByStage = {};
      vm.selectedStageIds = [];
      vm.tableRows = [];
      _addDefaultPropertyToStages(vm.stages);
   }

   function viewVacancy() {
      $state.go('vacancyView', {_data: vm.selectedVacancy, vacancyId: vm.selectedVacancy.id,
                                vacancyGoBack: vm.selectedVacancy});
   }

   function viewCandidate(selectedCandidate) {
      $state.go('candidateProfile', {_data: selectedCandidate, candidateId: selectedCandidate.id,
                                     vacancyGoBack: vm.selectedVacancy});
   }

   function _addDefaultPropertyToStages(stages) {
      each(stages, (stage) => {
         set(stage, '_isPressed', true);
         vm.selectedStageIds.push(stage.id);
      });
   }

   function _groupCandidatesInProgressByStages() {
      let userGroupObject = groupBy(vm.selectedVacancy.candidatesProgress, 'candidateId');
      let cleanedFromDuplicatesUserGroup = map(userGroupObject, last);
      set(vm, 'candidatesGropedByStage', groupBy(cleanedFromDuplicatesUserGroup, 'stageId'));
   }

   function _setTableRows() {
      let candidatesGropesLength = map(vm.candidatesGropedByStage, 'length');
      let countTableRows = max(candidatesGropesLength);
      for (let i = 0; i < countTableRows; i++) {
         vm.tableRows.push(i);
      }
   }

   function _genereteRecruitingFunnel() {
      const d = reduce(vm.candidatesGropedByStage, (resultObj, val, key) => {
         (resultObj[val[key]] || (resultObj[val[key]] = [])).push(val.length);
         return resultObj;
      }, {});
      const data = toArray(d);
      console.log('data', data);
      let D3Funnel = require('d3-funnel');
//      const data = [
//         ['Grade1', 15],
//         ['Grade2', 10],
//         ['Grade3', 4],
//         ['Grade4', 2]
//      ];
      const options = {
         chart: {
            width: 200,
            height: 100
         }
      };
      let chart = new D3Funnel('#funnel');
      chart.draw(data, options);
   }
}
