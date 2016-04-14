'use strict';

import vacancyService from './vacancyService';

describe('VacancyService tests for', () => {
   let service = null;
   let mock = {
      get: jasmine.createSpy(),
      post: jasmine.createSpy(),
      put: jasmine.createSpy(),
      remove: jasmine.createSpy()
   };

   beforeEach(() => {
      angular.module('test', []).service('VacancyService', vacancyService);
      angular.mock.module('test');
      angular.mock.module($provide => {
         $provide.value('HttpService', mock);
      });
   });

   it('getVacancies not to be undefined or null', inject((VacancyService) => {
      expect(VacancyService.getVacancies).not.toBeUndefined();
      expect(VacancyService.getVacancies).not.toBeNull();
   }));

   it('getVacancy not to be null', inject((VacancyService) => {
      expect(VacancyService.getVacancy).not.toBeUndefined();
      expect(VacancyService.getVacancy).not.toBeNull();
   }));

   it('saveVacancy not to be null', inject((VacancyService) => {
      expect(VacancyService.saveVacancy).not.toBeUndefined();
      expect(VacancyService.saveVacancy).not.toBeNull();
   }));

   it('deleteVacancy not to be null', inject((VacancyService) => {
      expect(VacancyService.deleteVacancy).not.toBeUndefined();
      expect(VacancyService.deleteVacancy).not.toBeNull();
   }));

   it('getVacancies call test', inject((VacancyService) => {
      let vacancies = VacancyService.getVacancies();
      expect(mock.get).toHaveBeenCalledWith('vacancies/');
   }));

   it('getVacancy with id 1 call test', inject((VacancyService) => {
      let id = 1;
      let vacancy = VacancyService.getVacancy(id);
      expect(mock.get).toHaveBeenCalledWith(`vacancies/${id}`);
   }));

   it('saveVacancy with id call test', inject((VacancyService) => {
      let vacancy = {
         Id: 1
      };

      VacancyService.saveVacancy(vacancy);
      expect(mock.put).toHaveBeenCalledWith(`vacancies/${vacancy.Id}`, vacancy);
   }));

   it('saveVacancy without id call test', inject((VacancyService) => {
      let vacancy = {
      };

      VacancyService.saveVacancy(vacancy);
      expect(mock.post).toHaveBeenCalledWith('vacancies/', vacancy);
   }));

   it('deleteVacancy', inject((VacancyService) => {
      let vacancy = {
         Id: 1
      };

      VacancyService.deleteVacancy(vacancy);
      expect(mock.remove).toHaveBeenCalledWith(`vacancies/${vacancy.Id}`, vacancy);
   }));
});
