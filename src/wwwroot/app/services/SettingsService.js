import {
   invokeMap,
   remove,
   curry,
   isEqual,
   isFunction
} from 'lodash';

let submitListeners = [];
let editListeners = [];
let cancelListeners = [];
let _$q;

export default class SettingsService {

   constructor($q) {
      'ngInject';
      _$q = $q;
   }
   addOnSubmitListener(listener) {
      return _addListner(submitListeners, listener);
   }

   addOnEditListener(listener) {
      return _addListner(editListeners, listener);
   }

   addOnCancelListener(listener) {
      return _addListner(cancelListeners, listener);
   }

   removeOnSubmitListener(listener) {
      return _removeListner(submitListeners, listener);
   }

   removeOnEditListener(listener) {
      return _removeListner(editListeners, listener);
   }

   removeOnCancelListener(listener) {
      return _removeListner(cancelListeners, listener);
   }

   onSubmit() {
      return _callListeners(submitListeners);
   }

   onEdit() {
      return _callListeners(editListeners);
   }

   onCancel() {
      return _callListeners(cancelListeners);
   }
}
const curryEqual = 2;
const equal = curry(isEqual, curryEqual);

function _callListeners(listeners) {
   let array = invokeMap(listeners, 'call');
   return _$q.all(array);
}

function _removeListner(listeners, listener) {
   remove(listeners, equal(listener));
}

function _addListner(listeners, listener) {
   if (isFunction(listener)) {
      listeners.push(listener);
   }
}
