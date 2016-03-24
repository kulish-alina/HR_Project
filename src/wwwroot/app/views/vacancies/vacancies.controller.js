export default function VacanciesController($scope, VacancyService) {
   'ngInject';

   var urlId = 'Vacancies';
   var vm = $scope;

   function getVacancies() {
      VacancyService.getVacancies(urlId).then(value => vm.vacancies = value);
   }

   vm.Vacancies = getVacancies;
}
