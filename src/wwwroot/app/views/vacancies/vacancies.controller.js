export default function VacanciesController($scope, $http) {
    'ngInject';

    var vm = $scope;

    var urlId = "Vacancies";
    vm.Vacancies = getVacancies;

    function getVacancies() {
        VacancyService.getVacancies(urlId).then(value => vm.vacancies = value);
    }
}