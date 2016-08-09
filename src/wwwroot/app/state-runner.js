export default function StateRunner($rootScope, $state, $stateParams) {
   'ngInject';
   $rootScope.$state = $state;
   $rootScope.$stateParams = $stateParams;

   $rootScope.pr = function _pr($event) {
      $event.stopPropagation();
   };

   $rootScope.$on('$stateChangeSuccess', function stateReload (ev, to, toParams, from, fromParams) {
      $state.previous = from;
      from.params = fromParams;
   });
}
