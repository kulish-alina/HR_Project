import popupDialog from './popup-info.view.html';
import {
   set,
   each,
   remove,
   groupBy,
   map,
   max,
   isEmpty,
   reduce,
   find,
   toNumber,
   round
} from 'lodash';

const D3_FUNNEL = require('d3-funnel');
const arrow = '\u2192';
const colorsOfFunnelBlocks = [
   '#7fafb1',
   '#669fa2',
   '#4c8f92',
   '#327f83',
   '#196f73',
   '#006064',
   '#00565a',
   '#004c50',
   '#004346',
   '#731d19',
   '#833632',
   '#924f4c',
   '#a26866'
];

export default function RecruitingFunnelController(
   $scope,
   $state,
   $translate,
   ThesaurusService,
   VacancyService,
   UserDialogService,
   TransitionsService
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
   vm.autocomplete                                = VacancyService.autocomplete;
   let chart                                      = new D3_FUNNEL('#funnel');

   (function init() {
      VacancyService.search(vm.vacancySearchConditions)
         .then(response => set(vm, 'vacancies', response.vacancies));
      ThesaurusService.getThesaurusTopics('stage')
         .then(topic => {
            set(vm, 'stages', topic);
            _addDefaultPropertyToStages(vm.stages);
            if (!isEmpty(vm.selectedVacancy)) {
               vm.selectedVacancyId = vm.selectedVacancy.id;
               genereteReportForSelectedVacancy();
            }
            return topic;
         });
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
      if (vm.selectedVacancyId) {
         VacancyService.getVacancy(vm.selectedVacancyId).then(response => {
            set(vm, 'selectedVacancy', response);
            vm.tableRows = [];
            _groupCandidatesInProgressByStages();
            _setTableRows();
            _genereteRecruitingFunnel();
         });
      }
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
      vm.selectedVacancyId = null;
      chart.destroy();
   }

   function viewVacancy() {
      TransitionsService.go('vacancyView',
                            {vacancyId: vm.selectedVacancy.id,
                             vacancyGoBack: vm.selectedVacancy});
   }

   function viewCandidate(selectedCandidate) {
      TransitionsService.go('candidateProfile',
                            {candidateId: selectedCandidate.id,
                             vacancyGoBack: vm.selectedVacancy});
   }

   function _addDefaultPropertyToStages(stages) {
      each(stages, (stage) => {
         set(stage, '_isPressed', true);
         vm.selectedStageIds.push(stage.id);
      });
   }

   function _groupCandidatesInProgressByStages() {
      set(vm, 'candidatesGropedByStage', groupBy(vm.selectedVacancy.candidatesProgress, 'stageId'));
   }

   function _setTableRows() {
      let candidatesGropesLength = map(vm.candidatesGropedByStage, 'length');
      let countTableRows = max(candidatesGropesLength);
      for (let i = 0; i < countTableRows; i++) {
         vm.tableRows.push(i);
      }
   }

   function _genereteRecruitingFunnel() {
      chart.destroy();
      let recruitingFunnelData = formingDataToRecruitingFunnel();
      if (recruitingFunnelData.length) {
         const recruitingFunnelOptions = {
            chart: {
               width: 900,
               height: 500
            },
            block: {
               highlight: true,
               fill: {
                  scale: colorsOfFunnelBlocks
               },
               dynamicHeight: true,
               minHeight: 40
            },
            label: {
               format: '{l}:\n{v}{f}',
               fontSize: 10
            },
            events: {
               click: {
                  block: onClickBlockHendler
               }
            }
         };
         chart.draw(recruitingFunnelData, recruitingFunnelOptions);
      }

   }

   function formingDataToRecruitingFunnel() {
      if (vm.selectedVacancy.candidatesProgress.length) {
         let firstValueLength = vm.candidatesGropedByStage[1].length;
         return reduce(vm.candidatesGropedByStage, (resultArr, val, key) => {
            let countAndPercentsValueArray = [];
            countAndPercentsValueArray[0] = val.length;
            countAndPercentsValueArray[1] = ` ${arrow} ${round(((val.length / firstValueLength) * 100), 1)} %`;
            let valueWithLableArray = [];
            valueWithLableArray[0] = find(vm.stages, {id: toNumber(key)}).title;
            valueWithLableArray[1] = countAndPercentsValueArray;
            (resultArr).push(valueWithLableArray);
            return resultArr;
         }, []);
      } else {
         return [];
      }
   }

   function onClickBlockHendler($event) {
      let stageId = find(vm.stages, {title: $event.label.raw}).id;
      let scope = {
         candidatesGroup : vm.candidatesGropedByStage[stageId]
      };
      let buttons = [
         {
            name: $translate.instant('COMMON.CLOSE')
         }
      ];
      UserDialogService.dialog($translate.instant('REPORTS.ADDITIONAL_INFORMATION'), popupDialog, buttons, scope);
   }
}
