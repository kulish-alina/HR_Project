export default function CandidatesController($scope, CandidateService) {
    'ngInject';

    var vm = $scope;
    vm.candidates = [];
    vm.getCandidates = getCandidates;
    vm.getCandidate = getCandidate;
    vm.deleteCandidate = deleteCandidate;
    vm.editCandidate = editCandidate;


    function getCandidates() {
        CandidateService.getCandidates().then(value => vm.candidates = value);
    }

    function getCandidate(candidateId) {
        CandidateService.getCandidate(candidateId).then(value => vm.candidates = [value]);
    }

    function deleteCandidate(candidate) {
        CandidateService.deleteCandidate(candidate);
    }

    function editCandidate(candidate) {
        CandidateService. saveCandidate(candidate);
    }
}