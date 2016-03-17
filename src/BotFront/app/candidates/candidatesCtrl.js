export default function CandidatesController($scope, $http) {
    'ngInject';

    var vm = $scope;

    vm.getCandidates = function() {
        $http({
            method : 'delete',
            url : 'http://localhost:53031/api/Candidates/Remove/',
            data  : '5',
            headers: { 'Access-Control-Allow-Origin' : '*',
                    'Access-Control-Allow-Methods' : 'GET, PUT, POST, DELETE, HEAD',
                     'Access-Control-Allow-Headers' : 'Origin, X-Requested-With, Content-Type, Accept'},
            withCredentials : true
        }).then((response) => {
            vm.candidates = response.data;
        });
    }
    /*$http.jsonp('http://localhost:53031/api/Candidates/GetCandidates').then(() => {
            console.log(lol);
        })*/
}