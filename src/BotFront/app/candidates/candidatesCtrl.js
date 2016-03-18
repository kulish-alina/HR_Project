export default function CandidatesController($scope, $http) {
    'ngInject';

    var vm = $scope;

    vm.getCandidates = function() {
        $http({
            method : 'delete',
            url : 'http://localhost:53031/api/Candidates/All',
        }).then((response) => {
            vm.candidates = response.data;
        });
    }
}