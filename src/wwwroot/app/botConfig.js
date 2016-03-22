import homeTemplate from './views/home/home.view.html';
import candidatesTemplate from './views/candidates/candidates.view.html';
import candidateTemplate from './views/candidate/candidate.view.html';
import vacanciesTemplate from './views/vacancies/vacancies.view.html';

import candidatesController from './views/candidates/candidates.controller';
import candidateController from './views/candidate/candidate.controller';
import vacanciesController from './views/vacancies/vacancies.controller';

export default function _config($stateProvider, $urlRouterProvider, $locationProvider) {
   $locationProvider.html5Mode({
      enabled: true,
      requireBase: false
   });

   $stateProvider
      .state('home', {
         url: '/home',
         template: homeTemplate
      })
      .state('candidates', {
         url: '/candidates',
         template: candidatesTemplate,
         controller: candidatesController
      })
      .state('vacancies', {
         url: '/vacancies',
         template: vacanciesTemplate,
         controller: vacanciesController
      })
      .state('candidate', {
         url: '/candidate',
         template: candidateTemplate,
         controller: candidateController
      })

   $urlRouterProvider.otherwise('home');
}
