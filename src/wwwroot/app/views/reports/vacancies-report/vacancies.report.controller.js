import {
   set,
   each,
   remove,
   isEmpty,
   reduce,
   flatten,
   map,
   groupBy,
   sumBy,
   find
} from 'lodash';

const LIST_OF_LOCATIONS = ['Dnipropetrovsk', 'Zaporizhia', 'Lviv', 'Berdyansk'];

export default function VacanciesReportController(
   $scope,
   $translate,
   UserService,
   ThesaurusService,
   ReportsService,
   ValidationService,
   UserDialogService
) {
   'ngInject';

   const vm                                    = $scope;
   vm.users                                    = [];
   vm.vacanciesReportParametrs                 = {};
   vm.vacanciesReportParametrs.locationIds     = [];
   vm.vacanciesReportParametrs.userIds         = [];
   vm.locations                                = [];
   vm.selectedLocations                        = [];
   vm.selectedUsers                            = [];
   vm.selectedUsersGroupedByLocation           = {};
   vm.clear                                    = clear;
   vm.useLocationField                         = useLocationField;
   vm.useUserField                             = useUserField;
   vm.addLocationIdsToVacanciesReportParametrs = addLocationIdsToVacanciesReportParametrs;
   vm.addUserIdsToVacanciesReportParametrs     = addUserIdsToVacanciesReportParametrs;
   vm.generateVacanciesReport                  = generateVacanciesReport;
   vm.isUserLocationSelected                   = isUserLocationSelected;
   vm.isSelectedUsersGroupedByLocationEmpty    = isSelectedUsersGroupedByLocationEmpty;
   vm.report                                   = {};
   vm.sumReportByStages                        = sumReportByStages;
   vm.filterArrayByProperty                    = filterArrayByProperty;


   (function init() {
      ThesaurusService.getThesaurusTopics('stage').then(topic => set(vm, 'stages', topic));
      ThesaurusService.getThesaurusTopics('city').then(locations => {
         each(locations, (location) => {
            if (LIST_OF_LOCATIONS.includes(location.title)) {
               vm.locations.push(location);
            }
            set(location, 'selected', vm.vacanciesReportParametrs.locationIds.length && vm.vacanciesReportParametrs.locationIds.includes(location.id)); // eslint-disable-line max-len
         });
      });
      UserService.getUsers().then(users => {
         set(vm, 'users', users);
         each(vm.users, (user) => {
            set(user, 'selected', vm.vacanciesReportParametrs.userIds.length && vm.vacanciesReportParametrs.userIds.includes(user.id));        // eslint-disable-line max-len
         });
      });
   }());

   function useLocationField(isActiveLocationField) {
      if (!isActiveLocationField) {
         _clearLocationField();
      }
   }

   function useUserField(isActiveUsersField) {
      if (!isActiveUsersField) {
         _clearUserField();
      }
   }

   function _clearLocationField() {
      vm.vacanciesReportParametrs.locationIds = [];
      vm.selectedLocations = [];
      vm.selectedUsersGroupedByLocation = {};
      each(vm.locations, (location) =>  set(location, 'selected', false));
   }

   function _clearUserField() {
      vm.vacanciesReportParametrs.userIds = [];
      vm.selectedUsers = [];
      vm.selectedUsersGroupedByLocation = {};
      each(vm.users, (user) => set(user, 'selected', false));
   }

   function addLocationIdsToVacanciesReportParametrs(location) {
      if (location.selected && !vm.vacanciesReportParametrs.locationIds.includes(location.id)) {
         vm.vacanciesReportParametrs.locationIds.push(location.id);
         vm.selectedLocations.push(location);
      } else if (!location.selected) {
         remove(vm.vacanciesReportParametrs.locationIds, (locationId) =>  locationId === location.id);
         remove(vm.selectedLocations, {id: location.id});
      }
   }

   function addUserIdsToVacanciesReportParametrs(user) {
      if (user.selected && !vm.vacanciesReportParametrs.userIds.includes(user.id)) {
         vm.vacanciesReportParametrs.userIds.push(user.id);
         if (vm.selectedLocations.length) {
            _convertUsersArrayToHash(user);
         } else {
            vm.selectedUsers.push(user);
         }
      } else if (!user.selected) {
         remove(vm.vacanciesReportParametrs.userIds, (userId) =>  userId === user.id);
         if (vm.selectedLocations.length) {
            remove(vm.selectedUsersGroupedByLocation[user.cityId], {id: user.id});
         } else {
            remove(vm.selectedUsers, (us) =>  us.id === user.id);
         }
      }
   }

   function generateVacanciesReport(form) {
      _reportConditionsValidation();
      ValidationService.validate(form).then(() => {
         ReportsService.getDataForVacancyReport(vm.vacanciesReportParametrs)
         .then(resp => {
            vm.vacanciesReportParametrs.startDate = resp.startDate;
            vm.vacanciesReportParametrs.endDate = resp.endDate;
            _convertReportToHash(resp);
         });
      });
   }

   function _convertReportToHash(report) {
      if (vm.selectedLocations.length) {
         set(vm.report, 'startDateReportGroupedByLocation', _convertToHash('locationId', report.startDateReport));
         set(vm.report, 'endDateReportGroupedByLocation', _convertToHash('locationId', report.endDateReport));
         set(vm.report, 'reportGroupedByLocation', _convertToHash('locationId', report.vacanciesReport));
      } else {
         set(vm.report, 'startDateReportGroupedByUser', _convertToHash('userId', report.startDateReport));
         set(vm.report, 'endDateReportGroupedByUser', _convertToHash('userId', report.endDateReport));
         set(vm.report, 'reportGroupedByUser', _convertToHash('userId', report.vacanciesReport));
      }
   }

   function _convertToHash(key, value) {
      if (key === 'locationId') {
         return reduce(value, (resultObj, x) => {
            if (x.dailyVacanciesStatisticsInfo) {
               (resultObj[x[key]] || (resultObj[x[key]] = [])).push(...x.dailyVacanciesStatisticsInfo);
            } else {
               (resultObj[x[key]] || (resultObj[x[key]] = [])).push(...x.vacanciesStatisticsInfo);
            }
            return resultObj;
         }, {});
      } else {
         let arr = flatten(map(value, (val) => {
            return val.dailyVacanciesStatisticsInfo ?
               val.dailyVacanciesStatisticsInfo : val.vacanciesStatisticsInfo;
         }));
         return groupBy(arr, 'userId');
      }
   }

   function sumReportByStages(report, type) {
      return sumBy(report, type);
   }

   function filterArrayByProperty(report, prop, type) {
      let rep = find(report, {userId: prop});
      return rep ? rep[type] : 0;
   }

   function isUserLocationSelected(user) {
      return isEmpty(vm.vacanciesReportParametrs.locationIds) ||
         vm.vacanciesReportParametrs.locationIds.includes(user.cityId);
   }

   function isSelectedUsersGroupedByLocationEmpty() {
      return isEmpty(vm.selectedUsersGroupedByLocation);
   }

   function clear() {
      _clearLocationField();
      _clearUserField();
      vm.vacanciesReportParametrs                = {};
      vm.vacanciesReportParametrs.locationIds    = [];
      vm.vacanciesReportParametrs.userIds        = [];
      vm.report.startDateReportGroupedByLocation = {};
      vm.report.endDateReportGroupedByLocation   = {};
      vm.report.reportGroupedByLocation          = {};
      vm.report.startDateReportGroupedByUser     = {};
      vm.report.endDateReportGroupedByUser       = {};
      vm.report.reportGroupedByUser              = {};
   }

   function _reportConditionsValidation() {
      if (!vm.selectedLocations.length && !vm.selectedUsers.length) {
         UserDialogService.notification($translate.instant('DIALOG_SERVICE.EMPTY_REPORT_CONDITIONS'), 'error');
         return false;
      }
      if (vm.vacanciesReportParametrs.startDate > vm.vacanciesReportParametrs.endDate) {
         UserDialogService.notification($translate.instant('DIALOG_SERVICE.INVALID_DATES'), 'error');
         return false;
      }
   }

   function _convertUsersArrayToHash(user) {
      each(vm.selectedLocations, location => {
         if (vm.selectedUsersGroupedByLocation[location.id] && location.id === user.cityId) {
            vm.selectedUsersGroupedByLocation[location.id].push(user);
         } else if (!vm.selectedUsers[location.id] && location.id === user.cityId) {
            (vm.selectedUsersGroupedByLocation[location.id] = []).push(user);
         }
      });
   }
}
