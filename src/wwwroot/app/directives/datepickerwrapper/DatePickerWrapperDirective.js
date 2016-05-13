import 'angularjs-datepicker/src/js/angular-datepicker';
import 'angularjs-datepicker/src/css/angular-datepicker.css';

import template from  './DatePickerTemplate.html';
import './datepicker.scss';

export default class DatePickerWrapperDirective {
   constructor() {
      this.restrict = 'E';
      this.template = template;
      this.controller = DatePickerController;
      this.scope = {
         datemodel   : '=',
         datemin     : '=',
         placeholder : '@'
      };
   }

   static createInstance() {
      DatePickerWrapperDirective.instance = new DatePickerWrapperDirective();
      return DatePickerWrapperDirective.instance;
   }
}

function DatePickerController ($scope) {
   'ngInject';
   // datepicker settings
   $scope.dateFormat = 'dd.MM.yyyy';
   $scope.minLimit = $scope.datemin || '01-01-1901';
}
