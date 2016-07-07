'use strict';

import  VacancyService from './VacancyService';

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
   let mockHttp = {
      get: jasmine.createSpy().and.returnValue(promiseMock),
      post: jasmine.createSpy().and.returnValue(promiseMock),
      put: jasmine.createSpy().and.returnValue(promiseMock),
      remove: jasmine.createSpy().and.returnValue(promiseMock)
   };
   let thesaurusMock = {
      saveThesaurusTopics: jasmine.createSpy(),
      getThesaurusTopicsByIds: jasmine.createSpy()
   };
   let loggerMock = {
      error: jasmine.createSpy()
   };

   let userServiceMock = {
      getUserById: jasmine.createSpy()
   };

   beforeEach(() => {
      service = new VacancyService(mockHttp, thesaurusMock, promiseMock, loggerMock, userServiceMock);
   });

   it('search not to be undefined or null', () => {
      expect(service.search).not.toBeUndefined();
      expect(service.search).not.toBeNull();
   });

   it('save not to be null', () => {
      expect(service.save).not.toBeUndefined();
      expect(service.save).not.toBeNull();
   });

   it('deleteVacancy not to be null', () => {
      expect(service.remove).not.toBeUndefined();
      expect(service.remove).not.toBeNull();
   });

   it('remove with id', () => {
      let vacancy = {
         id: 1
      };

      service.remove(vacancy);
      expect(mockHttp.remove).toHaveBeenCalledWith(`vacancy/${vacancy.id}`, vacancy);
   });

   it('remove without id', () => {
      let vacancy = {
      };

      expect(service.remove).toThrowError(/id/);
      expect(mockHttp.remove).not.toHaveBeenCalledWith(`vacancy/${vacancy.id}`, vacancy);
   });
});
