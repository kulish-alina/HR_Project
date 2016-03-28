export default function CandidateController($scope, CandidateService, LoggerService) {
   'ngInject';

   var vm = $scope;
   function _onError(message) {
      if (message.status === -1) {
         LoggerService.information('You cannot get a response from the server');
      }
   }
   function saveCandidate() {
      CandidateService.saveCandidate(vm.candidate).catch(_onError);
   }

   vm.saveCandidate = saveCandidate;
}
