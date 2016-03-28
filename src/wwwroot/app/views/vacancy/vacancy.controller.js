export default function VacancyController($scope, VacancyService, LoggerService) {
   'ngInject';

   var vm = $scope;

   function _onError(message) {
      if (message.status === -1) {
         LoggerService.information('You cannot get a response from the server');
      }
   }
   function saveVacancy() {
      VacancyService.saveVacancy(vm.vacancy).catch(_onError);
   }
   vm.saveVacancy = saveVacancy;
}
