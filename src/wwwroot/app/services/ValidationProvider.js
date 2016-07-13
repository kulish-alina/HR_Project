import {
   each,
   method,
   debounce
} from 'lodash';

const DEBOUNCE_TIME = 25;

let _q;
let validation;
let _rootScope;
let _defers = [];
let _resetCallback = debounce(_reset, DEBOUNCE_TIME);

export default class ValidationProvider {
   resetCallback() {
      _resetCallback();
   }

   $get($validation, $rootScope, $q) {
      'ngInject';
      _q          = $q;
      _rootScope  = $rootScope;
      validation  = $validation;
      return new ValidationService();
   }
}

class ValidationService {
   validate(form) {
      return validation.validate(form);
   }

   reset(form) {
      const deferred = _q.defer();
      _defers.push(deferred);
      validation.reset(form);
      return deferred.promise;
   }
}

function _reset() {
   _rootScope.$evalAsync(() => {
      each(_defers, method('resolve'));
      _defers = [];
   });
}
