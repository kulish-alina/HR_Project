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
   find,
   filter,
   some,
   assignIn
} from 'lodash';

const STATISTIC_KEYS_FOR_DATE = {
   Pending              : {key: 'Pending',     value: 'pendingVacanciesCount'},
   Open                 : {key: 'Open',        value: 'openVacanciesCount'},
   Progressing          : {key: 'Progressing', value: 'inProgressVacanciesCount'}
};
const STATISTIC_KEYS_FOR_PERIOD = {
   Period_Pending       : {key: 'Period_Pending',     value: 'vacanciesPendingInCurrentPeriodCount'},
   Period_Open          : {key: 'Period_Open',        value: 'vacanciesOpenedInCurrentPeriodCount'},
   Period_Progressing   : {key: 'Period_Progressing', value: 'vacanciesInProgressInCurrentPeriodCount'},
   Period_Closed        : {key: 'Period_Closed',      value: 'vacanciesClosedInCurrentPeriodCount'},
   Period_Canceled      : {key: 'Period_Canceled',    value: 'vacanciesCanceledInCurrentPeriodCount'}
};

export default function VacanciesReportController( // eslint-disable-line max-params, max-statements
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
   vm.toggleLocations                          = toggleLocations;
   vm.toggleUsers                              = toggleUsers;
   vm.generateVacanciesReport                  = generateVacanciesReport;
   vm.isUserLocationSelected                   = isUserLocationSelected;
   vm.isSelectedUsersGroupedByLocationEmpty    = isSelectedUsersGroupedByLocationEmpty;
   vm.report                                   = {};
   vm.sumReportByStages                        = sumReportByStages;
   vm.filterArrayByProperty                    = filterArrayByProperty;

   const REPORT_TITLES = [
      $translate.instant('REPORTS.START_DATE_CHART'),
      $translate.instant('REPORTS.PERIOD_DATE_CHART'),
      $translate.instant('REPORTS.END_DATE_CHART')
   ];

   (function init() {
      ThesaurusService.getThesaurusTopics('stage').then(topic => set(vm, 'stages', topic));
      ThesaurusService.getOfficeLocations()
         .then(locations => {
            set(vm, 'locations', locations);
            each(locations, location =>
               set(location, 'selected', vm.vacanciesReportParametrs.locationIds.includes(location.id))
            );
            return vm.locations;
         })
         .then(locations => {
            UserService.getUsers().then(users => {
               set(vm, 'users', filter(users, user => some(locations, {id : user.cityId})));
               each(vm.users, user => set(user, 'selected', vm.vacanciesReportParametrs.userIds.includes(user.id)));
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
      each(vm.selectedUsersGroupedByLocation, (locationGroup) => {
         each(locationGroup, (user) =>  set(user, 'selected', false));
      });
      vm.selectedUsersGroupedByLocation = {};
      each(vm.locations, (location) =>  set(location, 'selected', false));
   }

   function _clearUserField() {
      vm.vacanciesReportParametrs.userIds = [];
      vm.selectedUsers = [];
      vm.selectedUsersGroupedByLocation = {};
      each(vm.users, (user) => set(user, 'selected', false));
   }

   function toggleLocations(location) {
      if (location.selected && !vm.vacanciesReportParametrs.locationIds.includes(location.id)) {
         vm.vacanciesReportParametrs.locationIds.push(location.id);
         vm.selectedLocations.push(location);
      } else if (!location.selected) {
         remove(vm.vacanciesReportParametrs.locationIds, (locationId) =>  locationId === location.id);
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
      vm.vacanciesReportParametrs.startDate = vm.startDate;
      vm.vacanciesReportParametrs.endDate = vm.endDate;
      let validateObj = _reportConditionsValidation();
      if (validateObj.isValid) {
         ValidationService.validate(form).then(() => {
            ReportsService.getDataForVacancyReport(vm.vacanciesReportParametrs)
               .then(resp => {
                  _convertReport(resp);
                  vm.$broadcast('onGenerateReport', {
                     value: {
                        startReportValue: vm.selectedLocations.length ?
                           vm.startDateReportGroupedByLocationForHistogram :
                           vm.startDateReportGroupedByUserForHistogram,
                        periodReportValue: vm.selectedLocations.length ?
                           vm.reportGroupedByLocationForHistogram :
                           vm.reportGroupedByUserForHistogram,
                        endReportValue: vm.selectedLocations.length ?
                           vm.endDateReportGroupedByLocationForHistogram :
                           vm.endDateReportGroupedByUserForHistogram
                     },
                     titles: REPORT_TITLES
                  });
               });
         });
      } else {
         UserDialogService.notification(validateObj.errorMessage, 'error');
      }
   }

   function _convertReport(report) {
      if (vm.selectedLocations.length) {
         set(vm.report, 'startDateReportGroupedByLocation', _convertToHash('locationId', report.startDateReport));
         set(vm.report, 'endDateReportGroupedByLocation', _convertToHash('locationId', report.endDateReport));
         set(vm.report, 'reportGroupedByLocation', _convertToHash('locationId', report.vacanciesReport));
         set(vm, 'startDateReportGroupedByLocationForHistogram', _converReportForHistogram('locationId', 'forDate',
            report.startDateReport));
         set(vm, 'endDateReportGroupedByLocationForHistogram', _converReportForHistogram('locationId', 'forDate',
            report.endDateReport));
         set(vm, 'reportGroupedByLocationForHistogram', _converReportForHistogram('locationId', 'forPeriod',
            report.vacanciesReport));
      } else {
         set(vm.report, 'startDateReportGroupedByUser', _convertToHash('userId', report.startDateReport));
         set(vm.report, 'endDateReportGroupedByUser', _convertToHash('userId', report.endDateReport));
         set(vm.report, 'reportGroupedByUser', _convertToHash('userId', report.vacanciesReport));
         set(vm, 'startDateReportGroupedByUserForHistogram', _converReportForHistogram('userId', 'forDate',
            report.startDateReport));
         set(vm, 'endDateReportGroupedByUserForHistogram', _converReportForHistogram('userId', 'forDate',
            report.endDateReport));
         set(vm, 'reportGroupedByUserForHistogram', _converReportForHistogram('userId', 'forPeriod',
            report.vacanciesReport));
      }
   }

   function _converReportForHistogram(key, type, value) {
      if (key === 'locationId') {
         return map(value, (locationObj) => {
            let sumPropertiesObj = _getSumPropertiesObject(locationObj, type);
            let statKeysArr = assignIn(STATISTIC_KEYS_FOR_DATE, STATISTIC_KEYS_FOR_PERIOD);
            let statisticInfoObjectsArray = map(locationObj.vacanciesStatisticsInfo, (statisticInfo) => {
               return map(sumPropertiesObj, (prop, statKey) => {
                  return {
                     number: statisticInfo[statKeysArr[statKey].value],
                     name: statisticInfo.userDisplayName,
                     key: statKey
                  };
               });
            });
            let groupedArray = groupBy(flatten(statisticInfoObjectsArray), (element) => {
               return element.key;
            });
            let groupedStatisticInfoObjectsArray = map(groupedArray, (val, k, array) => {
               return {
                  name: k,
                  value: sumPropertiesObj[k],
                  users: array[k]
               };
            });
            let resultObj = {
               state: find(vm.locations, {id: locationObj.locationId}).title,
               values: groupedStatisticInfoObjectsArray
            };
            return resultObj;
         });
      } else {
         return reduce(value, (resultArray, locationObj) => {
            let convertedUserObject = map(locationObj.vacanciesStatisticsInfo, (userObject) => {
               let userStatArray = type === 'forDate' ?
                  map(STATISTIC_KEYS_FOR_DATE, (x) => {
                     return {
                        name: x.key,
                        value: userObject[x.value]
                     };
                  }) :
                 map(STATISTIC_KEYS_FOR_PERIOD, (x) => {
                    return {
                       name: x.key,
                       value: userObject[x.value]
                    };
                 });
               return {
                  state: userObject.userDisplayName,
                  values: userStatArray
               };
            });
            resultArray.push(convertedUserObject);
            return flatten(resultArray);
         }, []);
      }
   }

   function _getSumPropertiesObject(value, type) {
      return type === 'forDate' ?
         {
            Pending:     sumBy(value.vacanciesStatisticsInfo,
                         (statisticInfoForUser) => statisticInfoForUser.pendingVacanciesCount),
            Open:        sumBy(value.vacanciesStatisticsInfo, (statisticInfoForUser) =>
                         statisticInfoForUser.openVacanciesCount),
            Progressing: sumBy(value.vacanciesStatisticsInfo,
                         (statisticInfoForUser) => statisticInfoForUser.inProgressVacanciesCount)
         } :
         {
            Period_Pending:     sumBy(value.vacanciesStatisticsInfo, (statisticInfoForUser) =>
                                statisticInfoForUser.vacanciesPendingInCurrentPeriodCount),
            Period_Open:        sumBy(value.vacanciesStatisticsInfo, (statisticInfoForUser) =>
                                statisticInfoForUser.vacanciesOpenedInCurrentPeriodCount),
            Period_Progressing: sumBy(value.vacanciesStatisticsInfo, (statisticInfoForUser) =>
                                statisticInfoForUser.vacanciesInProgressInCurrentPeriodCount),
            Period_Closed:      sumBy(value.vacanciesStatisticsInfo, (statisticInfoForUser) =>
                                statisticInfoForUser.vacanciesClosedInCurrentPeriodCount),
            Period_Canceled:    sumBy(value.vacanciesStatisticsInfo, (statisticInfoForUser) =>
                                statisticInfoForUser.vacanciesCanceledInCurrentPeriodCount)
         };
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
      vm.startDate = null;
      vm.endDate = null;
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
      vm.$broadcast('onClear');
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
}
