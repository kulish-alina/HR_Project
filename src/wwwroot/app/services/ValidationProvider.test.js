import validationProvider from './ValidationProvider.js';

describe('ValidationService tests for', () => {

   let service = null;

   let _validationMock = {
      validate:   jasmine.createSpy(),
      checkValid: jasmine.createSpy(),
      reset:      jasmine.createSpy()
   };

   let _rootScopeMock = {
      $evalAsync: jasmine.createSpy()
   };

   let _$qMock = {
      defer: () => {
         return jasmine.createSpy();
      }
   };

   beforeEach(() => {
      angular.module('test', []).provider('ValidationService', validationProvider);
      angular.mock.module('test');
      angular.mock.module($provide => {
         $provide.value('$validation', _validationMock);
         $provide.value('$rootScope', _rootScopeMock);
         $provide.value('$q', _$qMock);
      });
   });

   beforeEach(inject((ValidationService) => {
      service = ValidationService;
   }));

   it('validate not to be undefined or null', () => {
      expect(service.validate).not.toBeUndefined();
      expect(service.validate).not.toBeNull();
   });

   it('reset not to be undefined or null', () => {
      expect(service.reset).not.toBeUndefined();
      expect(service.reset).not.toBeNull();
   });

   it('validation.validate and validation.checkValid are called with expected arguments ', () => {
      service.validate('fake');
      expect(_validationMock.validate).toHaveBeenCalledWith('fake');
   });

   it('validation.reset called with expected argument', () => {
      service.reset('fake');
      expect(_validationMock.reset).toHaveBeenCalledWith('fake');
   });
});
