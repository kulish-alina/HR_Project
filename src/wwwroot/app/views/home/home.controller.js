const LIST_OF_THESAURUS = ['industry', 'level', 'city',
    'typeOfEmployment'];
import {
   set,
   clone,
   remove
} from 'lodash';

export default function VacanciesController(
   $scope,
   $state,
   $translate,
   VacancyService,
   ThesaurusService,
   UserService,
   LoggerService,
   UserDialogService,
   EventsService
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

   _init();

   function pageChanged(newPage) {
      vm.vacancy.current = newPage;
      VacancyService.search(vm.vacancy).then(response => {
         vm.total = response.total;
         vm.vacancies = response.vacancies;
      }).catch(_onError);
   };

   function viewVacancy(vacancy) {
      $state.go('vacancyView', {_data: vacancy, vacancyId: vacancy.id});
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
