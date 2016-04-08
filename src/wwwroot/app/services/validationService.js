let _validation;

export default class VacancyService {
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
