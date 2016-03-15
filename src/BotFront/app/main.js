import angular from 'angular';
import angularRouter from 'angular-ui-router';

var dependencies = [angularRouter];

angular
   .module('bot', dependencies)
   .config(function($stateProvider, $urlRouterProvider) {
      $urlRouterProvider.otherwise('home');

      $stateProvider
         .state('home', {
            url: '/home',
            template: require('./views/home-view.html')
         })
         .state('candidates', {
            url: '/candidates',
            template: require('./views/candidates-view.html')
         })
         .state('vacancies', {
            url: '/vacancies',
            template: require('./views/vacancies-view.html')
         })
   })