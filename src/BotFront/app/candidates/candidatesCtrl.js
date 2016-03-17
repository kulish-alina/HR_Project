export
default
function CandidatesController($scope, $http) {
    'ngInject';

    var vm = $scope;

    vm.getCandidates = function() {
        $http({
            method: 'get',
            url: 'http://localhost:53031/api/Candidates/',
        }).then(function successCallback(response) {
                vm.candidates = response.data;
            },
            function errorCallback(response) {
                console.log(response.status)
            });
    }
}