import template from './main-menu.directive.html';
import './main-menu.scss';
export default class MainMenuDirective {
   constructor() {
      this.restrict = 'E';
      this.template = template;
      this.scope = true;
      this.controller = MainMenuController;
      this.replace = true;
   }
   static createInstance() {
      'ngInject';
      MainMenuDirective.instance = new MainMenuDirective();
      return MainMenuDirective.instance;
   }
}

function MainMenuController($scope, $state) {
   let vm = $scope;

   vm.goCandidate = _goCandidate;
   vm.goVacancy   = _goVacancy;

   function _goCandidate() {
      $state.go('candidate', {_data: null, candidateId: null, toPrevious: false});
   }

   function _goVacancy() {
      $state.go('vacancyEdit', {_data: null, vacancyId: null, toPrevious: false});
   }
}
