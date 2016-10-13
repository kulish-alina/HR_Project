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

const arrow = '\u2192';
const colorsOfFunnelBlocks = [
   '#006064',
   '#00838F',
   '#0097A7',
   '#00ACC1',
   '#00BCD4',
   '#26C6DA',
   '#4DD0E1',
   '#80DEEA',
   '#B2EBF2',
   '#E0F7FA',
   '#84FFFF',
   '#18FFFF'
];

export default function RecruitingFunnelController(
   $scope,
   $state,
   $translate,
   ThesaurusService,
   VacancyService,
   UserDialogService
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
            if (!isEmpty(vm.selectedVacancy)) {
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
      let D3Funnel = require('d3-funnel');
      let recruitingFunnelData = formingDataToRecruitingFunnel();
      const recruitingFunnelOptions = {
         chart: {
            width: 900,
            height: 350,
            bottomWidth: 1 / 2,
            curve: {
               enabled: true
            }
         },
         block: {
            highlight: true,
            fill: {
               type: 'gradient',
               scale: colorsOfFunnelBlocks
            },
            dynamicHeight: true,
            minHeight: 20
         },
         label: {
            format: '{l}: {v} {f}'
         },
         events: {
            click: {
               block: onClickBlockHendler
            }
         }
      };
      let chart = new D3Funnel('#funnel');
      chart.draw(recruitingFunnelData, recruitingFunnelOptions);
   }

   function formingDataToRecruitingFunnel() {
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
