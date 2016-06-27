import template from './contact-info.directive.html';
import './contact-info.scss';
export default class ContactInfoDirective {
   constructor() {
      this.restrict = 'E';
      this.template = template;
      this.scope = {
         entity: '=',
         direction: '@'
      };
      this.controller = ContactInfoController;
   }
   static createInstance() {
      'ngInject';
      ContactInfoDirective.instance = new ContactInfoDirective();
      return ContactInfoDirective.instance;
   }
}

function ContactInfoController($scope) {
   'ngInject';
   let vm = $scope;
   vm.direction = vm.direction || 'ltr';
}
