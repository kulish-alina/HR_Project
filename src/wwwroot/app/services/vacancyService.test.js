'use strict';

import vacancyService from '../services/vacancyService';

describe('VacancyService tests for', () => {
   let service = null;
   let mock    = {
      get   : jasmine.createSpy(),
      post  : jasmine.createSpy(),
      put   : jasmine.createSpy(),
      remove: jasmine.createSpy()
   };

   beforeEach(() => {
      angular.module('test', []).service('VacancyService', vacancyService);
      angular.mock.module('test');
      angular.mock.module($provide => {
         $provide.value('HttpService', mock);
      });
   });

   it('getVacancies not to be undefined', inject((VacancyService) => {
      expect(VacancyService.getVacancies).not.toBeUndefined();
   }));

   it('getVacancies not to be null', inject((VacancyService) => {
      expect(VacancyService.getVacancies).not.toBeNull();
   }));
});
