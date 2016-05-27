export default function StateRunner($rootScope, $state, $stateParams) {
   $rootScope.$state = $state;
   $rootScope.$stateParams = $stateParams;

   $rootScope.pr = function _pr($event) {
      $event.stopPropagation();
   };
}
