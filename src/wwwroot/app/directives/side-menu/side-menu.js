import template from './side-menu.directive.html';
import './side-menu.scss';
export default class SideMenuDirective {
   constructor() {
      this.restrict = 'E';
      this.template = template;
      this.scope = true;
      this.controller = SideMenuController;
   }
   static createInstance() {
      'ngInject';
      SideMenuDirective.instance = new SideMenuDirective();
      return SideMenuDirective.instance;
   }
}

function SideMenuController() {

}
