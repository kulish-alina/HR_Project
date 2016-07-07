export default function authRunner(
   $rootScope,
   SessionService) {
   'ngInject';
   $rootScope.$on('$stateChangeStart', SessionService.validateAccess);
};
