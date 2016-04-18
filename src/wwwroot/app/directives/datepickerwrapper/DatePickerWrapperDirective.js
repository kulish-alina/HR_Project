import template from  './DatePickerTemplate.html';

export default class DatePickerWrapperDirective {
   constructor() {
      this.restrict = 'E';
      this.template = template;
      this.scope = {
         datemodel: '=',
         placeholder: '='
      }
   }
   static createInstance() {
      'ngInject';
      DatePickerWrapperDirective.instance = new DatePickerWrapperDirective();
      return DatePickerWrapperDirective.instance;
   }
}
