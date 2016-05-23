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
   vm.getVacancy = getVacancy;
   vm.deleteVacancy = deleteVacancy;
   vm.editVacancy = editVacancy;
   vm.cancel = cancel;
   vm.thesaurus = [];
   vm.responsibles = [];
   vm.searchVacancies = searchVacancies;
   vm.beforeOpenModal = beforeOpenModal;
   vm.vacancy = {};
   vm.vacancies = [];
   vm.total = 0;
   vm.vacancy.current = 1;
   vm.vacancy.size = 20;
   vm.pagination = { current: 1 };
   vm.pageChanged = pageChanged;

   function pageChanged(newPage) {
      vm.vacancy.current = newPage;
      VacancyService.search(vm.vacancy).then(response => {
         vm.total = response.total;
         vm.vacancies = response.vacancies;
      }).catch(_onError);
   };

   ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then((data) => vm.thesaurus = data);

   UserService.getUsers().then((users) => {
      vm.responsibles = users;
   });

   function searchVacancies() {
      VacancyService.search(vm.vacancy).then(response => {
         vm.total = response.total;
         vm.vacancies = response.vacancies;
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

   function cancel() {
      $state.reload();
   }

   function beforeOpenModal(vacancyId) {
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
