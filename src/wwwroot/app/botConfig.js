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

import {
   reduce,
   isFunction,
   constant,
   method
} from 'lodash';

import context from './context';

export default function _config(
   $stateProvider,
   $urlRouterProvider,
   $locationProvider,
   $translateProvider,
   $validationProvider,
   LoggerServiceProvider,
   HttpServiceProvider
) {

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
      .preferredLanguage(context.defaultLang);

   LoggerServiceProvider.changeLogLevel(context.logLevel);
   HttpServiceProvider.changeApiUrl(context.serverUrl);

   /*------validation------*/
   const _true = constant(true);
   const methods = ['minlength', 'maxlength', 'email', 'number', 'url'];
   const _origin = reduce(methods, (memo, nameOfMethod) => {
      memo[nameOfMethod] = _wrap($validationProvider.getExpression(nameOfMethod));
      return memo;
   }, {});

   function _wrap(validation) {
      return isFunction(validation) ? validation : (str) => validation.test(str);
   }
   const validationExpression = reduce(methods, (memo, nameOfMethod) => {
      memo[nameOfMethod] = (value, scope, element, attrs, param) => {
         let orig = _origin[nameOfMethod];
         return !value ? _true : orig(value, scope, element, attrs, param);
      };
      return memo;
   }, {});
   validationExpression.title = function(value) {
      const minTitleLength = 3;
      const maxTitleLength = 50;
      return !!value ? value.length <= maxTitleLength && value.length >= minTitleLength : true;
   }
   $validationProvider.showSuccessMessage = false;
   $validationProvider
      .setDefaultMsg({ en: translationsEn.VALIDATION,
                       ru: translationsRu.VALIDATION}[context.defaultLang || 'en'])
      .setExpression(validationExpression)
      .setValidMethod('blur');
}

