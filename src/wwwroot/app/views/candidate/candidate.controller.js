export default function CandidateController($scope, CandidateService) {
    'ngInject';

    var vm = $scope;
    vm.candidate = {};
    vm.addCandidate = addCandidate;

    function addCandidate(firstName, middleName, lastName, industry, source, positionDesired, salaryDesired) {
        vm.candidate.personalInfo.firstName = firstName;
		  vm.candidate.personalInfo.middleName = middleName;
        vm.candidate.personalInfo.lastName = lastName;
        vm.candidate.industry = industry;
		  vm.candidate.source = source;
		  vm.candidate.workInfo.positionDesired = positionDesired;
		  vm.candidate.workInfo.salaryDesired = salaryDesired;
        CandidateService.addCandidate(vm.candidate);
    }

    function editCandidate() {
		 CandidateService.editCandidate();
    }
}