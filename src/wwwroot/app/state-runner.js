export default function StateRunner($rootScope, $state, $stateParams) {
   $rootScope.$state = $state;
   $rootScope.$stateParams = $stateParams;
}
