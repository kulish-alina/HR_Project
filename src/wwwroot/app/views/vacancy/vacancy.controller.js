export default function VacancyController($scope, VacancyService) {
   'ngInject';

   var vm = $scope;
   vm.saveVacancy = saveVacancy;

   function saveVacancy() {
      VacancyService.saveVacancy(vm.vacancy).catch(_onError);
   }

   function _onError(message) {
      vm.errorMessage = 'Sorry! Some error occurred';
   }
}
