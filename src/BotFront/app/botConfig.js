import homeTemplate        from './views/home-view.html';
import candidatesTemplate  from './views/candidates-view.html';
import vacanciesTemplate   from './views/vacancies-view.html';

export default function _config($stateProvider, $urlRouterProvider, $locationProvider) {
   $locationProvider.html5Mode({
      enabled     : true,
      requireBase : false
   });

   $stateProvider
      .state('home', {
         url      : '/home',
         template : homeTemplate
      })
      .state('candidates', {
         url      : '/candidates',
         template : candidatesTemplate
      })
      .state('vacancies', {
         url      : '/vacancies',
         template : vacanciesTemplate
      });

   $urlRouterProvider.otherwise('home');
}