const LIST_OF_THESAURUS = ['industries', 'levels', 'locations',
    'typesOfEmployment'];

export default function VacanciesController(
   $scope,
   VacancyService,
   ThesaurusService,
   $q,
   UserService,
   $state
   ) {
   'ngInject';

   const vm = $scope;
   vm.vacancies = [];
   vm.getVacancies = getVacancies;
   vm.getVacancy = getVacancy;
   vm.deleteVacancy = deleteVacancy;
   vm.editVacancy = editVacancy;
   vm.thesaurus = [];
   vm.responsibles = [];
   vm.searchVacancies = searchVacancies;
   vm.vacancy = {};
   vm.currentPage = 1;
   vm.pageSize = 10;

   ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then((data) => vm.thesaurus = data);

   UserService.getUsers().then((users) => {
      vm.responsibles = users;
   });

   function searchVacancies() {
      VacancyService.searchVacancies(vm.vacancy).then(value => {
         vm.vacancies = value;
      }).catch(_onError);
   }

   function getVacancies() {
      VacancyService.getVacancies().then(value => {
         vm.vacancies = value;
      }).catch(_onError);
   }

   function getVacancy(vacancyId) {
      VacancyService.getVacancy(vacancyId).then(value => {
         vm.vacancies.push(value);
      }).catch(_onError);
   }

   function editVacancy(index) {
      $state.go('vacancy', {_data: vm.vacancies[index]});
   }

   function deleteVacancy(index) {
      VacancyService.deleteVacancy(vm.vacancies[index]).then(() => delete vm.vacancies[index]);
   }

   function _onError() {
      vm.errorMessage = 'Sorry! Some error occurred';
   }
}
