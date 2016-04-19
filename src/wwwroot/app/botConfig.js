import homeTemplate from './views/home/home.view.html';

import candidatesTemplate from './views/candidates/candidates.view.html';
import candidateTemplate from './views/candidate/candidate.view.html';
import vacanciesTemplate from './views/vacancies/vacancies.view.html';
import vacancyTemplate from './views/vacancy/vacancy.view.html';
import settingsTemplate from './views/settings/settings.view.html';
import profileTemplate from './views/settings/profile/profile.view.html';
import membersTemplate from './views/settings/members/members.view.html';
import rolesTemplate from './views/settings/roles/roles.view.html';
import recruitingTemplate from './views/settings/recruiting/recruiting.view.html';

import candidatesController from './views/candidates/candidates.controller';
import candidateController from './views/candidate/candidate.controller';
import vacanciesController from './views/vacancies/vacancies.controller';
import vacancyController from './views/vacancy/vacancy.controller';
import profileController from './views/settings/profile/profile.controller';
import membersController from './views/settings/members/members.controller';
import rolesController from './views/settings/roles/roles.controller';
import recruitingController from './views/settings/recruiting/recruiting.controller';

import translationsEn from './translations/translationsEn.json';
import translationsRu from './translations/translationsRu.json';

import context from './context';

export default function _config(
   $stateProvider,
   $urlRouterProvider,
   $locationProvider,
   $translateProvider,
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
      .state('settings', {
         url: '/settings',
         template: settingsTemplate
      })
      .state('profile', {
         parent: 'settings',
         template: profileTemplate,
         controller: profileController
      })
      .state('members', {
         parent: 'settings',
         template: membersTemplate,
         controller: membersController
      })
      .state('roles', {
         parent: 'settings',
         template: rolesTemplate,
         controller: rolesController
      })
      .state('recruiting', {
         parent: 'settings',
         template: recruitingTemplate,
         controller: recruitingController
      })

   $urlRouterProvider.otherwise('home');
   $translateProvider
      .translations('en', translationsEn)
      .translations('ru', translationsRu)
      .preferredLanguage(context.defaultLang);

   LoggerServiceProvider.changeLogLevel(context.logLevel);
   HttpServiceProvider.changeApiUrl(context.serverUrl);
}
