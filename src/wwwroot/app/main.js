import angular    from 'angular';
import router     from 'angular-ui-router';
import translate  from 'angular-translate';

import 'angular-validation/dist/angular-validation';
import 'angular-validation/dist/angular-validation-rule';

import 'angular-file-upload/dist/angular-file-upload.min';
import pagination from 'angular-utils-pagination';

import 'foundation-apps/dist/js/foundation-apps';
import 'foundation-apps/dist/js/foundation-apps-templates';

import 'foundation-icons/foundation_icons_general/sass/general_foundicons.scss';
import 'foundation-icons/foundation_icons_social/sass/social_foundicons.scss';
import 'foundation-icon-fonts/foundation-icons.css';
import 'oi.select/dist/select.min';
import 'oi.select/dist/select-tpls.min';
import 'oi.select/dist/select.min.css';
import 'angularjs-slider/dist/rzslider.min';
import 'angularjs-slider/dist/rzslider.min.css';

import 'angular-srph-age-filter/angular-age-filter';

import './ta';
import './main.scss';

import config           from './bot-config';
import configValidation from './config-validation';

import LoggerProvider     from './services/LoggerProvider';
import HttpProvider       from './services/HttpProvider';
import ValidationProvider from './services/ValidationProvider';

import CandidateService    from './services/CandidateService';
import VacancyService      from './services/VacancyService';
import ThesaurusService    from './services/ThesaurusService';
import UserService         from './services/UserService';
import SettingsService     from './services/SettingsService';
import RolesService        from './services/RolesService';
import HttpCacheService    from './services/HttpCacheService';
import UserDialogService   from './services/UserDialogService/UserDialogService';
import FileService         from './services/FileService';

import ThesaurusDirective     from './directives/thesaurus/thesaurus';
import DatePickerDirective    from './directives/datepickerwrapper/DatePickerWrapperDirective';
import ContactInfoDirective   from './directives/contacts/contact-info';
import PhoneInputsDirective   from './directives/phones/phone-inputs';
import CanvasPreviewDirective from './directives/file-preview/canvas-preview';
import MainMenuDirective      from './directives/main-menu/main-menu';
import SideMenuDirective      from './directives/side-menu/side-menu';

import uiMask from 'angular-ui-mask';

import PhoneFormatFilter   from './filters/PhoneFormatFilter';
import ArrayAsString       from './filters/ArrayAsString';
import botUrl              from './filters/botUrl';

import StateRunner from './state-runner';

const dependencies = [
   router,
   translate,
   uiMask,
   pagination,
   'rzModule',
   'foundation',
   'validation',
   'validation.rule',
   '720kb.datepicker',
   'angularFileUpload',
   'textAngular',
   'oi.select',
   'srph.age-filter'
];

angular
   .module('bot', dependencies)

   .provider('LoggerService',     LoggerProvider)
   .provider('HttpService',       HttpProvider)
   .provider('ValidationService', ValidationProvider)

   .service('CandidateService',    CandidateService)
   .service('VacancyService',      VacancyService)
   .service('SettingsService',     SettingsService)
   .service('UserService',         UserService)
   .service('RolesService',        RolesService)
   .service('ThesaurusService',    ThesaurusService)
   .service('HttpCacheService',    HttpCacheService)
   .service('UserDialogService',   UserDialogService)
   .service('FileService',         FileService)


   .directive('thesaurus',      ThesaurusDirective.createInstance)
   .directive('date',           DatePickerDirective.createInstance)
   .directive('contacts',       ContactInfoDirective.createInstance)
   .directive('phones',         PhoneInputsDirective.createInstance)
   .directive('ngThumb',        CanvasPreviewDirective)
   .directive('mainMenu',       MainMenuDirective.createInstance)
   .directive('sideMenu',       SideMenuDirective.createInstance)
   .directive('convertToNumber', () => {
      return {
         require: 'ngModel',
         link: (scope, element, attrs, ngModel) => {
            ngModel.$parsers.push(val => parseInt(val, 10));
            ngModel.$formatters.push(val => `${val}`);
         }
      };
   })

   .filter('tel',               PhoneFormatFilter)
   .filter('arrayAsString',     ArrayAsString)
   .filter('botUrl',            botUrl)

   .run(StateRunner)

   .config(config)
   .config(configValidation);

angular.bootstrap(document.body, [ 'bot' ]);
