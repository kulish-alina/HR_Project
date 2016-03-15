import angular from 'angular/angular.js';

import angularRouter from 'angular-ui-router/release/angular-ui-router.js';

var dependencies =[angularRouter];

angular
   .module('bot', dependencies)
   .config(function($stateProvider, $urlRouterProvider) {
      $urlRouterProvider.otherwise('home');
      
      $stateProvider
         .state('home',{
            url: '/Home',
            templateUrl:'views/home-view.html'
         })      
         .state('candidates',{
            url: '/Candidates',
            templateUrl:'views/candidates-view.html'
         })
         .state('vacancies',{
            url: '/Vacancies',
            templateUrl:'views/vacancies-view.html'
         })
   })