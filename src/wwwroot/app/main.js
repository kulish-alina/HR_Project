import angular from 'angular';
import router from 'angular-ui-router';
import translate from 'angular-translate';
import 'angular-validation/dist/angular-validation';
import 'angular-validation/dist/angular-validation-rule';

import config from './botConfig';
import configValidation from './configValidation';

import LoggerProvider   from './services/LoggerProvider';
import HttpProvider     from './services/HttpProvider';

import CandidateService  from './services/candidateService';
import VacancyService    from './services/vacancyService';
import ValidationService from './services/validationService';

import uiMask from 'angular-ui-mask';
import phoneFormatFilter from './filters/PhoneFormatFilter';

var dependencies = [router, translate, uiMask, 'validation', 'validation.rule'];

angular
   .module('bot', dependencies)

   .provider('LoggerService', LoggerProvider)
   .provider('HttpService',   HttpProvider)

   .service('CandidateService',  CandidateService)
   .service('VacancyService',    VacancyService)
   .service('ValidationService', ValidationService)

   .filter('tel', phoneFormatFilter)

   .config(config)
   .config(configValidation);
