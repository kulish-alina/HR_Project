import template from  './DatePickerTemplate.html';

export default class DatePickerWrapperDirective {
   constructor() {
      this.restrict = 'E';
      this.template = template;
      this.controller = DatePickerController;
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

function DatePickerController ($scope) {
// datepicker settings
   $scope.dateFormat = 'dd-MM-yyyy';
   $scope.minLimit = '01-01-1901';
}
