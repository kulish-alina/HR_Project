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
      _$q = $q;
   }
   addOnSubmitListener(listener) {
      _addListner(submitListeners, listener);
   }

   addOnEditListener(listener) {
      _addListner(editListeners, listener);
   }

   addOnCancelListener(listener) {
      _addListner(cancelListeners, listener);
   }

   removeOnSubmitListener(listener) {
      _removeListner(submitListeners, listener);
   }

   removeOnEditListener(listener) {
      _removeListner(editListeners, listener);
   }

   removeOnCancelListener(listener) {
      _removeListner(cancelListeners, listener);
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
