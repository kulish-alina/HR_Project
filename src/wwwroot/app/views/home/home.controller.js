const LIST_OF_THESAURUS = ['industries', 'levels', 'locations',
    'typesOfEmployment'];

export default function VacanciesController(
   $scope,
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
   vm.total = 0;
   vm.vacancy.current = 1;
   vm.vacancy.size = 10;
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

   function _onError() {
      vm.errorMessage = 'Sorry! Some error occurred';
   }
}
