export default function CandidatesController($scope, CandidateService, LoggerService) {
   'ngInject';
   var urlId = 'Candidates/All';
   var vm = $scope;
   vm.candidates = [];
   vm.getCandidate = getCandidate;
   vm.deleteCandidate = deleteCandidate;
   vm.editCandidate = editCandidate;
   vm.getCandidates = getCandidates;

   function getCandidates() {
      CandidateService.getCandidates().then(value => vm.candidates = value).
        catch(_onError);
   }

   function getCandidate(candidateId) {
      CandidateService.getCandidate(candidateId).then(value => vm.candidates = [ value ]).
      catch(_onError);
   }

   function deleteCandidate(candidate) {
      CandidateService.deleteCandidate(candidate);
   }

   function editCandidate(candidate) {
      CandidateService.saveCandidate(candidate).catch(_onError);
   }

   function _onError(message) {
      vm.errorMessage = 'Sorry! Some error occurred';
   }
}
