import {
   set,
   each,
   assign,
   remove,
   isEmpty,
   reduce,
   flatten,
   map
} from 'lodash';

const LIST_OF_LOCATIONS = ['Dnipropetrovsk', 'Zaporizhia', 'Lviv', 'Berdyansk'];

export default function VacanciesReportController(
   $scope,
   UserService,
   ThesaurusService,
   ReportsService
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
   vm.formingVacanciesReport                   = formingVacanciesReport;
   vm.isEqualLocations                         = isEqualLocations;
   vm.isSelectedUsersGroupedByLocationEmpty    = isSelectedUsersGroupedByLocationEmpty;


   (function init() {
      ThesaurusService.getThesaurusTopics('stage').then(topic => set(vm, 'stages', topic));
      ThesaurusService.getThesaurusTopics('city').then(locations => {
         each(locations, (location) => {
            if (LIST_OF_LOCATIONS.includes(location.title)) {
               vm.locations.push(location);
            }
            if (vm.vacanciesReportParametrs.locationIds.length &&
                vm.vacanciesReportParametrs.locationIds.includes(location.id)) {
               assign(location, {selected: true});
            } else {
               assign(location, {selected: false});
            }
         });
      });
      UserService.getUsers().then(users => {
         set(vm, 'users', users);
         each(vm.users, (user) => {
            if (vm.vacanciesReportParametrs.userIds.length &&
                vm.vacanciesReportParametrs.userIds.includes(user.id)) {
               assign(user, {selected: true});
            } else {
               assign(user, {selected: false});
            }
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
      each(vm.locations, (location) => {
         location.selected = false;
      });
   }

   function _clearUserField() {
      vm.vacanciesReportParametrs.userIds = [];
      vm.selectedUsers = [];
      vm.selectedUsersGroupedByLocation = {};
      each(vm.users, (user) => {
         user.selected = false;
      });
   }

   function addLocationIdsToVacanciesReportParametrs(location) {
      if (location.selected && !vm.vacanciesReportParametrs.locationIds.includes(location.id)) {
         vm.vacanciesReportParametrs.locationIds.push(location.id);
         vm.selectedLocations.push(location);
      } else if (!location.selected) {
         remove(vm.vacanciesReportParametrs.locationIds, (locationId) =>  locationId === location.id);
         remove(vm.selectedLocations, (loc) =>  loc.id === location.id);
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

   function formingVacanciesReport() {
      ReportsService.getDataForVacancyReport(vm.vacanciesReportParametrs).then(resp => {
         _convertReportToHash(resp);
      });
   }

   function _convertReportToHash(report) {
      if (vm.selectedLocations.length && !vm.selectedUsers.length) {
         set(vm, 'startDateReportGroupedByLocation', _convertToHash('locationId', report.startDateReport));
         set(vm, 'endDateReportGroupedByLocation', _convertToHash('locationId', report.endDateReport));
         set(vm, 'reportGroupedByLocation', _convertToHash('locationId', report.vacanciesReport));
      } else if (!vm.selectedLocations.length && vm.selectedUsers.length) {
         set(vm, 'startDateReportGroupedByUser', _convertToHash('userId', report.startDateReport));
         set(vm, 'endDateReportGroupedByUser', _convertToHash('userId', report.endDateReport));
         set(vm, 'reportGroupedByUser', _convertToHash('userId', report.vacanciesReport));
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
            if (val.dailyVacanciesStatisticsInfo) {
               return val.dailyVacanciesStatisticsInfo;
            } else {
               return val.vacanciesStatisticsInfo;
            }
         }));
      }
   }

   function clear() {
      vm.vacanciesReportParametrs = {};
      _clearLocationField();
      _clearUserField();
      vm.startDateReportGroupedByLocation = {};
      vm.endDateReportGroupedByLocation = {};
      vm.reportGroupedByLocation = {};
   }

   function isEqualLocations(user) {
      if (vm.vacanciesReportParametrs.locationIds.length) {
         return vm.vacanciesReportParametrs.locationIds.includes(user.cityId);
      } else {
         return true;
      }
   }

   function isSelectedUsersGroupedByLocationEmpty() {
      return isEmpty(vm.selectedUsersGroupedByLocation);
   }

   function _convertUsersArrayToHash(user) {
      each(vm.selectedLocations, location => {
         if (vm.selectedUsersGroupedByLocation[location.id] && location.id === user.cityId) {
            vm.selectedUsersGroupedByLocation[location.id].push(user);
         } else if (!vm.selectedUsers[location.id] && location.id === user.cityId) {
            vm.selectedUsersGroupedByLocation[location.id] = [];
            vm.selectedUsersGroupedByLocation[location.id].push(user);
         }
      });
   }
}
