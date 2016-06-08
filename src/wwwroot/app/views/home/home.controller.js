const LIST_OF_THESAURUS = ['industries', 'levels', 'locations',
    'typesOfEmployment'];
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
   UserDialogService
   ) {
   'ngInject';

   const vm           = $scope;
   vm.thesaurus       = [];
   vm.responsibles    = [];
   vm.vacancy         = {};
   vm.vacancies       = [];
   vm.viewVacancy     = viewVacancy;
   vm.total           = 0;
   vm.vacancy.current = 1;
   vm.vacancy.size    = 20;
   vm.pagination      = { current: 1 };
   vm.pageChanged     = pageChanged;

   function pageChanged(newPage) {
      vm.vacancy.current = newPage;
      VacancyService.search(vm.vacancy).then(response => {
         vm.total = response.total;
         vm.vacancies = response.vacancies;
      }).catch(_onError);
   };
   UserService.getUsers().then(users => set(vm, 'responsibles', users));

   ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then(topics => set(vm, 'thesaurus', topics));

   VacancyService.search(vm.vacancy).then(response => {
      vm.total = response.total;
      vm.vacancies = response.vacancies;
   }).catch(_onError);

   function viewVacancy(vacancy) {
      $state.go('vacancyView', {_data: vacancy, vacancyId: vacancy.id});
   }

   function _onError(error) {
      UserDialogService.notification($translate.instant('DIALOG_SERVICE.ERROR_VACANCIES_SEARCH'), 'error');
      LoggerService.error(error);
   }
}
