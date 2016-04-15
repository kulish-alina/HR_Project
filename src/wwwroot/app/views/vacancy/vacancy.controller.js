export default function VacancyController($scope, VacancyService, ValidationService) {
   'ngInject';

   var vm = $scope;
   vm.saveVacancy = saveVacancy;

   function saveVacancy(form) {
      debugger;
      if (ValidationService.validate(form)) {
         VacancyService.saveVacancy(vm.vacancy).catch(_onError);
      }
   }

   function _onError(message) {
      vm.errorMessage = 'Sorry! Some error occurred';
   }
}
