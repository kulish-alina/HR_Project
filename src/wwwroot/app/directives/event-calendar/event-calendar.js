import {
   each,
   split,
   remove,
   difference,
   clone,
   mapKeys
} from 'lodash';
import template from './event-calendar.directive.html';
import './event-canlendar.scss';
export default class EventCalendarDirective {
   constructor() {
      this.restrict = 'E';
      this.template = template;
      this.controller = EventCalendarController;
      this.scope      = {
         getEvents       : '=',
         users           : '=',
         removeEvent     : '=',
         saveEvent       : '=',
         vacancies       : '=',
         candidates      : '=',
         eventTypes      : '=',
         getEventsByDate : '=',
         userColor       : '='
      };
   }
   static createInstance() {
      'ngInject';
      EventCalendarDirective.instance = new EventCalendarDirective();
      return EventCalendarDirective.instance;
   }
}

function EventCalendarController($scope) {
   let vm                       = $scope;
   vm.showAddDialog             = showAddDialog;
   vm.eventsForMonth            = {};
   vm.todayDate                 = new Date();
   vm.selectedDay               = new Date();
   vm.dateForSearchEvents       = new Date();
   vm.currentMonth              = vm.dateForSearchEvents.getMonth();
   vm.currentYear               = vm.dateForSearchEvents.getFullYear();
   vm.goToNextMonth             = goToNextMonth;
   vm.goToPreviousMonth         = goToPreviousMonth;
   vm.goToNextYear              = goToNextYear;
   vm.goToPreviousYear          = goToPreviousYear;
   vm.removeCurrentEvent        = removeCurrentEvent;
   vm.saveCurrentEvent          = saveCurrentEvent;
   vm.onDblclick                = showAddDialog;
   vm.checkedUsers              = [];
   vm.eventsForDay              = [];
   vm.getEventsForDate          = getEventsForDate;
   vm.getDailyEvents            = getDailyEvents;
   vm.numberEventsForDailyView  = 3;
   vm.lengthOfWeek              = 7;
   vm.daysOfWeek                = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];

   (function init() {
      formatingSearchCondition();
      getDaysInMonth(vm.startDate, vm.endDate);
      vm.getEvents(vm.startDate, vm.endDate, vm.checkedUsers).then(convertEventsToHash);
   }());

   function showAddDialog(day) {
      vm.$broadcast('onDblclick', {date: day});
   }

   function formatingSearchCondition() {
      vm.startDate = new Date(vm.currentYear, vm.currentMonth, 1);
      vm.firstDayOfWeek = vm.startDate.getDay();
      if (vm.firstDayOfWeek === 0) {
         vm.startDate.setDate(vm.startDate.getDate() - (vm.lengthOfWeek - 1));
      } else {
         vm.startDate.setDate(vm.startDate.getDate() - (vm.firstDayOfWeek - 1));
      }
      vm.endDate = new Date(vm.dateForSearchEvents.getFullYear(), vm.dateForSearchEvents.getMonth() + 1, 0);
      vm.lastDayOfWeek = vm.endDate.getDay();
      if (vm.lastDayOfWeek !== 0) {
         vm.endDate.setDate(vm.endDate.getDate() + (vm.lengthOfWeek - vm.lastDayOfWeek));
      }
   }

   vm.$on('checkUser', function fromParent(event, obj) {
      let diff = [];
      let clonedCheckedUsersIds = clone(obj.checkedUsersIds);
      if (vm.checkedUsers.length > obj.checkedUsersIds.length) {
         let unCheckedUserId = difference(vm.checkedUsers, obj.checkedUsersIds);
         mapKeys(vm.eventsForMonth, value => {
            remove(value, {responsibleId: unCheckedUserId[0].toString()});
         });
         remove(vm.eventsForMonth, {responsibleId: unCheckedUserId[0].toString()});
      } else {
         diff = difference(obj.checkedUsersIds, vm.checkedUsers);
         formatingSearchCondition();
         vm.getEvents(vm.startDate, vm.endDate, diff).then((events) => {
            convertEventsToHash(events);
            getEventsForDate();
         });
      }
      vm.checkedUsers = clonedCheckedUsersIds;
   });

   function getDaysInMonth(startDt, endDt) {
      vm.daysInMonth = [];
      vm.currentDate = new Date(startDt);
      let end = new Date(endDt);
      while (vm.currentDate <= end) {
         vm.daysInMonth.push(new Date(vm.currentDate));
         vm.currentDate.setDate(vm.currentDate.getDate() + 1);
      }
   }

   function getEventsForDate(day) {
      vm.selectedDay = day ? day : vm.selectedDay;
      vm.eventsForDay = vm.eventsForMonth[`${vm.selectedDay.getDate()},${vm.selectedDay.getMonth() + 1},${vm.selectedDay.getFullYear()}`]; // eslint-disable-line max-len
   }

   function getDailyEvents(day) {
      return vm.eventsForMonth[`${day.getDate()},${day.getMonth() + 1},${day.getFullYear()}`];
   }

   function goToNextMonth() {
      if (vm.currentMonth < 11) {
         vm.currentMonth = vm.currentMonth + 1;
      } else if (vm.currentMonth === 11) {
         vm.currentMonth = 0;
         vm.currentYear = vm.currentYear + 1;
      }
      formattingDataToCalendarView();
   }

   function goToPreviousMonth() {
      if (vm.currentMonth > 0) {
         vm.currentMonth = vm.currentMonth - 1;
      } else {
         vm.currentYear = vm.currentYear - 1;
         vm.currentMonth = 11;
      }
      formattingDataToCalendarView();
   }

   function goToNextYear() {
      vm.currentYear = vm.currentYear + 1;
      formattingDataToCalendarView();
   }

   function goToPreviousYear() {
      vm.currentYear = vm.currentYear - 1;
      formattingDataToCalendarView();
   }

   function formattingDataToCalendarView() {
      vm.daysInMonth = [];
      vm.eventsForMonth  = {};
      vm.dateForSearchEvents = new Date(vm.currentYear, vm.currentMonth);
      formatingSearchCondition();
      getDaysInMonth(vm.startDate, vm.endDate);
      vm.getEvents(vm.startDate, vm.endDate, vm.checkedUsers).then(convertEventsToHash);
      getEventsForDate(new Date(vm.currentYear, vm.currentMonth, 1));
   }


   function convertEventsToHash(events) {
      each(events, (event) => {
         let eventDate = convertDate(event);
         if (vm.eventsForMonth[eventDate]) {
            vm.eventsForMonth[eventDate].push(event);
         } else {
            vm.eventsForMonth[eventDate] = [];
            vm.eventsForMonth[eventDate].push(event);
         }
      });
   }

   function convertDate(event) {
      let splitedDate = split(split(event.eventDate,  ' ')[0], '-');
      let convertedSplitedDate = [];
      each(splitedDate, (part) => {
         convertedSplitedDate.push(parseInt(part));
      });
      return convertedSplitedDate.toString();
   }

   function removeCurrentEvent(event) {
      let eventDate = convertDate(event);
      vm.removeEvent(event).then(() => {
         remove(vm.eventsForMonth[eventDate], {id: event.id});
      });
   }

   function saveCurrentEvent(event) {
      let eventDate = convertDate(event);
      if (event.id) {
         remove(vm.eventsForMonth, {id: event.id});
      }
      return vm.saveEvent(event).then((responseEvent) => {
         if (vm.eventsForMonth[eventDate]) {
            vm.eventsForMonth[eventDate].push(responseEvent);
         } else {
            vm.eventsForMonth[eventDate] = [];
            vm.eventsForMonth[eventDate].push(responseEvent);
         }
         return responseEvent;
      });
   }
}
