import angular from 'angular';
import router from 'angular-ui-router';
import translate from 'angular-translate';
import 'foundation/js/foundation-apps.js';
import 'foundation/js/foundation-apps-templates.js';
import 'foundation/css/foundation-apps.min.css';

import config from './botConfig';


import HttpService from './services/httpService';
import CandidateService from './services/candidateService';
import VacancyService from './services/vacancyService';
import LoggerService from './services/loggerService';

var dependencies = [router, 'foundation'];

angular
   .module('bot', dependencies)
   .service('HttpService', HttpService)
   .service('LoggerService', LoggerService)
   .service('CandidateService', CandidateService)
   .service('VacancyService', VacancyService)
   .config(config);
