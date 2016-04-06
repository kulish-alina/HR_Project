import angular from 'angular';
import router from 'angular-ui-router';
import translate from 'angular-translate';

import 'foundation-apps/dist/js/foundation-apps.js';
import 'foundation-apps/dist/js/foundation-apps-templates.js';

import 'foundation-icons/foundation_icons_social/sass/social_foundicons.scss';
import 'foundation-icons/foundation_icons_general/sass/general_foundicons.scss';

import './main.scss';

import config from './botConfig';

import HttpService from './services/httpService';
import CandidateService from './services/candidateService';
import VacancyService from './services/vacancyService';
import LoggerService from './services/loggerService';

var dependencies = [router, 'foundation', translate];

angular
   .module('bot', dependencies)
   .service('HttpService', HttpService)
   .service('LoggerService', LoggerService)
   .service('CandidateService', CandidateService)
   .service('VacancyService', VacancyService)
   .config(config);
