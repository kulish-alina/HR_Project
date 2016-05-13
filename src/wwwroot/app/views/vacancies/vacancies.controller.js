//import utils from '../../utils';
const LIST_OF_THESAURUS = ['industries', 'levels', 'locations',
    'typesOfEmployment', 'entityStates'];

export default function VacanciesController(
   $scope,
   VacancyService,
   ThesaurusService,
   $q,
   UserService
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

   ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then((data) => vm.thesaurus = data);

   UserService.getUsers().then((users) => {
      vm.responsibles = users;
   });

   function getVacancies() {
      VacancyService.getVacancies().then(value => vm.vacancies = value).catch(_onError);
   }

   function getVacancy(vacancyId) {
      VacancyService.getVacancy(vacancyId).then(value => {
         vm.vacancies.push(value);
         console.log('vm.vacancies', vm.vacancies);
      }).catch(_onError);
   }

   function editVacancy(vacancy) {
      VacancyService.saveVacancy(vacancy).catch(_onError);
   }

   function deleteVacancy(vacancy) {
      VacancyService.deleteVacancy(vacancy);
   }

   function _onError() {
      vm.errorMessage = 'Sorry! Some error occurred';
   }
}
