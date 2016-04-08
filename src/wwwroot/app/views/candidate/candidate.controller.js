export default function CandidateController($scope, CandidateService, ValidationService) {
   'ngInject';

   var vm = $scope;
   vm.submit = _submit;

   function _onError(message) {
      vm.errorMessage = 'Sorry! Some error occurred';
   }

   function _submit(form) {
      ValidationService.validate(form)
         .then(() => CandidateService.saveCandidate(vm.candidate).catch(_onError));
   }
}
