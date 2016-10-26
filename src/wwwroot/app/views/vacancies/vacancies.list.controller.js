import './vacancies.list.scss';
const LIST_OF_THESAURUS = ['industry', 'level', 'city',
    'typeOfEmployment'];
import _utils from './../../utils';
import {
   remove,
   find,
   set,
   assign,
   includes,
   filter,
   forEach,
   cloneDeep
} from 'lodash';

const DEFAULT_VACANCY_PREDICATE = {
   current  : 1,
   size     : 20,
   sotrAsc  : false,
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
   LoggerService
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
   vm.isAllToogled              = false;
   vm.sortBy                    = _sortBy;
   vm.getArrow                  = _getArrow;
   vm.searchResponsible         = UserService.autocomplete;
   vm.getFullName               = UserService.getFullName;
   vm.utils                     = _utils;
   vm.getStateTitle             = _getStateTitle;

   (function init() {
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS)
         .then(topics => set(vm, 'thesaurus', topics));
      vm.vacancyPredicate = $state.params.vacancyPredicate || DEFAULT_VACANCY_PREDICATE;
      resetVacancyPredicateStateParams();
      searchVacancies();
      UserService.getUsers().then(users => set(vm, 'responsibles', users));
   }());

   function pageChanged(newPage) {
      vm.vacancyPredicate.current = newPage;
      searchVacancies();
   };

   function searchVacancies(predicate = vm.vacancyPredicate) {
      return SearchService.fetchVacancies(predicate).then(response => {
         forEach(response.vacancies, (vac) => {
            vac.isToogled = vm.isVacancyWasToogled(vac.id);
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
      $state.go('vacancyEdit', {_data: vacancy, vacancyId: vacancy.id, vacancyPredicate: vm.vacancyPredicate});
   }

   function viewVacancy(vacancy) {
      $state.go('vacancyView', {_data: vacancy, vacancyId: vacancy.id, vacancyPredicate: vm.vacancyPredicate});
   }

   function clear() {
      vm.vacancyPredicate = DEFAULT_VACANCY_PREDICATE;
   }

   function deleteVacancy(vacancy) {
      UserDialogService.confirm($translate.instant('VACANCY.VACANCY_REMOVE_MESSAGE')).then(() => {
         let predicate = {id: vacancy.id};
         let vacancyForRemove = find(vm.vacancies.vacancies, predicate);
         VacancyService.remove(vacancyForRemove).then((responseVacancy) => {
            remove(vm.vacancies.vacancies, predicate);
            if (vacancy.parentVacancyId !== null) {
               let parentVacancy = find(vm.vacancies.vacancies, {childVacanciesIds: [ vacancy.id ]});
               assign(parentVacancy, responseVacancy);
            } else if (vacancy.childVacanciesIds.length !== 0) {
               remove(vm.vacancies.vacancies, _vacancy => includes(vacancy.childVacanciesIds, _vacancy.id));
            }
            UserDialogService.notification($translate.instant('DIALOG_SERVICE.SUCCESSFUL_REMOVING'), 'success');
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
            $state.go('candidateProfile',
            { candidateId: vm.candidateIdToGoBack, 'vacancies': vm.selectedVacancies });
         }
      }
   };

   vm.isVacancyWasToogled = (vacancyId) => {
      let foundedVac = find(vm.selectedVacancies, (vac) => {
         return vac.id === vacancyId;
      });
      if (foundedVac) {
         return true;
      } else {
         return false;
      }
   };

   vm.toogleAll = () => {
      vm.selectedVacancies = [];
      if (vm.isAllToogled) {
         forEach(vm.vacancies.vacancies, (vacancy) => {
            vacancy.isToogled = true;
            vm.selectedVacancies.push(vacancy.id);
         });
      } else {
         forEach(vm.vacancies.vacancies, (vacancy) => {
            vacancy.isToogled = false;
         });
      }
   };

   vm.toogleVacancy = (toogledVacancy) => {
      let foundedVac = find(vm.selectedVacancies, (vac) => {
         return vac.id === toogledVacancy.id;
      });
      if (foundedVac) {
         vm.selectedVacancies = filter(vm.selectedVacancies, (vac) => {
            return vac.id !== toogledVacancy.id;
         });
      } else {
         vm.selectedVacancies.push(toogledVacancy);
      }
   };

   function resetVacancyPredicateStateParams() {
      delete $state.params.vacancyPredicate;
   }
}
