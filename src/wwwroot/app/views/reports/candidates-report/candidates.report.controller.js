import {
   set,
   each
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
   vm.candidatesReportParametrs.locationIds   = [];
   vm.candidatesReportParametrs.candidateIds  = [];
   vm.locations                               = [];
   vm.selectedLocations                       = [];
   vm.generateCandidatesReport                = generateCandidatesReport;
   vm.candidatesAutocomplete                  = CandidateService.autocomplete;
   vm.clear                                   = clear;

   (function init() {
      ThesaurusService.getThesaurusTopics('stage').then(topic => {
         set(vm, 'stages', topic);
      });
      ThesaurusService.getThesaurusTopics('city').then(locations => {
         each(locations, (location) => {
            if (LIST_OF_LOCATIONS.includes(location.title)) {
               vm.locations.push(location);
            }
            set(location, 'selected', vm.candidatesReportParametrs.locationIds.length && vm.candidatesReportParametrs.locationIds.includes(location.id)); // eslint-disable-line max-len
         });
      });
   }());

   function generateCandidatesReport(form) {
      let validateObj = _reportConditionsValidation();
      if (validateObj.isValid) {
         ValidationService.validate(form).then(() => {
            ReportsService.getDataForCandidatesReport(vm.candidatesReportParametrs).then(resp => {
               vm.candidatesReportParametrs.startDate = resp.startDate;
               vm.candidatesReportParametrs.endDate = resp.endDate;
            });
         });
      } else {
         UserDialogService.notification(validateObj.errorMessage, 'error');
      }
   }

   function clear() {
      vm.candidatesReportParametrs = {};
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
