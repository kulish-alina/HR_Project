export default function VacancyController($scope, VacancyService, LoggerService) {
   'ngInject';

   var vm = $scope;
   vm.vacancy = {};

   function _onError(message) {
      if (message.status === -1) {
         LoggerService.error(new Date(), 'You cannot get a response from the server');
      }
   }
   function addVacancy(title, status, location) {
      vm.vacancy.vacancyStatus = {};
      vm.vacancy.location = {};
      vm.vacancy.name = title;
      vm.vacancy.vacancyStatus.title = status;
      vm.vacancy.location.city = location;
      VacancyService.saveVacancy(vm.vacancy).catch(_onError);
   }
   vm.addVacancy = addVacancy;
}
