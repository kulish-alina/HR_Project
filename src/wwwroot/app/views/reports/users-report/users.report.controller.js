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
   find,
   toNumber,
   sortBy,
   filter,
   some
} from 'lodash';

export default function UsersReportController( // eslint-disable-line max-params, max-statements
   $scope,
   $translate,
   UserService,
   ThesaurusService,
   ReportsService,
   ValidationService,
   UserDialogService
) {
   'ngInject';

   const vm                                      = $scope;
   vm.users                                      = [];
   vm.usersReportParametrs                       = {};
   vm.usersReportParametrs.locationIds           = [];
   vm.usersReportParametrs.userIds               = [];
   vm.locations                                  = [];
   vm.selectedLocations                          = [];
   vm.selectedUsers                              = [];
   vm.selectedUsersGroupedByLocation             = {};
   vm.clear                                      = clear;
   vm.useLocationField                           = useLocationField;
   vm.useUserField                               = useUserField;
   vm.toggleLocations                            = toggleLocations;
   vm.toggleUsers                                = toggleUsers;
   vm.generateUsersReport                        = generateUsersReport;
   vm.isUserLocationSelected                     = isUserLocationSelected;
   vm.isSelectedUsersGroupedByLocationEmpty      = isSelectedUsersGroupedByLocationEmpty;
   vm.sumReportByStages                          = sumReportByStages;
   vm.filterArrayByProperty                      = filterArrayByProperty;
   vm.addedStageIndex                            = 0;

   (function init() {
      ThesaurusService.getThesaurusTopics('stage').then(topic => {
         set(vm, 'stages', topic);
      });
      ThesaurusService.getOfficeLocations()
         .then(locations => {
            set(vm, 'locations', locations);
            each(locations, location =>
               set(location, 'selected', vm.usersReportParametrs.locationIds.includes(location.id))
            );
            return vm.locations;
         })
         .then(locations => {
            UserService.getUsers().then(users => {
               set(vm, 'users', filter(users, user => some(locations, {id : user.cityId})));
               each(vm.users, user => set(user, 'selected', vm.usersReportParametrs.userIds.includes(user.id)));
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

   function toggleLocations(location) {
      if (location.selected && !vm.usersReportParametrs.locationIds.includes(location.id)) {
         vm.usersReportParametrs.locationIds.push(location.id);
         vm.selectedLocations.push(location);
      } else if (!location.selected) {
         remove(vm.usersReportParametrs.locationIds, (locationId) =>  locationId === location.id);
         remove(vm.selectedLocations, {id: location.id});
         if (vm.selectedUsersGroupedByLocation[location.id]) {
            each(vm.selectedUsersGroupedByLocation[location.id], (user) =>  {
               set(user, 'selected', false);
               remove(vm.usersReportParametrs.userIds, (userId) =>  userId === user.id);
               remove(vm.selectedUsers, {id: user.id});
               vm.selectedUsersGroupedByLocation[location.id] = [];
            });
         }
      }
   }

   function toggleUsers(user) {
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

   function generateUsersReport(form) {
      vm.usersReportParametrs.startDate = vm.startDate;
      vm.usersReportParametrs.endDate = vm.endDate;
      let validateObj = _reportConditionsValidation();
      if (validateObj.isValid) {
         ValidationService.validate(form).then(() => {
            ReportsService.getDataForUserReport(vm.usersReportParametrs).then(resp => {
               _convertReport(resp.userReport);
               vm.$broadcast('onGenerateReport', {value: vm.selectedLocations.length ?
                                                  vm.reportGroupedByLocationForHistogram :
                                                  vm.reportGroupedByUserForHistogram});
            });
         });
      } else {
         UserDialogService.notification(validateObj.errorMessage, 'error');
      }
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

   function isUserLocationSelected(user) {
      return isEmpty(vm.usersReportParametrs.locationIds) ||
         vm.usersReportParametrs.locationIds.includes(user.cityId);
   }

   function isSelectedUsersGroupedByLocationEmpty() {
      return isEmpty(vm.selectedUsersGroupedByLocation);
   }

   function clear() {
      vm.startDate = null;
      vm.endDate = null;
      _clearLocationField();
      _clearUserField();
      vm.usersReportParametrs                = {};
      vm.reportGroupedByLocation             = {};
      vm.reportGroupedByUser                 = {};
      vm.usersReportParametrs.locationIds    = [];
      vm.usersReportParametrs.userIds        = [];
      delete(vm, 'reportGroupedByLocationForHistogram');
      delete(vm, 'reportGroupedByUserForHistogram');
      //remove report chart on clean report parametrs
      vm.$broadcast('onClear');
   }

   function _convertReport(report) {
      if (vm.selectedLocations.length) {
         set(vm, 'reportGroupedByLocation', _convertReportToHash('locationId', report));
         set(vm, 'reportGroupedByLocationForHistogram', _converReportForHistogram('locationId',
            report));
      } else {
         set(vm, 'reportGroupedByUser', _convertReportToHash('userId', report));
         set(vm, 'reportGroupedByUserForHistogram', _converReportForHistogram('userId',
            report));
      }
   }

   function _converReportForHistogram(key, value) {
      let userGroupedByStagesObj = groupBy(_flattenResponseObgect(value), 'stageTitle');
      if (key === 'locationId') {
         return map(userGroupedByStagesObj, (stageGroup, stageTitle) => {
            let groupedByLocationObj = groupBy(stageGroup, 'location');
            let mappedArr = map(groupedByLocationObj, (locationGroup, locationTitle) => {
               return {
                  name: locationTitle,
                  value: sumBy(locationGroup, (userValue) => {
                     return userValue.number;
                  }),
                  users: sortBy(locationGroup, [ 'number' ])
               };
            });
            let resultHistogramObgect = {
               state: stageTitle,
               values: mappedArr
            };
            return resultHistogramObgect;
         });
      } else {
         return map(userGroupedByStagesObj, (stageGroup, stageTitle) => {
            let userObjArray = map(stageGroup, (groupElement) => {
               return {
                  name: groupElement.name,
                  value: groupElement.number
               };
            });
            let resultHistogramObgect = {
               state: stageTitle,
               values: sortBy(userObjArray, [ 'value' ])
            };
            return resultHistogramObgect;
         });
      }
   }

   function _flattenResponseObgect(value) {
      return reduce(value, (resultArray, val) => {
         let usersArr = map(val.usersStatisticsInfo, (entity) => {
            let groupedArr = map(entity.stagesData, (stageEntity, k) => {
               let stage = k === '0' ? 'Added' : find(vm.stages, {id: toNumber(k)}).title;
               return {
                  number: stageEntity,
                  name: entity.userDisplayName,
                  stageTitle: stage,
                  location: find(vm.locations, {id: val.locationId}).title
               };
            });
            return groupedArr;
         });
         resultArray.push(flatten(usersArr));
         return flatten(resultArray);
      }, []);
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
         return {
            isValid: false,
            errorMessage: $translate.instant('DIALOG_SERVICE.EMPTY_REPORT_CONDITIONS')
         };
      }
      return {
         isValid: true
      };
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
      each(vm.locations, (location) =>  set(location, 'selected', false));
      each(vm.selectedUsersGroupedByLocation, (locationGroup) => {
         each(locationGroup, (user) =>  set(user, 'selected', false));
      });
      vm.selectedUsersGroupedByLocation   = {};
   }

   function _clearUserField() {
      vm.usersReportParametrs.userIds     = [];
      vm.selectedUsers                    = [];
      vm.selectedUsersGroupedByLocation   = {};
      each(vm.users, (user) => set(user, 'selected', false));
   }

}
