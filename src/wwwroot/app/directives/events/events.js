import {
   clone,
   split,
   isEqual
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
         getEventsByDate: '=',
         candidateId    : '=',
         userId         : '=',
         source         : '@'
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
   'ngInject';
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
   vm.candidate           = {};
   vm.candidate.current   = 0;
   vm.candidate.size      = 20;
   vm.getEvents           = getEvents;
   vm.currentEvents       = [];

   function _init() {
      VacancyService.search(vm.vacancy).then((data) => vm.vacancies.push.apply(vm.vacancies, data.vacancies));
      UserService.getUsers().then((users) => vm.responsibles.push.apply(vm.responsibles, users));
      CandidateService.search(vm.candidate).then((data) => vm.candidates.push.apply(vm.candidates, data.candidate));
      ThesaurusService.getThesaurusTopics('eventtype')
         .then((eventTypes) => vm.eventTypes.push.apply(vm.eventTypes, eventTypes));
   }
   _init();

   function saveEvent() {
      vm.save(vm.event);
   }

   function getEvents(date) {
      if (date && vm.source === 'user') {
         vm.getEventsByDate(date).then((e) => {
            vm.currentEvents.length = 0;
            vm.currentEvents.push.apply(vm.currentEvents, e);
         });
      } else {
         vm.currentEvents.length = 0;
      }
   }

   function showAddEventDialog() {
      vm.event = {};
      vm.currentEvents.length = 0;
      if (vm.userId) {
         vm.event.responsibleId = `${vm.userId}`;
      }
      if (vm.candidateId) {
         vm.event.candidateId = `${vm.candidateId}`;
      }
      let scope = {
         type         : 'list-with-input',
         responsibles : vm.responsibles,
         eventTypes   : vm.eventTypes,
         vacancies    : vm.vacancies,
         candidates   : vm.candidates,
         events       : vm.currentEvents,
         event        : vm.event,
         getEvents    : vm.getEvents,
         source       : vm.source
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

      let eventDate = '';

      $scope.$watch('event.eventDate', function watch() {
         let clonedTrimedEventDate = split(eventDate, ' ');
         let newEventDate = split(vm.event.eventDate, ' ');
         if (!isEqual(newEventDate[0], clonedTrimedEventDate[0])) {
            getEvents(vm.event.eventDate);
         }
         eventDate = vm.event.eventDate;
      });
   }

   function showEditEventDialog(currentEvent) {
      vm.event = clone(currentEvent);
      if (vm.userId) {
         vm.event.responsibleId = `${vm.userId}`;
      }
      if (vm.candidateId) {
         vm.event.candidateId = `${vm.candidateId}`;
      }
      let scope = {
         type         : 'form-only',
         responsibles : vm.responsibles,
         eventTypes   : vm.eventTypes,
         vacancies    : vm.vacancies,
         candidates   : vm.candidates,
         event        : vm.event,
         source       : vm.source
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
