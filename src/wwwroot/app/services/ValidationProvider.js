import {
   each,
   method
} from 'lodash';

let _q;
let validation;
let _defers = [];

export default class ValidationProvider {
   resetCallback() {
      each(_defers, method('resolve'));
      _defers = [];
   }

   $get($validation, $q) {
      'ngInject';
      _q          = $q;
      validation  = $validation;
      return new ValidationService();
   }
}

class ValidationService {
   validate(form) {
      validation.validate(form);
      return validation.checkValid(form);
   }

   reset(form) {
      const deferred = _q.defer();
      _defers.push(deferred);
      validation.reset(form);
      return deferred.promise;
   }
}
