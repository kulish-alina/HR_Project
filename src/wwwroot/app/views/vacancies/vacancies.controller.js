export default function VacanciesController($scope, $http) {
    'ngInject';

    var vm = $scope;

    var additionalUrl = "Vacancies";
    vm.Vacancies = getVacancies;

    function getVacancies() {
        VacancyService.getVacancies(additionalUrl).then(value => vm.vacancies = value);
    }
}