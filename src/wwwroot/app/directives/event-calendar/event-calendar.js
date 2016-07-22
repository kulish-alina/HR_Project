import {
   each,
   split,
   remove
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
         getEventsByDate : '='
      };
   }
   static createInstance() {
      'ngInject';
      EventCalendarDirective.instance = new EventCalendarDirective();
      return EventCalendarDirective.instance;
   }
}

function EventCalendarController($scope) {
   let vm                 = $scope;
   vm.onDblclick          = showAddDialog;
   vm.eventsForMonth      = {};
   vm.todayDate           = new Date();
   vm.dateForSearchEvents = new Date();
   vm.currentMonth        = vm.dateForSearchEvents.getMonth();
   vm.currentYear         = vm.dateForSearchEvents.getFullYear();
   vm.goToNextMonth       = goToNextMonth;
   vm.goToPreviousMonth   = goToPreviousMonth;
   vm.removeCurrentEvent  = removeCurrentEvent;
   vm.saveCurrentEvent    = saveCurrentEvent;
   vm.onDblclick          = showAddDialog;
   vm.checkedUsers        = [];

   function init() {
      getDaysInMonth(vm.currentMonth, vm.currentYear);
      vm.getEvents(new Date(), vm.chekedUsers).then((events) => {
         convertEventsToHash(events);
      });
   }
   init();

   function showAddDialog(day) {
      vm.$broadcast('onDblclick', {date: day});
   }

   vm.$on('checkUser', function fromParent(event, obj) {
      vm.checkedUsers = obj.chekedUsers;
      vm.eventsForMonth = {};
      vm.getEvents(vm.dateForSearchEvents, vm.chekedUsers).then((events) => {
         convertEventsToHash(events);
      });
   });

   function getDaysInMonth(month, year) {
      vm.daysInMonth = [];
      let date = new Date(year, month, 1);
      while (date.getMonth() === month) {
         vm.daysInMonth.push(new Date(date));
         date.setDate(date.getDate() + 1);
      }
   }

   function goToNextMonth() {
      if (vm.currentMonth < 11) {
         vm.currentMonth = vm.currentMonth + 1;
      } else if (vm.currentMonth === 11) {
         vm.currentMonth = 0;
         vm.currentYear = vm.currentYear + 1;
      }
      vm.daysInMonth = [];
      vm.eventsForMonth      = {};
      vm.dateForSearchEvents = new Date(vm.currentYear, vm.currentMonth);
      getDaysInMonth(vm.currentMonth, vm.currentYear);
      vm.getEvents(new Date(vm.currentYear, vm.currentMonth, 1), vm.checkedUsers).then((events) => {
         convertEventsToHash(events);
      });
   }

   function goToPreviousMonth() {
      if (vm.currentMonth > 0) {
         vm.currentMonth = vm.currentMonth - 1;
      } else {
         vm.currentYear = vm.currentYear - 1;
         vm.currentMonth = 11;
      }
      vm.daysInMonth = [];
      vm.eventsForMonth  = {};
      vm.dateForSearchEvents = new Date(vm.currentYear, vm.currentMonth);
      getDaysInMonth(vm.currentMonth, vm.currentYear);
      vm.getEvents(new Date(vm.currentYear, vm.currentMonth, 1), vm.checkedUsers).then((events) => {
         convertEventsToHash(events);
      });
   }

   function convertEventsToHash(events) {
      each(events, (event) => {
         let eventDate = split(split(event.eventDate,  ' ')[0], '-');
         if (vm.eventsForMonth[eventDate[0]]) {
            vm.eventsForMonth[eventDate[0]].push(event);
         } else {
            vm.eventsForMonth[eventDate[0]] = [];
            vm.eventsForMonth[eventDate[0]].push(event);
         }
      });
   }

   function removeCurrentEvent(event) {
      let eventDate = split(split(event.eventDate,  ' ')[0], '-');
      vm.removeEvent(event).then(() => {
         remove(vm.eventsForMonth[eventDate[0]], {id: event.id});
      });
   }

   function saveCurrentEvent(event) {
      let eventDate = split(split(event.eventDate,  ' ')[0], '-');
      if (event.id) {
         remove(vm.eventsForMonth, {id: event.id});
      }
      return vm.saveEvent(event).then((responseEvent) => {
         if (vm.eventsForMonth[eventDate[0]]) {
            vm.eventsForMonth[eventDate[0]].push(responseEvent);
         } else {
            vm.eventsForMonth[eventDate[0]] = [];
            vm.eventsForMonth[eventDate[0]].push(responseEvent);
         }
         return responseEvent;
      });
   }
}
