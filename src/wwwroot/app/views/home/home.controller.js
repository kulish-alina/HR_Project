const LIST_OF_THESAURUS = ['industry', 'level', 'location',
    'typeOfEmployment'];
import {
   set
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

   const vm           = $scope;
   vm.thesaurus       = [];
   vm.responsibles    = [];
   vm.vacancy         = {};
   vm.vacancies       = [];
   vm.viewVacancy     = viewVacancy;
   vm.total           = 0;
   vm.vacancy.current = 0;
   vm.vacancy.size    = 20;
   vm.pagination      = { current: 0 };
   vm.pageChanged     = pageChanged;
   vm.upcomingEvents  = [];
   vm.eventCondidtion = {};
   vm.eventCondidtion.userIds = [];
   vm.saveEvent       = saveEvent;
   vm.removeEvent     = removeEvent;
   vm.editEvent       = editEvent;
   vm.getEventForDate = getEventForDate;

   function _init() {
      UserService.getUsers().then(users => set(vm, 'responsibles', users));
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then(topics => set(vm, 'thesaurus', topics));
      VacancyService.search(vm.vacancy).then(response => {
         vm.total = response.total;
         vm.vacancies = response.vacancies;
      }).catch(_onError);
      getUpcomingEvents();
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

   function getUpcomingEvents() {
      _formingDateConditions();
      //TODO: change getUserById to getCurrentUser
      UserService.getUserById(3).then((user) => {
         vm.eventCondidtion.userIds.push(user.id);
         EventsService.getEventsForPeriod(vm.eventCondidtion).then(events => set(vm, 'upcomingEvents', events));
      });
   }

   function saveEvent(event) {
      alert(event);
   }

   function removeEvent(event) {
      alert(event);
   }

   function editEvent(event) {
      alert(event);
   }

   function getEventForDate(event) {
      alert(event);
   }

   function _formingDateConditions() {
      vm.eventCondidtion.startDate = new Date();
      let featureDate = new Date();
      featureDate.setDate(
         vm.eventCondidtion.startDate.getDate() + 6);
      vm.eventCondidtion.endDate = featureDate;
   }
}
