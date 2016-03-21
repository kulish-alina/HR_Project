export default function CandidatesController($scope, CandidateService) {
    'ngInject';

    var vm = $scope;
	
    vm.getCandidates = getCandidates;
	
	 vm.getCandidate = getCandidate;

    function getCandidates() {
        CandidateService.getCandidates().then(value => vm.candidates = value);
    }
	 
	function getCandidate(candidateId){
		CandidateService.getCandidate(candidateId).then(value => vm.candidates = value);
	}
}