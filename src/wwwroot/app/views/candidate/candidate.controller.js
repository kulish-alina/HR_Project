export default function CandidateController($scope, CandidateService) {
   'ngInject';

   var urlId = 'Candidates/Put';
   var vm = $scope;
   vm.candidate = {};

   function addCandidate(firstName, lastName, gender) {
      vm.candidate.firstName = firstName;
      vm.candidate.lastName = lastName;
      vm.candidate.gender = gender;
      var candidateJson = JSON.stringify(vm.candidate);
      CandidateService.addCandidate(urlId, candidateJson);
   }


   vm.addCandidate = addCandidate;
}
