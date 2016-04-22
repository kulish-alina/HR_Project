import {
   remove,
   each,
   curry,
   isEqual
   /*isFunction*/
} from 'lodash';

let submitListeners = [];
let editListeners = [];
let cancelListeners = [];

export default class SettingsService {

   constructor() {
      this.asEdit = '';
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
      _removeListner(editListeners, listener);
   }

   removeOnEditListener(listener) {
      _removeListner(editListeners, listener);
   }

   removeOnCancelListener(listener) {
      _removeListner(cancelListeners, listener);
   }

   onSubmit() {
      _callListeners(submitListeners);
   }

   onEdit() {
      _callListeners(editListeners);
   }

   onCancel() {
      _callListeners(cancelListeners);
   }
   setAsEdit(condition) {
      this.asEdit = condition;
   }
   getAsEdit() {
      return this.asEdit;
   }
}
const curryEqual = 2;
const equal = curry(isEqual, curryEqual);

function _callListeners(listeners) {
   each(listeners, (fnc) => {
      fnc();
   });
}

function _removeListner(listeners, listener) {
   remove(listeners, equal(listener));
}

function _addListner(listeners, listener) {
   listeners.push(listener);
}
