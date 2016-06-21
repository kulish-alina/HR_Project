import {
   set,
   clone
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

function EventsController($scope, $translate, VacancyService, CandidateService, UserService,
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

   function showAddEventDialog() {
      vm.event = {};
      let defaultDate = new Date();
      vm.getEventsByDate(defaultDate).then((e) => set(vm, 'currentEvents', e));
      let scope = {
         type         : 'list-with-input',
         responsibles : vm.responsibles,
         eventTypes   : vm.eventTypes,
         vacancies    : vm.vacancies,
         candidates   : vm.candidates,
         events       : vm.currentEvents,
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
