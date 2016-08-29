export default function StateRunner($rootScope, $state, $stateParams) {
   'ngInject';
   $rootScope.$state = $state;
   $rootScope.$stateParams = $stateParams;

   $rootScope.pr = function _pr($event) {
      $event.stopPropagation();
   };

   $rootScope.$on('$stateChangeSuccess', function stateReload (ev, to, toParams, from, fromParams) {
      if ((from.name === 'home' || from.name === 'vacancies')  && to.name === 'vacancyView') {
         $state.parentView = from;
         from.params = fromParams;
      }
      if (from.name === 'candidates' && to.name === 'candidateProfile') {
         $state.parentView = from;
         from.params = fromParams;
      }
      $state.previous = from;
      from.params = fromParams;
   });
}
