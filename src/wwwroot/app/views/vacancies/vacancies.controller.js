const LIST_OF_THESAURUS = ['industries', 'levels', 'locations',
    'typesOfEmployment'];
import {
   remove,
   find
} from 'lodash';

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
   vm.beforeOpenModal = beforeOpenModal;
   vm.vacancy = {};
   vm.currentPage = 1;
   vm.pageSize = 10;

   ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then((data) => vm.thesaurus = data);

   UserService.getUsers().then((users) => {
      vm.responsibles = users;
   });

   function searchVacancies() {
      VacancyService.search(vm.vacancy).then(value => {
         console.log(value.vacancies);
         vm.vacancies = value.vacancies;
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

   function editVacancy(vacancy) {
      $state.go('vacancy', {_data: vacancy});
   }

   function beforeOpenModal(vacancyId) {
      console.log(vacancyId);
      vm.selectedVacancyId = vacancyId;
   }

   function deleteVacancy() {
      let vacancyForRemove = find(vm.vacancies, {id: vm.selectedVacancyId});
      VacancyService.remove(vacancyForRemove)
         .then(() => remove(vm.vacancies, (vacancy) => {
            return vacancy.id === vm.selectedVacancyId;
         }));
   }

   function _onError() {
      vm.errorMessage = 'Sorry! Some error occurred';
   }
}
