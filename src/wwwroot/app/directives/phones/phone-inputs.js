import template from './phone-inputs.directive.html';
import './phone-inputs.scss';

import {
   includes,
   filter
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
   vm.resolveSignOf      = _resolveSignOf;
   vm.removePhone        = _removePhone;

   /*---impl---*/
   function _addNewPhone() {
      vm.phones.push({number: '', index: vm.phones.length });
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
   function _removePhone(phoneToRemove) {
      vm.phones = phoneToRemove.index ?
         filter(vm.phones, phone => phone.index !== phoneToRemove.index) :
         filter(vm.phones, phone => phone.number !== phoneToRemove.number);
      if (vm.phones.length === 0) {
         _addNewPhone();
      }
   }
   function _resolveSignOf(index) {
      return vm.phones.length === 1 || index === vm.phones.length ? 'plus' : 'minus';
   }
}
