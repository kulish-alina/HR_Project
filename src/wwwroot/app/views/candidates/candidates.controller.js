export default function CandidatesController($scope, $http) {
   'ngInject';

   var vm = $scope;

   vm.getCandidates = function() {
      $http({
         method: 'get',
         url: 'http://localhost:53031/api/Candidates/GetCandidates'
      }).then((response) => {
         vm.candidates = response.data;
      });
   }
   /*$http.jsonp('http://localhost:53031/api/Candidates/GetCandidates').then(() => {
           console.log(lol);
       })*/
}
