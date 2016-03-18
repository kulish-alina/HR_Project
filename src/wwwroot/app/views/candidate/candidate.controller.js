export default function CandidateController($scope, CandidateService) {
    'ngInject';

    var vm = $scope;
    vm.candidate = {};
    var urlId = "Candidates/Put";
    vm.addCandidate = addCandidate;

    function addCandidate(firstName, lastName, gender) {
        vm.candidate.firstName = firstName;
        vm.candidate.lastName = lastName;
        vm.candidate.gender = gender;
        var candidateJson = JSON.stringify(vm.candidate);
        CandidateService.addCandidate(urlId, candidateJson);
    }
}