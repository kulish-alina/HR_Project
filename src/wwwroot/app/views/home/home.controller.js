const LIST_OF_THESAURUS = ['industry', 'level', 'city',
    'typeOfEmployment'];
import {
   set,
   clone,
   remove
} from 'lodash';

export default function HomeController(
   $scope,
   $q,
   $state,
   $translate,
   VacancyService,
   ThesaurusService,
   UserService,
   LoggerService,
   UserDialogService,
   EventsService
   NoteService
   ) {
   'ngInject';

   const vm                   = $scope;
   vm.thesaurus               = [];
   vm.responsibles            = [];
   vm.vacancy                 = {};
   vm.vacancies               = [];
   vm.viewVacancy             = viewVacancy;
   vm.total                   = 0;
   vm.vacancy.current         = 0;
   vm.vacancy.size            = 30;
   vm.pagination              = { current: 0 };
   vm.pageChanged             = pageChanged;
   vm.userNotes       = [];
   vm.notes           = cloneDeep(vm.userNotes);
   vm.saveNote        = saveNote;
   vm.removeNote      = removeNote;
   vm.editNote        = editNote;
   vm.upcomingEvents          = [];
   vm.cloneUpcomingEvents     = [];
   vm.eventCondidtion         = {};
   vm.eventCondidtion.userIds = [];
   vm.saveEvent               = saveEvent;
   vm.removeEvent             = removeEvent;
   vm.getEventsForDate        = getEventsForDate;

   function _init() {
      UserService.getUsers().then(users => set(vm, 'responsibles', users));
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then(topics => set(vm, 'thesaurus', topics));
      VacancyService.search(vm.vacancy).then(response => {
         vm.total = response.total;
         vm.vacancies = response.vacancies;
      }).catch(_onError);
      _getCurrentUser().then(() => _getUpcomingEvents());
   }

   function _init() {
      UserService.getUsers().then(users => set(vm, 'responsibles', users));
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then(topics => set(vm, 'thesaurus', topics));
      NoteService.getNotesByUser().then((notes) => {
         vm.userNotes = notes;
         vm.notes  = cloneDeep(vm.userNotes);
      });
      VacancyService.search(vm.vacancy).then(response => {
         vm.total = response.total;
         vm.vacancies = response.vacancies;
      }).catch(_onError);
   };
   _init();

   function viewVacancy(vacancy) {
      $state.go('vacancyView', {_data: vacancy, vacancyId: vacancy.id});
   }

   function saveNote(note) {
      return NoteService.save(note).then((res) => {
         vm.userNotes.push(res);
         return vm.notes  = cloneDeep(vm.userNotes);
      });
   }

   function removeNote(note) {
      UserDialogService.confirm($translate.instant('DIALOG_SERVICE.NOTE_REMOVING_DIALOG')).then(() => {
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
      _formingDateConditions();
      EventsService.getEventsForPeriod(vm.eventCondidtion).then(events => {
         set(vm, 'upcomingEvents', events);
         vm.cloneUpcomingEvents  = clone(vm.upcomingEvents);
      });
   }

   function saveEvent(event) {
      EventsService.save(event).then(() => {
         _getUpcomingEvents();
         vm.cloneUpcomingEvents  = clone(vm.upcomingEvents);
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
      //TODO: change getUserById to getCurrentUser
      return UserService.getUserById(150).then((user) => vm.eventCondidtion.userIds.push(user.id));
   }

   function _formingDateConditions() {
      vm.eventCondidtion.startDate = new Date();
      let featureDate = new Date();
      featureDate.setDate(
         vm.eventCondidtion.startDate.getDate() + 6);
      vm.eventCondidtion.endDate = featureDate;
   }
}
