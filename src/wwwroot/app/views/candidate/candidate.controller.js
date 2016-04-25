export default function CandidateController(
   $scope,
   $translate,
   CandidateService,
   ValidationService) {
   'ngInject';

   const vm = $scope;
   vm.saveCandidate = saveCandidate;

   function _onError() {
      vm.errorMessage = $translate.instant('CANDIDATE.ERROR');
   }

   function saveCandidate(form) {
      if (ValidationService.validate(form)) {
         CandidateService.saveCandidate(vm.candidate).catch(_onError);
      }
   }
}
