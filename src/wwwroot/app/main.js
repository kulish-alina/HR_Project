import angular from 'angular';
import router from 'angular-ui-router';
import translate from 'angular-translate';

import config from './botConfig';

import LoggerProvider   from './services/LoggerProvider';
import HttpProvider     from './services/HttpProvider';

import CandidateService from './services/candidateService';
import VacancyService   from './services/vacancyService';
import ThesauruseService   from './services/thesaurusService';

import ThesauruseDirective   from './directives/thesaurus';

var dependencies = [router, translate];

angular
   .module('bot', dependencies)

   .provider('LoggerService', LoggerProvider)
   .provider('HttpService',   HttpProvider)

   .service('CandidateService',  CandidateService)
   .service('VacancyService',    VacancyService)
   .service('ThesauruseService', ThesauruseService)

   .directive('thesauruse', ThesauruseDirective.createInstance)

   .config(config);
