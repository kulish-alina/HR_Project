export default function CandidateController($scope, CandidateService, $validation) {
   'ngInject';

   var vm = $scope;
   vm.submit = _submit;
   vm.reset = _reset;

   function _onError(message) {
      vm.errorMessage = 'Sorry! Some error occurred';
   }

   function _submit(form) {
      $validation.validate(form);
      if ($validation.checkValid(form)) {
         CandidateService.saveCandidate(vm.candidate).catch(_onError);
      }
   }

   function _reset(form) {
      $validation.reset(form);
   }
}
