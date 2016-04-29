import angular   from 'angular';
import router    from 'angular-ui-router';
import translate from 'angular-translate';
import 'angular-validation/dist/angular-validation';
import 'angular-validation/dist/angular-validation-rule';

import 'foundation-apps/dist/js/foundation-apps.js';
import 'foundation-apps/dist/js/foundation-apps-templates.js';

import 'foundation-icons/foundation_icons_general/sass/general_foundicons.scss';
import 'foundation-icons/foundation_icons_social/sass/social_foundicons.scss';

import './main.scss';

import config           from './bot-config';
import configValidation from './config-validation';

import LoggerProvider   from './services/LoggerProvider';
import HttpProvider     from './services/HttpProvider';
import ValidationProvider  from './services/ValidationProvider';

import CandidateService  from './services/CandidateService';
import VacancyService    from './services/VacancyService';
import UserService       from './services/userService';
import ThesaurusService  from './services/thesaurusService';
import SettingsService   from './services/SettingsService';

import ThesaurusDirective   from './directives/thesaurus/thesaurus';
import DatePickerDirective  from './directives/datepickerwrapper/DatePickerWrapperDirective';
import ContactInfoDirective from './directives/contacts/ContactInfo';

import uiMask from 'angular-ui-mask';
import phoneFormatFilter from './filters/PhoneFormatFilter';

const dependencies = [

   router,
   translate,
   uiMask,
   'foundation',
   'validation',
   'validation.rule',
   '720kb.datepicker'
];

angular
   .module('bot', dependencies)

   .provider('LoggerService',     LoggerProvider)
   .provider('HttpService',       HttpProvider)
   .provider('ValidationService', ValidationProvider)

   .service('CandidateService',  CandidateService)
   .service('VacancyService',    VacancyService)
   .service('UserService',       UserService)
   .service('SettingsService',   SettingsService)
   .service('ThesaurusService',  ThesaurusService)

   .directive('thesaurus', ThesaurusDirective.createInstance)
   .directive('date',      DatePickerDirective.createInstance)
   .directive('contacts',  ContactInfoDirective.createInstance)

   .filter('tel', phoneFormatFilter)

   .run(['$rootScope', '$state', '$stateParams',
   ($rootScope, $state, $stateParams) => {
      $rootScope.$state = $state;
      $rootScope.$stateParams = $stateParams;
   }])

   .config(config)
   .config(configValidation);
