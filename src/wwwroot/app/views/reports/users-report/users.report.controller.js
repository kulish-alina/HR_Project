import {
   set,
   remove,
   each,
   isEmpty,
   reduce,
   flatten,
   map,
   groupBy,
   sumBy,
   find
} from 'lodash';
const LIST_OF_LOCATIONS = ['Dnipropetrovsk', 'Zaporizhia', 'Lviv', 'Berdyansk'];

export default function UsersReportController(
   $scope,
   $translate,
   UserService,
   ThesaurusService,
   ReportsService,
   ValidationService,
   UserDialogService
) {
   'ngInject';

   const vm                                 = $scope;
   vm.users                                 = [];
   vm.usersReportParametrs                  = {};
   vm.usersReportParametrs.locationIds      = [];
   vm.usersReportParametrs.userIds          = [];
   vm.locations                             = [];
   vm.selectedLocations                     = [];
   vm.selectedUsers                         = [];
   vm.selectedUsersGroupedByLocation        = {};
   vm.clear                                 = clear;
   vm.useLocationField                      = useLocationField;
   vm.useUserField                          = useUserField;
   vm.addLocationIdsToUsersReportParametrs  = addLocationIdsToUsersReportParametrs;
   vm.addUserIdsToUsersReportParametrs      = addUserIdsToUsersReportParametrs;
   vm.genereteUsersReport                   = genereteUsersReport;
   vm.isEqualLocations                      = isEqualLocations;
   vm.isSelectedUsersGroupedByLocationEmpty = isSelectedUsersGroupedByLocationEmpty;
   vm.sumReportByStages                     = sumReportByStages;
   vm.filterArrayByProperty                 = filterArrayByProperty;
   vm.addedStageIndex                       = 0;

   (function init() {
      ThesaurusService.getThesaurusTopics('stage').then(topic => set(vm, 'stages', topic));
      ThesaurusService.getThesaurusTopics('city').then(locations => {
         each(locations, (location) => {
            if (LIST_OF_LOCATIONS.includes(location.title)) {
               vm.locations.push(location);
            }
            set(location, 'selected', vm.usersReportParametrs.locationIds.length && vm.usersReportParametrs.locationIds.includes(location.id)); // eslint-disable-line max-len
         });
      });
      UserService.getUsers().then(users => {
         set(vm, 'users', users);
         each(vm.users, (user) => {
            set(user, 'selected', vm.usersReportParametrs.userIds.length && vm.usersReportParametrs.userIds.includes(user.id));        // eslint-disable-line max-len
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

   function addLocationIdsToUsersReportParametrs(location) {
      if (location.selected && !vm.usersReportParametrs.locationIds.includes(location.id)) {
         vm.usersReportParametrs.locationIds.push(location.id);
         vm.selectedLocations.push(location);
      } else if (!location.selected) {
         remove(vm.usersReportParametrs.locationIds, (locationId) =>  locationId === location.id);
         remove(vm.selectedLocations, {id: location.id});
      }
   }

   function addUserIdsToUsersReportParametrs(user) {
      if (user.selected && !vm.usersReportParametrs.userIds.includes(user.id)) {
         vm.usersReportParametrs.userIds.push(user.id);
         if (vm.selectedLocations.length) {
            _convertUsersArrayToHash(user);
         } else {
            vm.selectedUsers.push(user);
         }
      } else if (!user.selected) {
         remove(vm.usersReportParametrs.userIds, (userId) =>  userId === user.id);
         if (vm.selectedLocations.length) {
            remove(vm.selectedUsersGroupedByLocation[user.cityId], {id: user.id});
         } else {
            remove(vm.selectedUsers, {id: user.id});
         }
      }
   }

   function genereteUsersReport(form) {
      _reportConditionsValidation();
      ValidationService.validate(form).then(() => {
         ReportsService.getDataForUserReport(vm.usersReportParametrs).then(resp => {
            vm.usersReportParametrs.startDate = resp.startDate;
            vm.usersReportParametrs.endDate = resp.endDate;
            _convertReport(resp.userReport);
         });
      });
   }

   function sumReportByStages(report, stageId) {
      if (report !== undefined) {
         return sumBy(report, x => x.stagesData[stageId]);
      }
   }

   function filterArrayByProperty(report, prop, stageId) {
      let rep = find(report, {userId: prop});
      return rep && rep.stagesData[stageId] ? rep.stagesData[stageId] : 0;
   }

   function isEqualLocations(user) {
      return isEmpty(vm.usersReportParametrs.locationIds) ?
         true :
         vm.usersReportParametrs.locationIds.includes(user.cityId);
   }

   function isSelectedUsersGroupedByLocationEmpty() {
      return isEmpty(vm.selectedUsersGroupedByLocation);
   }

   function clear() {
      _clearLocationField();
      _clearUserField();
      vm.usersReportParametrs              = {};
      vm.reportGroupedByLocation           = {};
      vm.reportGroupedByUser               = {};
      vm.usersReportParametrs.locationIds  = [];
      vm.usersReportParametrs.userIds      = [];
   }

   function _convertReport(report) {
      if (vm.selectedLocations.length) {
         set(vm, 'reportGroupedByLocation', _convertReportToHash('locationId', report));
      } else {
         set(vm, 'reportGroupedByUser', _convertReportToHash('userId', report));
      }
   }

   function _convertReportToHash(key, value) {
      if (key === 'locationId') {
         return reduce(value, (resultObj, val) => {
            (resultObj[val[key]] || (resultObj[val[key]] = [])).push(...val.usersStatisticsInfo);
            return resultObj;
         }, {});
      } else {
         let arr = flatten(map(value, 'usersStatisticsInfo'));
         return groupBy(arr, 'userId');
      }
   }

   function _reportConditionsValidation() {
      if (!vm.selectedLocations.length && !vm.selectedUsers.length) {
         UserDialogService.notification($translate.instant('DIALOG_SERVICE.EMPTY_REPORT_CONDITIONS'), 'error');
         return false;
      }
      if (vm.usersReportParametrs.startDate > vm.usersReportParametrs.endDate) {
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

   function _clearLocationField() {
      vm.usersReportParametrs.locationIds = [];
      vm.selectedLocations                = [];
      vm.selectedUsersGroupedByLocation   = {};
      each(vm.locations, (location) =>  set(location, 'selected', false));
   }

   function _clearUserField() {
      vm.usersReportParametrs.userIds     = [];
      vm.selectedUsers                    = [];
      vm.selectedUsersGroupedByLocation   = {};
      each(vm.users, (user) => set(user, 'selected', false));
   }

}
