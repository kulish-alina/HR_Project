'use strict';

import ThesaurusService from './ThesaurusService';

describe('ThesaurusService testing: ', () => {
   let service = null;
   let promiseMock = {
      when:    () => promiseMock,
      all:     () => promiseMock,
      then:    () => promiseMock,
      finally: () => promiseMock,
      reject:  () => promiseMock
   };
   let mockHttp = {
      get: jasmine.createSpy().and.returnValue(promiseMock),
      post: jasmine.createSpy().and.returnValue(promiseMock),
      put: jasmine.createSpy().and.returnValue(promiseMock),
      remove: jasmine.createSpy().and.returnValue(promiseMock)
   };
   let mockTranslate = {
      instant: jasmine.createSpy()
   };

   beforeEach(() => {
      service = new ThesaurusService(mockHttp, promiseMock, mockTranslate);
      mockHttp.get.calls.reset();
      mockHttp.put.calls.reset();
      mockHttp.post.calls.reset();
      mockHttp.remove.calls.reset();
      spyOn(service, 'getThesaurusTopics').and.callThrough();
   });

   function _sendToCacheThesaurus(thesaurusName) {
      spyOn(promiseMock, 'then');
      service.getThesaurusTopics(thesaurusName);
      let callback = promiseMock.then.calls.first().args[0];
      callback({[thesaurusName]: []});
      promiseMock.then.calls.reset();
   }

   it('getThesaurusTopics not to be undefined or null', () => {
      expect(service.getThesaurusTopics).not.toBeUndefined();
      expect(service.getThesaurusTopics).not.toBeNull();
   });

   it('getThesaurusTopic not to be undefined or null', () => {
      expect(service.getThesaurusTopic).not.toBeUndefined();
      expect(service.getThesaurusTopic).not.toBeNull();
   });

   it('getThesaurusTopicsGroup not to be undefined or null', () => {
      expect(service.getThesaurusTopicsGroup).not.toBeUndefined();
      expect(service.getThesaurusTopicsGroup).not.toBeNull();
   });

   it('saveThesaurusTopic not to be undefined or null', () => {
      expect(service.saveThesaurusTopic).not.toBeUndefined();
      expect(service.saveThesaurusTopic).not.toBeNull();
   });

   it('saveThesaurusTopics not to be undefined or null', () => {
      expect(service.saveThesaurusTopics).not.toBeUndefined();
      expect(service.saveThesaurusTopics).not.toBeNull();
   });

   it('deleteThesaurusTopic not to be undefined or null', () => {
      expect(service.deleteThesaurusTopic).not.toBeUndefined();
      expect(service.deleteThesaurusTopic).not.toBeNull();
   });

   it('getThesaurusTopics call test for simple thesaurus', () => {
      service.getThesaurusTopics('socialnetworks');
      expect(mockHttp.get).toHaveBeenCalledWith('socialnetworks');
   });

   it('getThesaurusTopics call test for complex thesaurus', () => {
      service.getThesaurusTopics('locations');
      expect(mockHttp.get).toHaveBeenCalledWith('locations');
      expect(mockHttp.get).toHaveBeenCalledWith('countries');
   });

   it('test cache: http calls once', () => {
      _sendToCacheThesaurus('languages');
      service.getThesaurusTopics('languages');
      expect(mockHttp.get.calls.count()).toEqual(1);
   });

   it('getThesaurusTopic with id 1 call test for simple thesaurus not existed to cache', () => {
      const skillId = 1;
      service.getThesaurusTopic('skills', skillId);
      expect(service.getThesaurusTopics).toHaveBeenCalledWith('skills');
   });

   it('getThesaurusTopic with id 1 call test for simple thesaurus existed to cache', () => {
      const skillId = 1;
      _sendToCacheThesaurus('skills');
      service.getThesaurusTopics.calls.reset();
      service.getThesaurusTopic('skills', skillId);
      expect(service.getThesaurusTopics).not.toHaveBeenCalled();
   });

   it('saveThesaurusTopic with id call test', () => {
      let country = {
         id: 1
      };
      service.saveThesaurusTopic('countries', country);
      expect(mockHttp.put).toHaveBeenCalledWith(`countries/${country.id}`, country);
   });

   it('saveThesaurusTopic without id call test', () => {
      let country = {};
      service.saveThesaurusTopic('countries', country);
      expect(mockHttp.post).toHaveBeenCalledWith('countries/', country);
   });

   it('saveThesaurusTopic test for only thesaurus saving', () => {
      service.saveThesaurusTopic('notThesaurus', {});
      expect(mockHttp.put).not.toHaveBeenCalled();
      expect(mockHttp.post).not.toHaveBeenCalled();
   });

   it('deleteThesaurusTopic with id', () => {
      let country = {
         id: 1
      };
      service.deleteThesaurusTopic('countries', country);
      expect(mockHttp.remove).toHaveBeenCalledWith(`countries/${country.id}`, country);
   });

   it('deleteThesaurusTopic test for only thesaurus deletning', () => {
      service.deleteThesaurusTopic('notThesaurus', {});
      expect(mockHttp.remove).not.toHaveBeenCalled();
   });

});
