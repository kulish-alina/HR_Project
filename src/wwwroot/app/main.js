import angular from 'angular';
import router from 'angular-ui-router';
import translate from 'angular-translate';
import uiDate from 'angular-ui-date';
//import 'jquery-ui/themes/base/jquery-ui.css';
import config from './botConfig';

import LoggerProvider   from './services/LoggerProvider';
import HttpProvider     from './services/HttpProvider';

import CandidateService from './services/candidateService';
import VacancyService   from './services/vacancyService';

var dependencies = [router, translate, uiDate.name];

angular
   .module('bot', dependencies)

   .provider('LoggerService', LoggerProvider)
   .provider('HttpService',   HttpProvider)

   .service('CandidateService',  CandidateService)
   .service('VacancyService',    VacancyService)

   .config(config);
