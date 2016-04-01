import angular from 'angular';
import router from 'angular-ui-router';
import translate from 'angular-translate';

import config from './botConfig';

import HttpService      from './services/httpService';
import CandidateService from './services/candidateService';
import VacancyService   from './services/vacancyService';
import LoggerProvider   from './services/LoggerProvider';

var dependencies = [router, translate];

angular
   .module('bot', dependencies)

   .provider('LoggerService', LoggerProvider)

   .service('HttpService', HttpService)
   .service('CandidateService', CandidateService)
   .service('VacancyService', VacancyService)

   .config(config);
