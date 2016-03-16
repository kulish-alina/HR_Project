import angular from 'angular';
import angularRouter from 'angular-ui-router';
import candidatesCtrl from './candidates/candidatesCtrl.js';
import vacanciesCtrl from './vacancies/vacanciesCtrl.js';

var dependencies = [angularRouter];

angular
    .module('bot', dependencies)
    .config(function($stateProvider, $urlRouterProvider) {

        $urlRouterProvider.otherwise('home');

        $stateProvider
            .state('home', {
                url: '/home',
                template: require('./home/home-view.html')
            })
            .state('candidates', {
                url: '/candidates',
                template: require('./candidates/candidates-view.html'),
                controller: candidatesCtrl
            })
            .state('vacancies', {
                url: '/vacancies',
                template: require('./vacancies/vacancies-view.html'),
			       controller: vacanciesCtrl
            })
    })