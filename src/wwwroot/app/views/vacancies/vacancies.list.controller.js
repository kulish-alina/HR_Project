const LIST_OF_THESAURUS = ['industry', 'level', 'city',
    'typeOfEmployment'];
import {
   remove,
   find,
   set
} from 'lodash';

export default function VacanciesController(
   $scope,
   $state,
   $q,
   $translate,
   $element,
   $window,
   VacancyService,
   ThesaurusService,
   UserService,
   UserDialogService,
   LoggerService,
   LocalStorageService
   ) {
   'ngInject';

   const vm            = $scope;
   vm.getVacancy       = getVacancy;
   vm.deleteVacancy    = deleteVacancy;
   vm.editVacancy      = editVacancy;
   vm.viewVacancy      = viewVacancy;
   vm.cancel           = cancel;
   vm.thesaurus        = [];
   vm.responsibles     = [];
   vm.searchVacancies  = searchVacancies;
   vm.vacancy.current  = 0;
   vm.vacancy.size     = 20;
   vm.pageChanged      = pageChanged;
   vm.vacancy          = LocalStorageService.get('vacancy') || {};
   vm.vacancies        = LocalStorageService.get('vacancies') || [];
   (function init() {
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS)
         .then(topics => set(vm, 'thesaurus', topics));
      UserService.getUsers().then(users => set(vm, 'responsibles', users));
      $element.on('$destroy', _setToStorage);
      $window.onbeforeunload = _setToStorage;
   }());

   function pageChanged(newPage) {
      vm.vacancy.current = newPage - 1;
      searchVacancies();
   };

   function searchVacancies() {
      VacancyService.search(vm.vacancy).then(response => {
         vm.vacancies = response;
      }).catch(_onError);
   }

   function pageChanged(newPage) {
      vm.vacancy.current = newPage;
      searchVacancies();
   };

   function getVacancy(vacancyId) {
      VacancyService.getVacancy(vacancyId).then(value => {
         vm.vacancies.push(value);
      }).catch(_onError);
   }

   function editVacancy(vacancy) {
      $state.go('vacancyEdit', {_data: vacancy, vacancyId: vacancy.id});
   }

   function viewVacancy(vacancy) {
      $state.go('vacancyView', {_data: vacancy, vacancyId: vacancy.id});
   }

   function cancel() {
      $state.reload();
   }

   function deleteVacancy(vacancyId) {
      UserDialogService.confirm($translate.instant('VACANCY.VACANCY_REMOVE_MESSAGE')).then(() => {
         let predicate = {id: vacancyId};
         let vacancyForRemove = find(vm.vacancies.vacancies, predicate);
         VacancyService.remove(vacancyForRemove).then(() => {
            remove(vm.vacancies.vacancies, predicate);
            UserDialogService.notification($translate.instant('DIALOG_SERVICE.SUCCESSFUL_REMOVING'), 'success');
         });
      });
   }

   function _onError(error) {
      UserDialogService.notification($translate.instant('DIALOG_SERVICE.ERROR_VACANCIES_SEARCH'), 'error');
      LoggerService.error(error);
   }

   function _setToStorage() {
      LocalStorageService.set('vacancy', vm.vacancy);
      LocalStorageService.set('vacancies', vm.vacancies);
   }
}
