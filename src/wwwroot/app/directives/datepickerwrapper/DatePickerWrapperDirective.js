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
   vm.dateFormat = 'dd.MM.yyyy';
   vm.minLimit   = vm.datemin || '01-01-1901';
   vm.required   = vm.daterequired || false;
   vm.uniqueId   = uniqueId('date');
   vm.validator  = vm.daterequired === 'true' ? 'required, date' : 'date';
}
