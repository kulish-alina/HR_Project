import './candidates.scss';
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

import utils from './../../utils.js';

const LIST_OF_THESAURUS = ['industry', 'level', 'city', 'language', 'languageLevel',
    'department', 'typeOfEmployment', 'tag', 'skill', 'stage', 'currency'];

const DEFAULT_MIN_AGE = 18;
const DEFAULT_MAX_AGE = 45;

const DEFAULT_CANDIDATE_PREDICATE = {
   current  : 1,
   size     : 20,
   sortAsc  : false,
   sortBy   : 'CreatedOn'
};

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
   ValidationService,
   TransitionsService
   ) {
   'ngInject';
   const vm                = $scope;
   vm.candidates           = [];
   vm.deleteCandidate      = deleteCandidate;
   vm.editCandidate        = editCandidate;
   vm.viewCandidate        = viewCandidate;
   vm.clear                = clear;
   vm.thesaurus            = [];
   vm.submit               = submit;
   vm.pageChanged          = pageChanged;
   vm.selectedCandidates   = [];
   vm.vacancyIdToGoBack    = $state.params.vacancyIdToGoBack;
   vm.isActiveAgeField     = false;
   vm.useAgeInSearch       = useAgeInSearch;
   vm.toogleContainer      = { isAllToogled : false};
   vm.sortBy               = _sortBy;
   vm.getArrow             = _getArrow;
   vm.locationsSort        = utils.locationsSort;
   vm.candidateIdsAttachedToVacancy = $state.params.candidateIds || [];

   vm.slider = {
      min: DEFAULT_MIN_AGE,
      max: DEFAULT_MAX_AGE,
      options: {
         floor: 15,
         ceil: 65,
         onChange() {
            vm.candidatePredicate.minAge  = vm.slider.min;
            vm.candidatePredicate.maxAge  = vm.slider.max;
         }
      }
   };

   vm.isCandidateAlreadyAttached = (candidate) => {
      return includes(vm.candidateIdsAttachedToVacancy, candidate.id);
   };

   (function _initData() {
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS)
         .then(topics => set(vm, 'thesaurus', topics));
      vm.candidatePredicate = $state.params.candidatePredicate || cloneDeep(DEFAULT_CANDIDATE_PREDICATE);
      resetCandidatePredicateStateParams();
      _searchCandidates()
      .then(() => {
         forEach(vm.candidates.candidate, (candidate) => {
            if (includes(vm.candidateIdsAttachedToVacancy, candidate.id)) {
               candidate.isToogled = true;
            }
         });
      });
   }());

   function pageChanged(newPage) {
      vm.candidatePredicate.current = newPage;
      _searchCandidates();
   };

   function _searchCandidates(predicate = vm.candidatePredicate) {
      return SearchService.fetchCandidates(CandidateService, predicate).then(response => {
         forEach(response.candidate, (cand) => {
            cand.isToogled = vm.isCandidateWasToogled(cand.id);
            if (includes(vm.candidateIdsAttachedToVacancy, cand.id)) {
               cand.isToogled = true;
            }
         });
         vm.candidates = response;
      }).catch(_onError);
   }

   function submit(form) {
      ValidationService.validate(form).then(() => {
         _searchCandidates();
      });
   }
   function editCandidate(candidate) {
      TransitionsService.go('candidate',
                            { candidateId : candidate.id },
                            { candidatePredicate : vm.candidatePredicate });
   }

   function viewCandidate(candidate) {
      TransitionsService.go(
         'candidateProfile', { candidateId: candidate.id }, { candidatePredicate: vm.candidatePredicate });
   }

   function clear() {
      vm.candidatePredicate   = cloneDeep(DEFAULT_CANDIDATE_PREDICATE);
      vm.isActiveAgeField     = false;
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
            _searchCandidates();
         });
      });
   }

   function _onError(error) {
      UserDialogService.notification($translate.instant('DIALOG_SERVICE.ERROR_CANDIDATES_SEARCH'), 'error');
      LoggerService.error(error);
   }

   function _sortBy(column) {
      let searchPredicate = cloneDeep(vm.candidatePredicate);
      searchPredicate.sortBy = column;
      searchPredicate.sortAsc = (searchPredicate.sortBy === vm.candidatePredicate.sortBy) ?/*this case is switching
      field 'sort Asc' if same column is selected twice or more and set value to true if new column is
      selected*/
         !!(searchPredicate.sortAsc ^ true) : true; // eslint-disable-line no-bitwise
      searchPredicate.sortBy = column;
      _searchCandidates(searchPredicate).then(() => {
         vm.candidatePredicate = searchPredicate;
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
            TransitionsService.back('vacancyView',
                                  { vacancyId: vm.vacancyIdToGoBack,
                                   candidatesIds: vm.selectedCandidates });
         }
      }
   };

   vm.isCandidateWasToogled = (candidateId) => {
      return some(vm.selectedCandidates, selectedCandidateId => selectedCandidateId === candidateId);
   };

   vm.toogleAll = () => {
      vm.selectedCandidates = [];
      if (vm.toogleContainer.isAllToogled) {
         forEach(vm.candidates.candidate, (candidate) => {
            candidate.isToogled = true;
            vm.selectedCandidates.push(candidate.id);
         });
      } else {
         forEach(vm.candidates.candidate, (candidate) => {
            if (!includes(vm.candidateIdsAttachedToVacancy, candidate.id)) {
               candidate.isToogled = false;
            }
         });
      }
   };

   vm.toogleCandidate = (candidate) => {
      if (candidate.isToogled) {
         if (candidate.closedVacanciesIds.length) {
            UserDialogService.notification('Selected candidate has been already hired for another position!',
               'warning');
         }
         vm.selectedCandidates.push(candidate.id);
      } else {
         vm.selectedCandidates = filter(vm.selectedCandidates, candId => candId !== candidate.id);
      }
      vm.toogleContainer.isAllToogled = vm.candidates.candidate.length === vm.selectedCandidates.length;
   };

   function resetCandidatePredicateStateParams() {
      delete $state.params.candidatePredicate;
   }
}
