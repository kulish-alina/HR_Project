import './candidates.scss';
import {
   remove,
   find,
   set,
   filter,
   forEach,
   cloneDeep
} from 'lodash';

const LIST_OF_THESAURUS = ['industry', 'level', 'city', 'language', 'languageLevel',
    'department', 'typeOfEmployment', 'tag', 'skill', 'stage', 'currency', 'parsingSource'];

export default function CandidatesController(
   $scope,
   $state,
   $q,
   $translate,
   $element,
   $window,
   CandidateService,
   SearchService,
   ThesaurusService,
   UserDialogService,
   LoggerService,
   LocalStorageService
   ) {
   'ngInject';
   const vm                = $scope;
   vm.deleteCandidate      = deleteCandidate;
   vm.editCandidate        = editCandidate;
   vm.viewCandidate        = viewCandidate;
   vm.clear                = clear;
   vm.thesaurus            = [];
   vm.searchCandidates     = searchCandidates;
   vm.candidatePredicate   = { current : 1, size : 20, sotrAsc : true, sortBy : 'Lastname'};
   vm.pageChanged          = pageChanged;
   vm.selectedCandidates   = [];
   vm.vacancyIdToGoBack    = $state.params.vacancyIdToGoBack;
   vm.isActiveAgeField     = true;
   vm.useAgeInSearch       = useAgeInSearch;
   vm.isAllToogled         = false;
   vm.sortBy               = _sortBy;
   vm.getArrow             = _getArrow;

   vm.slider = {
      min: 18,
      max: 45,
      options: {
         floor: 15,
         ceil: 65,
         onChange() {
            vm.candidatePredicate.minAge  = vm.slider.min;
            vm.candidatePredicate.maxAge  = vm.slider.max;
         }
      }
   };

   (function _initData() {
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS)
         .then(topics => set(vm, 'thesaurus', topics));
      vm.candidates = LocalStorageService.get('candidates') || [];
      vm.candidatePredicate = LocalStorageService.get('candidatePredicate') || {};
      vm.candidatePredicate.minAge  = vm.slider.min;
      vm.candidatePredicate.maxAge  = vm.slider.max;
   }());

   function pageChanged(newPage) {
      vm.candidatePredicate.current = newPage;
      searchCandidates();
   };

   function searchCandidates(predicate = vm.candidatePredicate) {
      return SearchService.fetchCandidates(predicate).then(response => {
         forEach(response.candidate, (cand) => {
            cand.isToogled = vm.isCandidateWasToogled(cand.id);
         });
         vm.candidates = response;
         _setToStorage();
      }).catch(_onError);
   }

   function editCandidate(candidate) {
      $state.go('candidate', {_data: null, candidateId: candidate.id});
   }

   function viewCandidate(candidate) {
      $state.go('candidateProfile', {_data: null, candidateId: candidate.id});
   }

   function clear() {
      vm.candidatePredicate = {};
      vm.candidatePredicate.current = 1;
      vm.candidatePredicate.size = 20;
      vm.isActiveAgeField = false;
   }

   function useAgeInSearch() {
      if (vm.isActiveAgeField === true) {
         vm.candidatePredicate.minAge  = vm.slider.min;
         vm.candidatePredicate.maxAge  = vm.slider.max;
      } else {
         vm.candidatePredicate.minAge  = null;
         vm.candidatePredicate.maxAge  = null;
      }
   }

   function deleteCandidate(candidateId) {
      UserDialogService.confirm($translate.instant('DIALOG_SERVICE.CANDIDATE_REMOVING_DIALOG')).then(() => {
         let predicate = {id: candidateId};
         let candidateForRemove = find(vm.candidates.candidate, predicate);
         CandidateService.deleteCandidate(candidateForRemove).then(() => {
            remove(vm.candidates.candidate, predicate);
            UserDialogService.notification
            ($translate.instant('DIALOG_SERVICE.SUCCESSFUL_REMOVING_CANDIDATE'), 'success');
         });
      });
   }

   function _onError(error) {
      UserDialogService.notification($translate.instant('DIALOG_SERVICE.ERROR_CANDIDATES_SEARCH'), 'error');
      LoggerService.error(error);
   }

   function _setToStorage() {
      LocalStorageService.set('candidatePredicate', vm.candidatePredicate);
      LocalStorageService.set('candidates', vm.candidates);
   }

   function _sortBy(column) {
      let searchPredicate = cloneDeep(vm.candidatePredicate);
      searchPredicate.sortBy = column;
      searchPredicate.sortAsc = (searchPredicate.sortBy === vm.candidatePredicate.sortBy) ?/*this case is switching
      field 'sort Asc' if same column is selected twice or more and set value to true if new column is
      selected*/
         !!(searchPredicate.sortAsc ^ true) : true; // eslint-disable-line no-bitwise
      searchPredicate.sortBy = column;
      searchCandidates(searchPredicate).then(() => {
         vm.candidatePredicate = searchPredicate;
         _setToStorage();
      });
   }

   function _getArrow(column) {
      if (column === vm.candidatePredicate.sortBy) {
         return vm.candidatePredicate.sortAsc ? 'fi-arrow-down' : 'fi-arrow-up';
      } else {
         return '';
      }
   }

   vm.goBackToVacancy = () => {
      if (vm.vacancyIdToGoBack) {
         if (vm.selectedCandidates && vm.selectedCandidates.length) {
            $state.go('vacancyView', { vacancyId: vm.vacancyIdToGoBack, 'candidatesIds': vm.selectedCandidates });
         }
      }
   };

   vm.isCandidateWasToogled = (candidateId) => {
      let foundedCand = find(vm.selectedCandidates, (cand) => {
         return cand === candidateId;
      });
      if (foundedCand) {
         return true;
      } else {
         return false;
      }
   };

   vm.toogleAll = () => {
      vm.selectedCandidates = [];
      if (vm.isAllToogled) {
         forEach(vm.candidates.candidate, (candidate) => {
            candidate.isToogled = true;
            vm.selectedCandidates.push(candidate.id);
         });
      } else {
         forEach(vm.candidates.candidate, (candidate) => {
            candidate.isToogled = false;
         });
      }
   };

   vm.toogleCandidate = (candidateId) => {
      let foundedCand = find(vm.selectedCandidates, (cand) => {
         return cand === candidateId;
      });
      if (foundedCand) {
         vm.selectedCandidates = filter(vm.selectedCandidates, (candId) => {
            return candId !== candidateId;
         });
      } else {
         vm.selectedCandidates.push(candidateId);
      }
   };
}
