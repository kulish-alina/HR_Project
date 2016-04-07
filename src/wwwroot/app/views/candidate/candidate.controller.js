export default function CandidateController($scope, CandidateService) {
   'ngInject';

   var vm = $scope;
   vm.saveCandidate = saveCandidate;

   function saveCandidate() {
      CandidateService.saveCandidate(vm.candidate).catch(_onError);
   }

   function _onError(message) {
      vm.errorMessage = 'Sorry! Some error occurred';
   }
}
