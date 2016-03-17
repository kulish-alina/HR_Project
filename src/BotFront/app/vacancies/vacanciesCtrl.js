export default function VacanciesController($scope, $http) {
    'ngInject';

    var vm = $scope;

    vm.getVacancies = function() {
        $http.get("").success(
            function(response) {
                vm.vacancies = response;
            })
    }
}