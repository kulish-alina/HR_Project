import {
   remove,
   find,
   set
} from 'lodash';

const LIST_OF_THESAURUS = ['industry', 'level', 'city', 'language', 'languageLevel',
    'department', 'typeOfEmployment', 'tag', 'skill', 'stage'];

export default function CandidatesController(
   $scope,
   $state,
   $q,
   $translate,
   CandidateService,
   ThesaurusService,
   UserDialogService,
   LoggerService
   ) {
   'ngInject';
   const vm             = $scope;
   vm.deleteCandidate   = deleteCandidate;
   vm.editCandidate     = editCandidate;
   vm.viewCandidate     = viewCandidate;
   vm.cancel            = cancel;
   vm.thesaurus         = [];
   vm.searchCandidates  = searchCandidates;
   vm.candidates        = [];
   vm.total             = 0;
   vm.pagination        = { current: 0 };
   vm.pageChanged       = pageChanged;
   vm.slider = {
      min: 21,
      max: 45,
      options: {
         floor: 15,
         ceil: 65,
         onChange() {
            vm.candidate.minAge  = vm.slider.min;
            vm.candidate.maxAge  = vm.slider.max;
         }
      }
   };

   function _initData() {
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then(topics => set(vm, 'thesaurus', topics));
      _initPagination();
   }
   _initData();

   function _initPagination() {
      vm.candidate = {};
      vm.candidate.current = 0;
      vm.candidate.size    = 20;
   }

   function pageChanged(newPage) {
      vm.candidate.current = newPage;
      CandidateService.search(vm.candidate).then(response => {
         vm.total = response.total;
         vm.candidates = response.candidate;
      }).catch(_onError);
   };

   function searchCandidates() {
      CandidateService.search(vm.candidate).then(response => {
         vm.total = response.total;
         vm.candidates = response.candidate;
         console.log(vm.candidates);
         _initPagination();
      }).catch(_onError);
   }

   function editCandidate(candidate) {
      $state.go('candidate', {_data: candidate, candidateId: candidate.id});
   }

   function viewCandidate(candidate) {
      $state.go('candidateProfile', {_data: candidate, candidateId: candidate.id});
   }

   function cancel() {
      $state.reload();
   }

   function deleteCandidate(candidateId) {
      UserDialogService.confirm($translate.instant('DIALOG_SERVICE.CANDIDATE_REMOVING_DIALOG')).then(() => {
         let predicate = {id: candidateId};
         let candidateForRemove = find(vm.candidates, predicate);
         CandidateService.deleteCandidate(candidateForRemove).then(() => {
            remove(vm.candidates, predicate);
            UserDialogService.notification
            ($translate.instant('DIALOG_SERVICE.SUCCESSFUL_REMOVING_CANDIDATE'), 'success');
         });
      });
   }

   function _onError(error) {
      UserDialogService.notification($translate.instant('DIALOG_SERVICE.ERROR_CANDIDATES_SEARCH'), 'error');
      LoggerService.error(error);
   }
}
