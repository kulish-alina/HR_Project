import 'angularjs-datepicker/src/js/angular-datepicker';
import 'angularjs-datepicker/src/css/angular-datepicker.css';

import template from  './DatePickerTemplate.html';
import './datepicker.scss';

import {
   uniqueId
} from 'lodash';

export default class DatePickerWrapperDirective {
   constructor() {
      this.restrict = 'E';
      this.template = template;
      this.controller = DatePickerController;
      this.scope = {
         datemodel    : '=',
         datemin      : '=',
         dateset      : '=',
         placeholder  : '@',
         daterequired : '@',
         datevalidator: '@'
      };
   }

   static createInstance() {
      DatePickerWrapperDirective.instance = new DatePickerWrapperDirective();
      return DatePickerWrapperDirective.instance;
   }
}

function DatePickerController ($scope) {
   'ngInject';
   const vm = $scope;

   /*--- datepicker settings ---*/
   vm.dateFormat = 'MMM d, y';
   vm.minLimit   = vm.datemin || 'Jan 01, 1901';
   vm.uniqueId   = uniqueId('date');
   vm.validator  = vm.daterequired === 'true' ? 'required, date' : 'date';
}
