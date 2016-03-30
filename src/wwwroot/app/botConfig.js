import homeTemplate from './views/home/home.view.html';

import candidatesTemplate from './views/candidates/candidates.view.html';
import candidateTemplate from './views/candidate/candidate.view.html';
import vacanciesTemplate from './views/vacancies/vacancies.view.html';
import vacancyTemplate from './views/vacancy/vacancy.view.html';

import candidatesController from './views/candidates/candidates.controller';
import candidateController from './views/candidate/candidate.controller';
import vacanciesController from './views/vacancies/vacancies.controller';
import vacancyController from './views/vacancy/vacancy.controller';

import translationsEn from './translations/translationsEn.json';
import translationsRu from './translations/translationsRu.json';

export default function
   _config($stateProvider, $urlRouterProvider, $locationProvider, $translateProvider) {
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
      .state('vacancy', {
         url: '/vacancy',
         template: vacancyTemplate,
         controller: vacancyController
      })

   $urlRouterProvider.otherwise('home');
   $translateProvider
      .translations('en', translationsEn)
      .translations('ru', translationsRu)
      .preferredLanguage('en');
}
