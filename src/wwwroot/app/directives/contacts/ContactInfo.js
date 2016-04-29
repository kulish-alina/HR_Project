import template from './contact-info.directive.html';

export default class ContactInfoDirective {
   constructor() {
      this.restrict = 'E';
      this.template = template;
      this.scope = {
         phone  : '=',
         email  : '=',
         skype  : '=',
         social : '='
      };
      this.controller = ContactInfoController;
   }
   static createInstance() {
      'ngInject';
      ContactInfoDirective.instance = new ContactInfoDirective();
      return ContactInfoDirective.instance;
   }
}

function ContactInfoController() {

}
