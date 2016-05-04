import homeTemplate from './views/home/home.view.html';

import candidatesTemplate  from './views/candidates/candidates.view.html';
import candidateTemplate   from './views/candidate/candidate.view.html';
import vacanciesTemplate   from './views/vacancies/vacancies.view.html';
import vacancyTemplate     from './views/vacancy/vacancy.view.html';
import thesaurusesTemplate from './views/thesauruses/thesauruses.view.html';


import candidatesController   from './views/candidates/candidates.controller';
import candidateController    from './views/candidate/candidate.controller';
import vacanciesController    from './views/vacancies/vacancies.controller';
import vacancyController      from './views/vacancy/vacancy.controller';
import thesaurusesController from './views/thesauruses/thesauruses.controller';

import translationsEn from './translations/translations-en.json';
import translationsRu from './translations/translations-ru.json';

import context from './context';

export default function _config(
   $stateProvider,
   $urlRouterProvider,
   $locationProvider,
   $translateProvider,
   LoggerServiceProvider,
   HttpServiceProvider
) {
   'ngInject';

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
      .state('thesauruses', {
         url: '/thesauruses',
         template: thesaurusesTemplate,
         controller: thesaurusesController
      });

   $urlRouterProvider.otherwise('home');
   $translateProvider
      .translations('en', translationsEn)
      .translations('ru', translationsRu)
      .preferredLanguage(context.defaultLang);

   LoggerServiceProvider.changeLogLevel(context.logLevel);
   HttpServiceProvider.changeApiUrl(context.serverUrl);
}
