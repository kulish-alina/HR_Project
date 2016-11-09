import {
   set,
   each,
   remove,
   filter
} from 'lodash';

const LIST_OF_LOCATIONS = ['Dnipropetrovsk', 'Zaporizhia', 'Lviv', 'Berdiansk'];

export default function CandidatesReportController(
   $scope,
   $translate,
   ThesaurusService,
   CandidateService,
   ReportsService,
   ValidationService,
   UserDialogService
) {
   'ngInject';
   const vm                                   = $scope;
   vm.candidatesReportParametrs               = {};
   vm.candidatesReportParametrs.locationsIds  = [];
   vm.candidatesReportParametrs.candidateIds  = [];
   vm.locations                               = [];
   vm.selectedLocations                       = [];
   vm.generateCandidatesReport                = generateCandidatesReport;
   vm.candidatesAutocomplete                  = CandidateService.autocomplete;
   vm.stageSwitch                             = stageSwitch;
   vm.locationSwitch                          = locationSwitch;
   vm.clear                                   = clear;
   vm.selectedStageIds                        = [];
   vm.tableColumnsCount                       = [];

   (function init() {
      ThesaurusService.getThesaurusTopics('stage').then(topic => {
         set(vm, 'stages', topic);
         _addDefaultPropertyToStages(vm.stages);
         _calculateTableColumnsCount(vm.selectedStageIds.length);
      });
      ThesaurusService.getThesaurusTopics('city').then(locations => {
         each(locations, (location) => {
            if (LIST_OF_LOCATIONS.includes(location.title)) {
               vm.locations.push(location);
            }
            set(location, 'selected', vm.candidatesReportParametrs.locationsIds.length && vm.candidatesReportParametrs.locationsIds.includes(location.id)); // eslint-disable-line max-len
         });
      });
   }());

   function generateCandidatesReport(form) {
      vm.candidatesReportParametrs.startDate = vm.startDate;
      vm.candidatesReportParametrs.endDate = vm.endDate;
      let validateObj = _reportConditionsValidation();
      if (validateObj.isValid) {
         ValidationService.validate(form).then(() => {
            ReportsService.getDataForCandidatesReport(vm.candidatesReportParametrs).then(response => {
               _convertReportForTable(response);
            });
         });
      } else {
         UserDialogService.notification(validateObj.errorMessage, 'error');
      }
   }

   function locationSwitch() {
      vm.selectedLocations = filter(vm.locations, (location) => {
         return vm.candidatesReportParametrs.locationsIds.includes(location.id);
      });
   }

   function clear() {
      vm.startDate = null;
      vm.endDate = null;
      vm.candidatesReportParametrs = {};
      _addDefaultPropertyToStages(vm.stages);
      _calculateTableColumnsCount(vm.selectedStageIds.length);
   }

   function stageSwitch(stage) {
      if (stage._isPressed) {
         stage._isPressed = false;
         remove(vm.selectedStageIds, (stageId) =>  stageId === stage.id);
      } else {
         stage._isPressed = true;
         vm.selectedStageIds.push(stage.id);
      }
      _calculateTableColumnsCount(vm.selectedStageIds.length);
   }

   function _convertReportForTable(report) {
      console.log('report', report);
   }

   function _addDefaultPropertyToStages(stages) {
      each(stages, (stage) => {
         set(stage, '_isPressed', false);
      });
   }

   function _calculateTableColumnsCount(count) {
      vm.tableColumnsCount = Array.apply(null, Array(count)).map((x, i) => {
         return i;
      });
   }

   function _reportConditionsValidation() {
      if (!vm.candidatesReportParametrs.startDate &&
          !vm.candidatesReportParametrs.endDate &&
          !vm.candidatesReportParametrs.candidateIds.length) {
         return {
            isValid: false,
            errorMessage: $translate.instant('DIALOG_SERVICE.EMPTY_CANDIDATE_REPORT_CONDITIONS')
         };
      }
      if (vm.candidatesReportParametrs.startDate && !vm.candidatesReportParametrs.endDate ||
         !vm.candidatesReportParametrs.startDate && vm.candidatesReportParametrs.endDate) {
         return {
            isValid: false,
            errorMessage: $translate.instant('DIALOG_SERVICE.EMPTY_DATE_REPORT_CONDITIONS')
         };
      }
      if (vm.candidatesReportParametrs.startDate > vm.candidatesReportParametrs.endDate) {
         return {
            isValid: false,
            errorMessage: $translate.instant('DIALOG_SERVICE.INVALID_DATES')
         };
      }
      return {
         isValid: true
      };
   }
}
