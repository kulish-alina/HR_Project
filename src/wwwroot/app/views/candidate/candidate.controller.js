export default function CandidateController($scope, CandidateService, LoggerService) {
   'ngInject';

   var urlId = 'Candidates/Put';
   var vm = $scope;
   vm.candidate = {};

    function addCandidate(firstName, middleName, lastName, industry, source, positionDesired, salaryDesired) {
        vm.candidate = {};
        vm.candidate.personalInfo = {};
        vm.candidate.workInfo = {};
        vm.candidate.personalInfo.firstName = firstName;
        vm.candidate.personalInfo.middleName = middleName;
        vm.candidate.personalInfo.lastName = lastName;
        vm.candidate.industry = industry;
        vm.candidate.workInfo.positionDesired = positionDesired;
        vm.candidate.workInfo.salaryDesired = salaryDesired;
        CandidateService.saveCandidate(vm.candidate).catch(_onError);
    }

    function _onError(message) {
        if (message.status === -1) {
            LoggerService.error(new Date(), 'You cannot get a response from the server');
        }
   }


   vm.addCandidate = addCandidate;
}
