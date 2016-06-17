import {
   set
} from 'lodash';
import template from './events.directive.html';
import './events.scss';
export default class EventsDirective {
   constructor() {
      this.restrict   = 'E';
      this.template   = template;
      this.scope      = {
         type        : '@',
         events      : '=',
         save        : '=',
         remove      : '=',
         edit        : '=',
         evertsByDate: '='
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
   const vm              = $scope;
   vm.vacancies          = [];
   vm.candidates         = [];
   vm.responsibles       = [];
   vm.eventTypes         = [];
   vm.showAddEventDialog = showAddEventDialog;
   vm.vacancy            = {};
   vm.vacancy.current    = 0;
   vm.vacancy.size       = 20;

   function _init() {
      VacancyService.search(vm.vacancy).then(data => set(vm, 'vacancies', data.vacancies));
      UserService.getUsers().then(users => set(vm, 'responsibles', users));
      CandidateService.getCandidates().then(candidates => set(vm, 'candidates', candidates));
      ThesaurusService.getThesaurusTopics('eventtype').then(eventTypes => set(vm, 'eventTypes', eventTypes));
   }
   console.log(vm);

   _init();

   function showAddEventDialog() {
      let scope = {
         type         : 'list-with-input',
         responsibles : vm.responsibles,
         eventTypes   : vm.eventTypes,
         vacancies    : vm.vacancies,
         candidates   : vm.candidates
      };
      let buttons = [
         {
            name: $translate.instant('COMMON.CANCEL')
         },
         {
            name: $translate.instant('COMMON.APLY'),
            func: vm.save,
            needValidate: true
         }
      ];
      UserDialogService.dialog($translate.instant('COMMON.EVENTS'), template, buttons, scope);
   }

}
