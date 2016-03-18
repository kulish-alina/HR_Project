export default function CandidatesController($scope, CandidateService) {
    'ngInject';

    var vm = $scope;
	
    var urlId = "Candidates/All";
    vm.getCandidates = getCandidates;

    function getCandidates() {
        CandidateService.getCandidates(urlId).then(value => vm.candidates = value);
    }
}