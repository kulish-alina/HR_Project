import angular from 'angular';
import router from 'angular-ui-router';

import config from './botConfig';

import HttpService from './services/httpService';
import CandidateService from './services/candidateService';
import VacancyService from './services/vacancyService';

var dependencies = [ router ];

angular
   .module('bot', dependencies)

   .service('HttpService', HttpService)
   .service('CandidateService', CandidateService)
   .service('VacancyService', VacancyService)

   .config(config);
