const LIST_OF_THESAURUS = ['industries', 'levels', 'locations',
    'typesOfEmployment'];

export default function VacanciesController(
   $scope,
   $state,
   VacancyService,
   ThesaurusService,
   UserService
   ) {
   'ngInject';

   const vm = $scope;
   vm.thesaurus = [];
   vm.responsibles = [];
   vm.vacancy = {};
   vm.vacancies = [];
   vm.viewVacancy = viewVacancy;
   vm.total = 0;
   vm.vacancy.current = 1;
   vm.vacancy.size = 15;
   vm.pagination = { current: 1 };
   vm.pageChanged = pageChanged;

   function pageChanged(newPage) {
      vm.vacancy.current = newPage;
      VacancyService.search(vm.vacancy).then(response => {
         vm.total = response.total;
         vm.vacancies = response.vacancies;
      }).catch(_onError);
   };
   UserService.getUsers().then((users) => {
      vm.responsibles = users;
   });

   ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then((data) => vm.thesaurus = data);

   VacancyService.search(vm.vacancy).then(response => {
      vm.total = response.total;
      vm.vacancies = response.vacancies;
   }).catch(_onError);

   function viewVacancy(vacancy) {
      $state.go('vacancyView', {_data: vacancy, vacancyId: vacancy.id});
   }

   function _onError() {
      vm.errorMessage = 'Sorry! Some error occurred';
   }
}
