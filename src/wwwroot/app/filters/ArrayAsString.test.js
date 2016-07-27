import ArrayAsString from './ArrayAsString';

const arr = [{location:'Dniepropetrovsk1', email: 'antonov@mail.be1', login: 'dant1'},
             {location:'Dniepropetrovsk2', email: 'antonov@mail.be2', login: 'dant2'},
             {location:'Dniepropetrovsk3', email: 'antonov@mail.be3', login: 'dant3'},
             {location:'Dniepropetrovsk4', email: 'antonov@mail.be4', login: 'dant4'},
             {location:'Dniepropetrovsk5', email: 'antonov@mail.be5', login: 'dant5'}];

describe('Filter: arrayAsString', () => {

   beforeEach(() => {
      angular.module('test',[]).filter('arrayAsString', ArrayAsString);
      angular.mock.module('test');
   });

   let _arrayAsString;
   beforeEach(angular.mock.inject(($filter) => {
      _arrayAsString = $filter('arrayAsString');
   }));

   it('should be able to show only location', () => {
      expect(_arrayAsString(arr, 'location'))
         .toBe('Dniepropetrovsk1, Dniepropetrovsk2, Dniepropetrovsk3, Dniepropetrovsk4, Dniepropetrovsk5');
   });

   it('should be able to show only email', () => {
      expect(_arrayAsString(arr, 'email'))
         .toBe('antonov@mail.be1, antonov@mail.be2, antonov@mail.be3, antonov@mail.be4, antonov@mail.be5');
   });

   it('should be able to show only login', () => {
      expect(_arrayAsString(arr, 'login'))
         .toBe('dant1, dant2, dant3, dant4, dant5');
   });

   it('should be able to show only logins, includes "/" after each word', () => {
      expect(_arrayAsString(arr,'login','/'))
         .toBe('dant1/dant2/dant3/dant4/dant5');
   });

   it('should be able to show array as string', () => {
      expect(_arrayAsString(['dant1', 'dant2', 'dant3', 'dant4', 'dant5']))
         .toBe('dant1, dant2, dant3, dant4, dant5');
   });

   it('should be able to show array as string, includes "/" after each word', () => {
      expect(_arrayAsString(['dant1', 'dant2', 'dant3', 'dant4', 'dant5'], null, '/'))
         .toBe('dant1/dant2/dant3/dant4/dant5');
   });
});
