export default function CandidateController(
   $scope,
    $translate,
    CandidateService,
    ValidationService)
{
   'ngInject';

   var vm = $scope;
   vm.submit = _submit;

   function _onError(message) {
   }

   function _submit(form) {
      if (ValidationService.validate(form)) {
         CandidateService.saveCandidate(vm.candidate).catch(_onError);
      }
   }
}
