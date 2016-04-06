import angular from 'angular';
import router from 'angular-ui-router';
import translate from 'angular-translate';

import 'foundation-apps/dist/js/foundation-apps.js';
import 'foundation-apps/dist/js/foundation-apps-templates.js';

import 'foundation-icons/foundation_icons_social/sass/social_foundicons.scss';
import 'foundation-icons/foundation_icons_general/sass/general_foundicons.scss';

import './main.scss';

import config from './botConfig';

import LoggerProvider   from './services/LoggerProvider';
import HttpProvider     from './services/HttpProvider';

import CandidateService from './services/candidateService';
import VacancyService   from './services/vacancyService';

var dependencies = [router, 'foundation', translate];

angular
   .module('bot', dependencies)

   .provider('LoggerService', LoggerProvider)
   .provider('HttpService',   HttpProvider)

   .service('CandidateService',  CandidateService)
   .service('VacancyService',    VacancyService)

   .config(config);
