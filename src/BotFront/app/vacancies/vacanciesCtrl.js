export default function VacanciesController($scope, $http) {
    'ngInject';

    var vm = $scope;

    vm.getVacancies = function() {
        $http({
            method: 'get',
            url: 'http://localhost:53031/api/Vacancies/',
        }).then(function successCallback(response) {
                vm.vacancies = response.data;
            },
            function errorCallback(response) {
                console.log(response.status)
            });
    }
}