export default function CandidatesController($scope, CandidateService) {
   'ngInject';
   var urlId = 'Candidates/All';
   var vm = $scope;
   function getCandidates() {
      CandidateService.getCandidates(urlId).then(value => vm.candidates = value);
   }

   vm.getCandidates = getCandidates;
}
