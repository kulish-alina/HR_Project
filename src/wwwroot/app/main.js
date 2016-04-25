import angular    from 'angular';
import router     from 'angular-ui-router';
import translate  from 'angular-translate';

import 'angular-validation/dist/angular-validation';
import 'angular-validation/dist/angular-validation-rule';

import 'angular-file-upload/dist/angular-file-upload';

import 'foundation-apps/dist/js/foundation-apps';
import 'foundation-apps/dist/js/foundation-apps-templates';

import 'foundation-icons/foundation_icons_general/sass/general_foundicons.scss';
import 'foundation-icons/foundation_icons_social/sass/social_foundicons.scss';
import 'foundation-icon-fonts/foundation-icons.css';
import 'oi.select/dist/select';
import 'oi.select/dist/select-tpls';
import 'oi.select/dist/select.css';

import './ta';
import './main.scss';

import config from './bot-config';
import configValidation from './config-validation';

import LoggerProvider   from './services/LoggerProvider';
import HttpProvider     from './services/HttpProvider';

import CandidateService  from './services/CandidateService';
import VacancyService    from './services/VacancyService';
import ValidationService from './services/ValidationService';
import ThesaurusService   from './services/thesaurusService';

import ThesaurusDirective   from './directives/thesaurus/thesaurus';

import uiMask from 'angular-ui-mask';
import phoneFormatFilter from './filters/PhoneFormatFilter';

import DatePickerDirective from './directives/datepickerwrapper/DatePickerWrapperDirective';

const dependencies = [
   router,
   translate,
   uiMask,
   'foundation',
   'validation',
   'validation.rule',
   '720kb.datepicker',
   'angularFileUpload',
   'textAngular',
   'oi.select'
];

angular
   .module('bot', dependencies)

   .provider('LoggerService', LoggerProvider)
   .provider('HttpService',   HttpProvider)

   .service('CandidateService',  CandidateService)
   .service('VacancyService',    VacancyService)
   .service('ValidationService', ValidationService)
   .service('ThesaurusService', ThesaurusService)

   .directive('thesaurus', ThesaurusDirective.createInstance)

   .filter('tel', phoneFormatFilter)

   .directive('date', DatePickerDirective.createInstance)

   .config(config)
   .config(configValidation);
