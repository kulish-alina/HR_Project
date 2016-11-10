const LIST_OF_THESAURUS = ['industry', 'level', 'city', 'typeOfEmployment', 'eventtype'];
const AMOUNT_OF_DAYS = 6;
import {
   set,
   cloneDeep,
   clone,
   remove
} from 'lodash';

import './home.scss';

export default function HomeController( //eslint-disable-line max-params
   $scope,
   $q,
   $state,
   $translate,
   SearchService,
   ThesaurusService,
   UserService,
   LoggerService,
   UserDialogService,
   EventsService,
   NoteService,
   CandidateService,
   LocalStorageService,
   VacancyService,
   TransitionsService
   ) {
   'ngInject';

   const vm                   = $scope;
   vm.thesaurus               = [];
   vm.responsibles            = [];
   vm.vacancy                 = {};
   vm.viewVacancy             = viewVacancy;
   vm.vacancy                 = LocalStorageService.get('homeVacancyPredicate') ||
      {current : 1, size : 20, sortBy : 'CreatedOn', sortAsc  : false };
   vm.vacancies               = LocalStorageService.get('homeVacancies') || {};
   vm.candidate               = {};
   vm.candidate.current       = 1;
   vm.candidate.size          = 20;
   vm.pageChanged             = pageChanged;
   vm.userNotes               = [];
   vm.notes                   = cloneDeep(vm.userNotes);
   vm.saveNote                = saveNote;
   vm.removeNote              = removeNote;
   vm.editNote                = editNote;
   vm.upcomingEvents          = [];
   vm.cloneUpcomingEvents     = [];
   vm.eventCondidtion         = {};
   vm.eventCondidtion.userIds = [];
   vm.saveEvent               = saveEvent;
   vm.removeEvent             = removeEvent;
   vm.getEventsForDate        = getEventsForDate;
   vm.user                    = {};

   (function _init() {
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then(topics => set(vm, 'thesaurus', topics));
      NoteService.getNotesByUser().then((notes) => {
         vm.userNotes = notes;
         vm.notes  = cloneDeep(vm.userNotes);
      });
      _getCurrentUser();
      _getUpcomingEvents();
      if (!vm.user) {
         $state.go('login');
      }
      _SearchVacancies();
   }());

   function _SearchVacancies() {
      SearchService.fetchVacancies(VacancyService, vm.vacancy).then(resp => set(vm, 'vacancies', resp))
         .then(() => {
            LocalStorageService.set('homeVacancyPredicate', vm.vacancy);
            LocalStorageService.set('homeVacancies', vm.vacancies);
         })
         .catch(_onError);
   }
   function pageChanged(newPage) {
      vm.vacancy.current = newPage;
      _SearchVacancies();
   };

   function viewVacancy(vacancy) {
      TransitionsService.go('vacancyView', {_data: null, vacancyId: vacancy.id});
   }

   function saveNote(note) {
      return NoteService.save(note).then((res) => {
         vm.userNotes.push(res);
         return vm.notes  = cloneDeep(vm.userNotes);
      });
   }

   function removeNote(note) {
      return UserDialogService.confirm($translate.instant('DIALOG_SERVICE.NOTE_REMOVING_DIALOG')).then(() => {
         return NoteService.remove(note).then(() => {
            remove(vm.userNotes, {id: note.id});
            return vm.notes  = cloneDeep(vm.userNotes);
         });
      });
   }

   function editNote(note) {
      return $q.when(remove(vm.userNotes, {id: note.id}));
   }

   function _onError(error) {
      UserDialogService.notification($translate.instant('DIALOG_SERVICE.ERROR_VACANCIES_SEARCH'), 'error');
      LoggerService.error(error);
   }

   function _getUpcomingEvents() {
      _getCurrentUser();
      _formingDateConditions();
      EventsService.getEventsForPeriod(vm.eventCondidtion).then(events => {
         set(vm, 'upcomingEvents', events);
         vm.cloneUpcomingEvents  = clone(vm.upcomingEvents);
      });
   }

   function saveEvent(event) {
      return EventsService.save(event).then((responseEvent) => {
         _getUpcomingEvents();
         vm.cloneUpcomingEvents  = clone(vm.upcomingEvents);
         return responseEvent;
      });
   }

   function removeEvent(event) {
      EventsService.remove(event).then(() => {
         remove(vm.upcomingEvents, {id: event.id});
         vm.cloneUpcomingEvents  = clone(vm.upcomingEvents);
         UserDialogService.notification($translate.instant('DIALOG_SERVICE.SUCCESSFUL_REMOVING_EVENT'), 'success');
      });
   }

   function getEventsForDate(date) {
      vm.eventCondidtion.startDate = date;
      vm.eventCondidtion.endDate = vm.eventCondidtion.startDate;
      return EventsService.getEventsForPeriod(vm.eventCondidtion);
   }

   function _getCurrentUser() {
      vm.eventCondidtion.userIds = [];
      vm.user = UserService.getCurrentUser();
      vm.eventCondidtion.userIds.push(vm.user.id);
   }

   function _formingDateConditions() {
      vm.eventCondidtion.startDate = new Date();
      let featureDate = new Date();
      featureDate.setDate(vm.eventCondidtion.startDate.getDate() + AMOUNT_OF_DAYS);
      vm.eventCondidtion.endDate = featureDate;
   }
}
