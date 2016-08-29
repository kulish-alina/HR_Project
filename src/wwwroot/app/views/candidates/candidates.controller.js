import {
   remove,
   find,
   set,
   filter,
   forEach
} from 'lodash';

const LIST_OF_THESAURUS = ['industry', 'level', 'city', 'language', 'languageLevel',
    'department', 'typeOfEmployment', 'tag', 'skill', 'stage'];

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
   vm.cancel               = cancel;
   vm.thesaurus            = [];
   vm.searchCandidates     = searchCandidates;
   vm.candidate            = {};
   vm.candidate.current    = 0;
   vm.candidate.size       = 20;
   vm.candidateTotal       = 0;
   vm.pageChanged          = pageChanged;
   vm.selectedCandidates   = [];
   vm.vacancyIdToGoBack    = $state.params.vacancyIdToGoBack;

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

   (function _initData() {
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS)
         .then(topics => set(vm, 'thesaurus', topics));
      vm.candidates = LocalStorageService.get('candidates') || [];
      vm.candidate = LocalStorageService.get('candidate') || {};
      vm.candidate.minAge  = vm.slider.min;
      vm.candidate.maxAge  = vm.slider.max;
   }());

   function pageChanged(newPage) {
      vm.candidate.current = newPage - 1;
      searchCandidates();
   };

   function searchCandidates() {
      SearchService.getCandidates(vm.candidate).then(response => {
         forEach(response.candidate, (cand) => {
            cand.isToogled = vm.isCandidateWasToogled(cand.id);
         });
         vm.candidates = response;
         _setToStorage();
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
      LocalStorageService.set('candidate', vm.candidate);
      LocalStorageService.set('candidates', vm.candidates);
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
