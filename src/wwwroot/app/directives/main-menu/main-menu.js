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

function MainMenuController() {

}
