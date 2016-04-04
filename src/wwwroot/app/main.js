import angular from 'angular';
import router from 'angular-ui-router';
import translate from 'angular-translate';
import 'angular-validation/dist/angular-validation';
import 'angular-validation/dist/angular-validation-rule';

import config from './botConfig';

import LoggerProvider   from './services/LoggerProvider';
import HttpProvider     from './services/HttpProvider';

import CandidateService from './services/candidateService';
import VacancyService   from './services/vacancyService';

var dependencies = [router, translate, 'validation', 'validation.rule'];

angular
   .module('bot', dependencies)

   .provider('LoggerService', LoggerProvider)
   .provider('HttpService',   HttpProvider)

   .service('CandidateService',  CandidateService)
   .service('VacancyService',    VacancyService)

   .config(config);
