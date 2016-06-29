import {
   clone,
   set
} from 'lodash';
import template from './events.directive.html';
import './events.scss';
export default class EventsDirective {
   constructor() {
      this.restrict   = 'E';
      this.template   = template;
      this.scope      = {
         type           : '@',
         events         : '=',
         save           : '=',
         remove         : '=',
         getEventsByDate: '='
      };
      this.controller = EventsController;
   }
   static createInstance() {
      'ngInject';
      EventsDirective.instance = new EventsDirective();
      return EventsDirective.instance;
   }
}

function EventsController($scope, $translate, $timeout, VacancyService, CandidateService, UserService,
                           ThesaurusService, UserDialogService) {
   const vm               = $scope;
   vm.event               = {};
   vm.vacancies           = [];
   vm.candidates          = [];
   vm.responsibles        = [];
   vm.eventTypes          = [];
   vm.saveEvent           = saveEvent;
   vm.showAddEventDialog  = showAddEventDialog;
   vm.showEditEventDialog = showEditEventDialog;
   vm.vacancy             = {};
   vm.vacancy.current     = 0;
   vm.vacancy.size        = 20;
   vm.getEvents           = getEvents;
   vm.currentEvents       = [];

   function _init() {
      VacancyService.search(vm.vacancy).then(data => set(vm, 'vacancies', data.vacancies));
      UserService.getUsers().then(users => set(vm, 'responsibles', users));
      CandidateService.getCandidates().then(candidates => set(vm, 'candidates', candidates));
      ThesaurusService.getThesaurusTopics('eventtype').then(eventTypes => set(vm, 'eventTypes', eventTypes));
   }

   _init();

   function saveEvent() {
      vm.save(vm.event);
   }

   function getEvents(date) {
      vm.getEventsByDate(date).then((e) => {
         vm.currentEvents.length = 0;
         vm.currentEvents.push.apply(vm.currentEvents, e);
      });
   }

   function showAddEventDialog() {
      vm.event = {};
      let scope = {
         type         : 'list-with-input',
         responsibles : vm.responsibles,
         eventTypes   : vm.eventTypes,
         vacancies    : vm.vacancies,
         candidates   : vm.candidates,
         events       : vm.currentEvents,
         event        : vm.event,
         getEvents    : vm.getEvents
      };
      let buttons = [
         {
            name: $translate.instant('COMMON.CANCEL')
         },
         {
            name: $translate.instant('COMMON.APLY'),
            func: vm.saveEvent,
            needValidate: true
         }
      ];
      UserDialogService.dialog($translate.instant('COMMON.EVENTS'), template, buttons, scope);
      let initializing = true;

      $scope.$watch('event.eventDate', function watch() {
         if (initializing) {
            $timeout(function timeout() {
               initializing = false;
            });
         } else {
            getEvents(vm.event.eventDate);
         }
      });
   }

   function showEditEventDialog(currentEvent) {
      vm.event = clone(currentEvent);
      let scope = {
         type         : 'form-only',
         responsibles : vm.responsibles,
         eventTypes   : vm.eventTypes,
         vacancies    : vm.vacancies,
         candidates   : vm.candidates,
         event        : vm.event
      };
      let buttons = [
         {
            name: $translate.instant('COMMON.CANCEL')
         },
         {
            name: $translate.instant('COMMON.APLY'),
            func: vm.saveEvent,
            needValidate: true
         }
      ];
      UserDialogService.dialog($translate.instant('COMMON.EVENTS'), template, buttons, scope);
   }

}
