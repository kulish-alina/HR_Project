'use strict';

import  vacancyService from './VacancyService';

describe('VacancyService tests for', () => {
   let service = null;
   let promiseMock = {
      when:    () => promiseMock,
      all:     () => promiseMock,
      then:    () => promiseMock,
      finally: () => promiseMock,
      reject:  () => promiseMock,
      catch:   () => promiseMock
   };
   let mock = {
      get: jasmine.createSpy().and.returnValue(promiseMock),
      post: jasmine.createSpy().and.returnValue(promiseMock),
      put: jasmine.createSpy().and.returnValue(promiseMock),
      remove: jasmine.createSpy().and.returnValue(promiseMock)
   };
   let thesaurusMock = {
      saveThesaurusTopics: jasmine.createSpy()
   };
   let loggerMock = {
      error: jasmine.createSpy()
   };

   beforeEach(() => {
      angular.module('test', []).service('VacancyService', vacancyService);
      angular.mock.module('test');
      angular.mock.module($provide => {
         $provide.value('HttpService', mock);
         $provide.value('ThesaurusService', thesaurusMock);
         $provide.value('LoggerService', loggerMock);
         $provide.value('$q', promiseMock);
      });
   });

   beforeEach(inject(VacancyService => {
      service = VacancyService;
   }));

   it('getVacancies not to be undefined or null', () => {
      expect(service.getVacancies).not.toBeUndefined();
      expect(service.getVacancies).not.toBeNull();
   });

   it('getVacancy not to be null', () => {
      expect(service.getVacancy).not.toBeUndefined();
      expect(service.getVacancy).not.toBeNull();
   });

   it('saveVacancy not to be null', () => {
      expect(service.saveVacancy).not.toBeUndefined();
      expect(service.saveVacancy).not.toBeNull();
   });

   it('deleteVacancy not to be null', () => {
      expect(service.deleteVacancy).not.toBeUndefined();
      expect(service.deleteVacancy).not.toBeNull();
   });

   it('getVacancies call test', () => {
      service.getVacancies();
      expect(mock.get).toHaveBeenCalledWith('vacancies/');
   });

   it('getVacancy with id 1 call test', () => {
      let id = 1;
      service.getVacancy(id);
      expect(mock.get).toHaveBeenCalledWith(`vacancies/${id}`);
   });

   it('saveVacancy with id call test', () => {
      let vacancy = {
         id: 1
      };
      spyOn(service, '_saveNewTopicsToThesaurus').and.callThrough();
      spyOn(promiseMock, 'then').and.returnValue(promiseMock);
      service.saveVacancy(vacancy);
      let response = {
         'skills' : [],
         'tags': []
      };
      let callback = promiseMock.then.calls.first().args[0];
      callback(response);
      promiseMock.then.calls.reset();
      expect(mock.put).toHaveBeenCalledWith(`vacancies/${vacancy.id}`, vacancy);
   });

   it('saveVacancy without id call test', () => {
      let vacancy = {
      };
      spyOn(service, '_saveNewTopicsToThesaurus').and.callThrough();
      spyOn(promiseMock, 'then').and.returnValue(promiseMock);
      service.saveVacancy(vacancy);
      let response = {
         'skills' : [],
         'tags': []
      };
      let callback = promiseMock.then.calls.first().args[0];
      callback(response);
      promiseMock.then.calls.reset();

      service.saveVacancy(vacancy);
      expect(mock.post).toHaveBeenCalledWith('vacancies/', vacancy);
   });

   it('deleteVacancy with id', () => {
      let vacancy = {
         id: 1
      };

      service.deleteVacancy(vacancy);
      expect(mock.remove).toHaveBeenCalledWith(`vacancies/${vacancy.id}`, vacancy);
   });

   it('deleteVacancy without id', () => {
      let vacancy = {
      };

      expect(service.deleteVacancy).toThrowError(/id/);
      expect(mock.remove).not.toHaveBeenCalledWith(`vacancies/${vacancy.id}`, vacancy);
   });
});
