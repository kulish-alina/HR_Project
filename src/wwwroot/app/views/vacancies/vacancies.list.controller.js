import './vacancies.list.scss';
const LIST_OF_THESAURUS = ['industry', 'level', 'typeOfEmployment', 'department'];
import _utils from './../../utils';
import {
   remove,
   find,
   set,
   filter,
   forEach,
   cloneDeep,
   includes,
   some
} from 'lodash';

const DEFAULT_VACANCY_PREDICATE = {
   current  : 1,
   size     : 20,
   sortAsc  : false,
   sortBy   : 'CreatedOn'
};

export default function VacanciesController(//eslint-disable-line  max-statements
   $scope,
   $state,
   $q,
   $translate,
   $element,
   $window,
   VacancyService,
   SearchService,
   ThesaurusService,
   UserService,
   UserDialogService,
   LoggerService,
   TransitionsService
   ) {
   'ngInject';

   const vm                     = $scope;
   vm.getVacancy                = getVacancy;
   vm.deleteVacancy             = deleteVacancy;
   vm.editVacancy               = editVacancy;
   vm.viewVacancy               = viewVacancy;
   vm.clear                     = clear;
   vm.thesaurus                 = [];
   vm.responsibles              = [];
   vm.pageChanged               = pageChanged;
   vm.searchVacancies           = searchVacancies;
   vm.vacancies                 = [];
   vm.selectedVacancies         = [];
   vm.candidateIdToGoBack       = $state.params.candidateIdToGoBack;
   vm.toogleContainer           = { isAllToogled : false};
   vm.sortBy                    = _sortBy;
   vm.getArrow                  = _getArrow;
   vm.searchResponsible         = UserService.autocomplete;
   vm.getFullName               = UserService.getFullName;
   vm.utils                     = _utils;
   vm.getStateTitle             = _getStateTitle;
   vm.vacanciesIdsAttachedToCandidate = $state.params.vacanciesIds || [];

   (function init() {
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS)
         .then(topics => set(vm, 'thesaurus', topics));
      ThesaurusService.getOfficeLocations()
         .then(locations => set(vm, 'locations', locations));
      vm.vacancyPredicate = $state.params.vacancyPredicate || cloneDeep(DEFAULT_VACANCY_PREDICATE);
      resetVacancyPredicateStateParams();
      searchVacancies()
        .then(() => {
           forEach(vm.vacancies.vacancies, (vacancy) => {
              if (includes(vm.vacanciesIdsAttachedToCandidate, vacancy.id)) {
                 vacancy.isToogled = true;
              }
           });
        });
      UserService.getUsers().then(users => set(vm, 'responsibles', users));
   }());

   function pageChanged(newPage) {
      vm.vacancyPredicate.current = newPage;
      searchVacancies();
   };

   vm.isVacancyAlreadyAttached = (vacancy) => {
      return includes(vm.vacanciesIdsAttachedToCandidate, vacancy.id);
   };

   function searchVacancies(predicate = vm.vacancyPredicate) {
      return SearchService.fetchVacancies(VacancyService, predicate).then(response => {
         forEach(response.vacancies, (vac) => {
            vac.isToogled = vm.isVacancyWasToogled(vac.id);
            if (includes(vm.vacanciesIdsAttachedToCandidate, vac.id)) {
               vac.isToogled = true;
            }
         });
         vm.vacancies = response;
      }).catch(_onError);
   }

   function getVacancy(vacancyId) {
      VacancyService.getVacancy(vacancyId).then(value => {
         vm.vacancies.push(value);
      }).catch(_onError);
   }

   function editVacancy(vacancy) {
      TransitionsService.go('vacancyEdit', {vacancyId: vacancy.id}, {vacancyPredicate: vm.vacancyPredicate});
   }

   function viewVacancy(vacancy) {
      TransitionsService.go('vacancyView', { vacancyId: vacancy.id}, {vacancyPredicate: vm.vacancyPredicate});
   }

   function clear() {
      vm.vacancyPredicate = cloneDeep(DEFAULT_VACANCY_PREDICATE);
   }

   function deleteVacancy(vacancy) {
      UserDialogService.confirm($translate.instant('VACANCY.VACANCY_REMOVE_MESSAGE')).then(() => {
         let predicate = {id: vacancy.id};
         let vacancyForRemove = find(vm.vacancies.vacancies, predicate);
         VacancyService.remove(vacancyForRemove).then(() => {
            remove(vm.vacancies.vacancies, predicate);
            UserDialogService.notification($translate.instant('DIALOG_SERVICE.SUCCESSFUL_REMOVING'), 'success');
            searchVacancies();
         });
      });
   }

   function _onError(error) {
      UserDialogService.notification($translate.instant('DIALOG_SERVICE.ERROR_VACANCIES_SEARCH'), 'error');
      LoggerService.error(error);
   }

   function _sortBy(column) {
      let searchPredicate = cloneDeep(vm.vacancyPredicate);
      searchPredicate.sortBy = column;
      searchPredicate.sortAsc = (searchPredicate.sortBy === vm.vacancyPredicate.sortBy) ?/*this case is
      switching field 'sortAsc' if same column is selected twice or more and set value to true if new column
      is selected*/
         !!(searchPredicate.sortAsc ^ true) : true; // eslint-disable-line no-bitwise
      searchPredicate.sortBy = column;
      searchVacancies(searchPredicate).then(() => {
         vm.vacancyPredicate = searchPredicate;
      });
   }

   function _getArrow(column) {
      if (column === vm.vacancyPredicate.sortBy) {
         return vm.vacancyPredicate.sortAsc ? 'fi-arrow-down' : 'fi-arrow-up';
      } else {
         return '';
      }
   }

   function _getStateTitle(key) {
      return $translate.instant(key);
   }
   vm.goBackToCandidate = () => {
      if (vm.candidateIdToGoBack) {
         if (vm.selectedVacancies && vm.selectedVacancies.length) {
            TransitionsService.back('candidateProfile',
            { candidateId: vm.candidateIdToGoBack, vacancies : vm.selectedVacancies });
         }
      }
   };

   vm.isVacancyWasToogled = (vacancyId) => {
      return some(vm.selectedVacancies, ['id', vacancyId]);
   };

   vm.toogleAll = () => {
      vm.selectedVacancies = [];
      if (vm.toogleContainer.isAllToogled) {
         forEach(vm.vacancies.vacancies, (vacancy) => {
            vacancy.isToogled = true;
            vm.selectedVacancies.push(vacancy);
         });
      } else {
         forEach(vm.vacancies.vacancies, (vacancy) => {
            if (!includes(vm.vacanciesIdsAttachedToCandidate, vacancy.id)) {
               vacancy.isToogled = false;
            }
         });
      }
   };

   vm.toogleVacancy = (toogledVacancy) => {
      if (toogledVacancy.isToogled) {
         if (toogledVacancy.closingCandidateId) {
            UserDialogService.notification('Selected vacancy has been already closed by another candidate!',
               'warning');
         }
         vm.selectedVacancies.push(toogledVacancy);
      } else {
         vm.selectedVacancies = filter(vm.selectedVacancies, vacancy => vacancy.id !== toogledVacancy.id);
      }
      vm.toogleContainer.isAllToogled = vm.vacancies.vacancies.length === vm.selectedVacancies.length;
   };

   function resetVacancyPredicateStateParams() {
      delete $state.params.vacancyPredicate;
   }
}
