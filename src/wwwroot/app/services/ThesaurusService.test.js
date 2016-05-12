'use strict';

import thesaurusService from './ThesaurusService';

import {
   forEach,
   has,
   not
} from 'lodash';

import THESAURUS_STRUCTURES from './ThesaurusStructuresStore.js';

describe('ThesaurusService testing: ', function() {
   let service = null;
   let promiseMock = {
      when: function() {
         return this;
      },
      all: function () {
         return this;
      },
      then: function () {
         return this;
      },
      finally: function () {
         return this;
      },
      reject: function () {
         return this;
      }
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

   function _sendToCacheThesaurus(thesaurusName) {
      spyOn(promiseMock, 'then');
      service.getThesaurusTopics(thesaurusName);
      let callback = promiseMock.then.calls.first().args[0];
      let response = {};
      response[thesaurusName] = [];
      callback(response);
      promiseMock.then.calls.reset();  
   }

   beforeEach(() => {
      angular.module('test', []).service('ThesaurusService', thesaurusService);
      angular.mock.module('test');
      angular.mock.module($provide => {
         $provide.value('HttpService', mockHttp);
         $provide.value('$translate', mockTranslate);
         $provide.value('$q', promiseMock);
      });
   });

   beforeEach(inject(ThesaurusService => {
      service = ThesaurusService;
   }));

   it('getThesaurusTopics not to be undefined or null', function() {
      expect(service.getThesaurusTopics).not.toBeUndefined();
      expect(service.getThesaurusTopics).not.toBeNull();
   });

   it('getThesaurusTopic not to be undefined or null', function() {
      expect(service.getThesaurusTopic).not.toBeUndefined();
      expect(service.getThesaurusTopic).not.toBeNull();
   });

   it('getThesaurusTopicsGroup not to be undefined or null', function() {
      expect(service.getThesaurusTopicsGroup).not.toBeUndefined();
      expect(service.getThesaurusTopicsGroup).not.toBeNull();
   });

   it('saveThesaurusTopic not to be undefined or null', function() {
      expect(service.saveThesaurusTopic).not.toBeUndefined();
      expect(service.saveThesaurusTopic).not.toBeNull();
   });

   it('saveThesaurusTopics not to be undefined or null', function() {
      expect(service.saveThesaurusTopics).not.toBeUndefined();
      expect(service.saveThesaurusTopics).not.toBeNull();
   });

   it('deleteThesaurusTopic not to be undefined or null', function() {
      expect(service.deleteThesaurusTopic).not.toBeUndefined();
      expect(service.deleteThesaurusTopic).not.toBeNull();
   });

   it('getThesaurusTopics call test for simple thesaurus', function() {
      service.getThesaurusTopics('socialnetworks');
      expect(mockHttp.get).toHaveBeenCalledWith('socialnetworks');
   });

   it('getThesaurusTopics call test for complex thesaurus', function() {
      service.getThesaurusTopics('locations')
      expect(mockHttp.get).toHaveBeenCalledWith('locations');
      expect(mockHttp.get).toHaveBeenCalledWith('countries');
   });

   it('test cache: http calls once', function() {
      mockHttp.get.calls.reset();
      _sendToCacheThesaurus('languages');
      service.getThesaurusTopics('languages');
      expect(mockHttp.get.calls.count()).toEqual(1);
   });

   it('getThesaurusTopic with id 1 call test for simple thesaurus not existed to cache', function() {      
      const skillId = 1;
      spyOn(service, 'getThesaurusTopics').and.callThrough();
      service.getThesaurusTopic('skills', skillId);
      expect(service.getThesaurusTopics).toHaveBeenCalledWith(`skills`);
   });

   it('getThesaurusTopic with id 1 call test for simple thesaurus existed to cache', function() {
      const skillId = 1;
      _sendToCacheThesaurus('skills');
      spyOn(service, 'getThesaurusTopics').and.callThrough();
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

   it('saveThesaurusTopic test for only thesaurus saving', function() {
      mockHttp.put.calls.reset();
      mockHttp.post.calls.reset();
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

   it('deleteThesaurusTopic test for only thesaurus deletning', function() {
      mockHttp.remove.calls.reset();
      service.deleteThesaurusTopic('notThesaurus', {});
      expect(mockHttp.remove).not.toHaveBeenCalled();
   });

});
