import {
   set,
   each,
   remove,
   filter,
   find
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
   vm.candidatesReportParametrs.candidatesIds = [];
   vm.locations                               = [];
   vm.selectedLocations                       = [];
   vm.generateCandidatesReport                = generateCandidatesReport;
   vm.candidatesAutocomplete                  = CandidateService.autocomplete;
   vm.stageSwitch                             = stageSwitch;
   vm.locationSwitch                          = locationSwitch;
   vm.clear                                   = clear;
   vm.selectedStageIds                        = [];
   vm.tableColumnsCount                       = [];
   vm.responseTableObject                     = {};
   vm.filterArrayByProperty                   = filterArrayByProperty;
   vm.exportToExcel                           = exportToExcel;

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
               vm.candidatesReportParametrs.locationsIds.push(location.id);
               vm.selectedLocations.push(location);
            }
         });
         vm.isInited = true;
      });
   }());

   function generateCandidatesReport(form) {
      vm.candidatesReportParametrs.startDate = vm.startDate;
      vm.candidatesReportParametrs.endDate = vm.endDate;
      let validateObj = _reportConditionsValidation();
      if (validateObj.isValid) {
         ValidationService.validate(form).then(() => {
            ReportsService.getDataForCandidatesReport(vm.candidatesReportParametrs).then(response => {
               vm.responseTableObject = response;
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
      vm.startDate                 = null;
      vm.endDate                   = null;
      vm.candidatesReportParametrs = {};
      vm.selectedLocations         = [];
      vm.responseTableObject       = {};
      _addDefaultPropertyToStages(vm.stages);
      _calculateTableColumnsCount(vm.selectedStageIds.length);
   }

   function filterArrayByProperty(obj, id, type) {
      if (obj !== undefined) {
         let rep = find(obj.stages, {stageId: id});
         return rep === undefined ? '' : rep[type];
      }
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

   function _addDefaultPropertyToStages(stages) {
      each(stages, (stage) => {
         set(stage, '_isPressed', true);
         vm.selectedStageIds.push(stage.id);
      });
   }

   function exportToExcel() {
   }

   function _calculateTableColumnsCount(count) {
      vm.tableColumnsCount = Array.apply(null, Array(count)).map((x, i) => {
         return i;
      });
   }

   function _reportConditionsValidation() {
      if (!vm.candidatesReportParametrs.startDate &&
          !vm.candidatesReportParametrs.endDate &&
          !vm.candidatesReportParametrs.candidatesIds.length) {
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
      return {
         isValid: true
      };
   }
}
