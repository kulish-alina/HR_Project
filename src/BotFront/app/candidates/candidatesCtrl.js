export default function CandidatesController($scope, $http) {
    'ngInject';

    var vm = $scope;

    vm.getCandidates = function() {
        $http.get("").success(
            function(response) {
                vm.candidates = response;
            })
    }
}