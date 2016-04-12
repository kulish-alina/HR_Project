import angular from 'angular';
import router from 'angular-ui-router';
import translate from 'angular-translate';

import config from './botConfig';

import LoggerProvider   from './services/LoggerProvider';
import HttpProvider     from './services/HttpProvider';

import CandidateService from './services/candidateService';
import VacancyService   from './services/vacancyService';

import uiMask from 'angular-ui-mask';
import phoneFormatFilter from './filters/PhoneFormatFilter';

var dependencies = [router, translate, uiMask];

angular
   .module('bot', dependencies)

   .provider('LoggerService', LoggerProvider)
   .provider('HttpService',   HttpProvider)

   .service('CandidateService',  CandidateService)
   .service('VacancyService',    VacancyService)

   .filter('tel', phoneFormatFilter)

   .config(config);
