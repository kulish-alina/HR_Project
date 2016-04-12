let _validation;

export default class ValidationService {
   constructor($validation) {
      'ngInject';
      _validation = $validation;
   }

   validate(form) {
      _validation.validate(form);
      return _validation.checkValid(form);
   }
   reset(form) {
      _validation.reset(form);
   }
}
