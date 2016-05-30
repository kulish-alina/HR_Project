import homeTemplate from './views/home/home.view.html';

import candidatesTemplate  from './views/candidates/candidates.view.html';
import candidateTemplate   from './views/candidate/candidate.view.html';
import vacanciesTemplate   from './views/vacancies/vacancies.view.html';
import vacancyTemplate     from './views/vacancy/vacancy.view.html';
import settingsTemplate    from './views/settings/settings.view.html';
import thesaurusesTemplate from './views/settings/thesauruses/thesauruses.view.html';
import profileTemplate     from './views/settings/profile/profile.view.html';
import profileEditTemplate from './views/settings/profile/profileEdit.view.html';
import membersTemplate     from './views/settings/members/members.view.html';
import rolesTemplate       from './views/settings/roles/roles.view.html';

import homeController         from './views/home/home.controller';
import candidatesController   from './views/candidates/candidates.controller';
import candidateController    from './views/candidate/candidate.controller';
import vacanciesController    from './views/vacancies/vacancies.controller';
import vacancyController      from './views/vacancy/vacancy.controller';
import settingsController     from './views/settings/settings.controller';
import thesaurusesController  from './views/settings/thesauruses/thesauruses.controller';
import profileController      from './views/settings/profile/profile.controller';
import membersController      from './views/settings/members/members.controller';
import rolesController        from './views/settings/roles/roles.controller';

import translationsEn from './translations/translations-en.json';
import translationsRu from './translations/translations-ru.json';

import context                from './context';

export default function _config(
   $stateProvider,
   $urlRouterProvider,
   $locationProvider,
   $translateProvider,
   $compileProvider,
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
         template: homeTemplate,
         controller: homeController
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
         controller: vacancyController,
         params: {
            _data: null
         }
      })
      .state('settings', {
         url: '/settings',
         template: settingsTemplate,
         controller: settingsController,
         data: {asEdit: false}
      })
      .state('profile', {
         url: '/profile',
         parent: 'settings',
         template: profileTemplate,
         controller: profileController,
         data: {asEdit: false}
      })
      .state('profile.edit', {
         url: '/edit',
         parent: 'profile',
         template: profileEditTemplate,
         data: {asEdit: true}
      })
      .state('members', {
         url: '/members',
         parent: 'settings',
         template: membersTemplate,
         controller: membersController,
         data: {asEdit: true}
      })
      .state('roles', {
         url: '/roles',
         parent: 'settings',
         template: rolesTemplate,
         controller: rolesController,
         data: {asEdit: true}
      })
      .state('thesauruses', {
         url: '/recruiting',
         parent: 'settings',
         template: thesaurusesTemplate,
         controller: thesaurusesController
      });

   $urlRouterProvider.otherwise('home');

   $translateProvider
      .useSanitizeValueStrategy('sanitize')
      .translations('en', translationsEn)
      .translations('ru', translationsRu)
      .preferredLanguage(context.defaultLang);

   $compileProvider.aHrefSanitizationWhitelist(/^\s*(https?|ftp|mailto|file|skype|tel):/);

   LoggerServiceProvider.changeLogLevel(context.logLevel);
   HttpServiceProvider.changeApiUrl(context.serverUrl + context.apiSuffix);
}
