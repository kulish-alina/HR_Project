const LIST_OF_THESAURUS = ['industry', 'level', 'city',
    'typeOfEmployment'];
import {
   remove,
   find,
   set,
   assign,
   includes,
   filter,
   forEach
} from 'lodash';

export default function VacanciesController(
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
   LocalStorageService
   ) {
   'ngInject';

   const vm                     = $scope;
   vm.getVacancy                = getVacancy;
   vm.deleteVacancy             = deleteVacancy;
   vm.editVacancy               = editVacancy;
   vm.viewVacancy               = viewVacancy;
   vm.cancel                    = cancel;
   vm.thesaurus                 = [];
   vm.responsibles              = [];
   vm.vacancyPredicate           = LocalStorageService.get('vacancyPredicate') || {};
   vm.vacancyPredicate.current   = 0;
   vm.vacancyPredicate.size      = 20;
   vm.pageChanged               = pageChanged;
   vm.searchVacancies           = searchVacancies;
   vm.vacancies                 = LocalStorageService.get('vacancies') || [];
   vm.selectedVacancies         = [];
   vm.candidateIdToGoBack       = $state.params.candidateIdToGoBack;

   (function init() {
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS)
         .then(topics => set(vm, 'thesaurus', topics));
      searchVacancies();
      UserService.getUsers().then(users => set(vm, 'responsibles', users));
   }());

   function pageChanged(newPage) {
      vm.vacancyPredicate.current = newPage - 1;
      searchVacancies();
   };

   function searchVacancies() {
      SearchService.getVacancies(vm.vacancyPredicate).then(response => {
         forEach(response.vacancies, (vac) => {
            vac.isToogled = vm.isVacancyWasToogled(vac.id);
         });
         vm.vacancies = response;
         _setToStorage();
      }).catch(_onError);
   }

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

   function _setToStorage() {
      LocalStorageService.set('vacancyPredicate', vm.vacancyPredicate);
      LocalStorageService.set('vacancies', vm.vacancies);
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
}
