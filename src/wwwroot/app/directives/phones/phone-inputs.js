import template from './phone-inputs.directive.html';
import './phone-inputs.scss';

import {
   includes
} from 'lodash';
export default class PhoneInputsDirective {
   constructor() {
      this.restrict   = 'E';
      this.template   = template;
      this.scope      = {
         phones: '=ngModel'
      };
      this.controller = PhoneInputsController;
   }
   static createInstance() {
      'ngInject';
      PhoneInputsDirective.instance = new PhoneInputsDirective();
      return PhoneInputsDirective.instance;
   }
}

function PhoneInputsController($scope) {
   'ngInject';
   let vm = $scope;

   /*---api---*/
   vm.addNewPhone        = _addNewPhone;
   vm.cantAddNewPhone    = _cantAddNewPhone;
   vm.updatePhoneNumbers = _updatePhoneNumbers;

   /*---impl---*/
   function _addNewPhone() {
      vm.phones.push({number: ''});
   }

   function _cantAddNewPhone() {
      return includes(vm.phones, {number: ''});
   }

   function _updatePhoneNumbers(number, index) {
      if (number && !includes(vm.phones, {number})) {
         vm.phones[index] = number;
      } else {
         vm.phones.slice(index, 1);
      }
   }
}
