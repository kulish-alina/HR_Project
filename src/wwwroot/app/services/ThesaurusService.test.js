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
      get      : jasmine.createSpy().and.returnValue(promiseMock),
      post     : jasmine.createSpy().and.returnValue(promiseMock),
      put      : jasmine.createSpy().and.returnValue(promiseMock),
      remove   : jasmine.createSpy().and.returnValue(promiseMock)
   };
   let mockTranslate = {
      instant  : jasmine.createSpy()
   };

   let mockHttpCache = {
      get         : jasmine.createSpy(),
      clearCache  : jasmine.createSpy()
   };

   beforeEach(() => {
      service = new ThesaurusService(mockHttp, promiseMock, mockTranslate, mockHttpCache);
      mockHttp.get.calls.reset();
      mockHttp.put.calls.reset();
      mockHttp.post.calls.reset();
      mockHttp.remove.calls.reset();
      spyOn(service, 'getThesaurusTopics').and.callThrough();
   });

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
      expect(mockHttpCache.get).toHaveBeenCalledWith('socialnetworks');
   });

   it('getThesaurusTopics call test for complex thesaurus', () => {
      service.getThesaurusTopics('location');
      expect(mockHttpCache.get).toHaveBeenCalledWith('location');
      expect(mockHttpCache.get).toHaveBeenCalledWith('country');
   });


   it('saveThesaurusTopic with id call test', () => {
      let country = {
         id: 1
      };
      service.saveThesaurusTopic('country', country);
      expect(mockHttp.put).toHaveBeenCalledWith(`country/${country.id}`, country);
   });

   it('saveThesaurusTopic without id call test', () => {
      let country = {};
      service.saveThesaurusTopic('country', country);
      expect(mockHttp.post).toHaveBeenCalledWith('country/', country);
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
      service.deleteThesaurusTopic('country', country);
      expect(mockHttp.remove).toHaveBeenCalledWith(`country/${country.id}`, country);
   });

   it('deleteThesaurusTopic test for only thesaurus deleting', () => {
      service.deleteThesaurusTopic('notThesaurus', {});
      expect(mockHttp.remove).not.toHaveBeenCalled();
   });

});
