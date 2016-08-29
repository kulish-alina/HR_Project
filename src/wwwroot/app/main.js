import angular      from 'angular';
import ngAnimate    from 'angular-animate';
import router       from 'angular-ui-router';
import translate    from 'angular-translate';

import 'angular-drag-and-drop-lists/angular-drag-and-drop-lists';
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
import './directives/datepickerwrapper/angularjs-datetime-picker.js';
import './directives/datepickerwrapper/angularjs-datetime-picker.scss';

import './ta';
import './main.scss';

import config                        from './bot-config';
import configValidation              from './config-validation';

import LoggerProvider                from './services/LoggerProvider';
import HttpProvider                  from './services/HttpProvider';
import ValidationProvider            from './services/ValidationProvider';
import CandidateService              from './services/CandidateService';
import VacancyService                from './services/VacancyService';
import ThesaurusService              from './services/ThesaurusService';
import UserService                   from './services/UserService';
import SettingsService               from './services/SettingsService';
import RolesService                  from './services/RolesService';
import HttpCacheService              from './services/HttpCacheService';
import UserDialogService             from './services/UserDialogService/UserDialogService';
import FileService                   from './services/FileService';
import EventsService                 from './services/EventsService';
import NoteService                   from './services/NoteService';
import LocalStorageService           from './services/LocalStorageService';
import LoginService                  from './services/LoginService';
import SessionService                from './services/SessionService';
import SearchService                 from './services/SearchService';

import ThesaurusDirective            from './directives/thesaurus/thesaurus';
import ContactInfoDirective          from './directives/contacts/contact-info';
import PhoneInputsDirective          from './directives/phones/phone-inputs';
import CanvasPreviewDirective        from './directives/file-preview/canvas-preview';
import CommentsDirective             from './directives/comments/comments';
import MainMenuDirective             from './directives/main-menu/main-menu';
import SideMenuDirective             from './directives/side-menu/side-menu';
import EventsDirective               from './directives/events/events';
import CandidateInfoDirective        from './directives/candidate-info/candidate-info';
import LoginDirective                from './directives/login/login';
import convertToNumberDirective      from './directives/convertToNumber/convert-to-number';
import languageSkillsDirective       from './directives/language-skills/language-skills';
import EventCalendarDirective    from './directives/event-calendar/event-calendar';
import CandidateVacancyInfoDirective from './directives/candidate-vacancy-info/candidate-vacancy-info';

import uiMask                        from 'angular-ui-mask';

import PhoneFormatFilter             from './filters/PhoneFormatFilter';
import ArrayAsString                 from './filters/ArrayAsString';
import botUrl                        from './filters/botUrl';

import StateRunner                   from './state-runner';
import AuthRunner                    from './runner.auth';

import authInterceptor               from './interceptors/AuthRequestInterceptor';

const dependencies = [
   ngAnimate,
   router,
   translate,
   uiMask,
   pagination,
   'dndLists',
   'rzModule',
   'foundation',
   'validation',
   'validation.rule',
   'angularFileUpload',
   'textAngular',
   'oi.select',
   'srph.age-filter',
   'angularjs-datetime-picker'
];

angular
   .module('bot', dependencies)

   .provider('LoggerService',      LoggerProvider)
   .provider('HttpService',        HttpProvider)
   .provider('ValidationService',  ValidationProvider)
   .service('CandidateService',    CandidateService)
   .service('VacancyService',      VacancyService)
   .service('SettingsService',     SettingsService)
   .service('UserService',         UserService)
   .service('RolesService',        RolesService)
   .service('ThesaurusService',    ThesaurusService)
   .service('HttpCacheService',    HttpCacheService)
   .service('UserDialogService',   UserDialogService)
   .service('FileService',         FileService)
   .service('EventsService',       EventsService)
   .service('NoteService',         NoteService)
   .service('LocalStorageService', LocalStorageService)
   .service('LoginService',        LoginService)
   .service('SessionService',      SessionService)
   .service('SearchService',       SearchService)


   .directive('thesaurus',              ThesaurusDirective.createInstance)
   .directive('contacts',               ContactInfoDirective.createInstance)
   .directive('phones',                 PhoneInputsDirective.createInstance)
   .directive('ngThumb',                CanvasPreviewDirective)
   .directive('comments',               CommentsDirective.createInstance)
   .directive('mainMenu',               MainMenuDirective.createInstance)
   .directive('sideMenu',               SideMenuDirective.createInstance)
   .directive('events',                 EventsDirective.createInstance)
   .directive('convertToNumber',        convertToNumberDirective)
   .directive('candidateInfo',          CandidateInfoDirective.createInstance)
   .directive('login',                  LoginDirective.createInstance)
   .directive('languageSkills',         languageSkillsDirective.createInstance)
   .directive('eventCalendar',    EventCalendarDirective.createInstance)
   .directive('candidateVacancyInfo',   CandidateVacancyInfoDirective.createInstance)

   .filter('tel',                 PhoneFormatFilter)
   .filter('arrayAsString',       ArrayAsString)
   .filter('botUrl',              botUrl)

   .factory('authInterceptor', authInterceptor)

   .run(StateRunner)
   .run(AuthRunner)

   .config(config)
   .config(configValidation);

angular.bootstrap(document.body, [ 'bot' ]);
