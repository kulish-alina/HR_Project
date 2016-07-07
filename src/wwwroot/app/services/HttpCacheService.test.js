'use strict';

import HttpCacheService from './HttpCacheService';

import { times } from 'lodash';

describe('HttpCacheService testing', () => {
   let service = null;
   let promiseMock = {
      when:    () => promiseMock,
      all:     () => promiseMock,
      then:    () => promiseMock,
      catch:   () => promiseMock,
      finally: () => promiseMock,
      reject:  () => promiseMock,
      defer:   () => promiseMock
   };
   let mockHttp = {
      get      : jasmine.createSpy().and.returnValue(promiseMock),
      post     : jasmine.createSpy().and.returnValue(promiseMock),
      put      : jasmine.createSpy().and.returnValue(promiseMock),
      remove   : jasmine.createSpy().and.returnValue(promiseMock)
   };

   function executeThen(dataObject) {
      if (promiseMock.then.calls.count()) {
         let callback = promiseMock.then.calls.first().args[0];
         callback(dataObject);
         promiseMock.then.calls.reset();
      }
   }

   function executeFinally(dataObject) {
      if (promiseMock.finally.calls.count()) {
         let callback = promiseMock.finally.calls.first().args[0];
         callback(dataObject);
         promiseMock.finally.calls.reset();
      }
   }

   beforeEach(() => {
      service = new HttpCacheService(mockHttp, promiseMock);
      mockHttp.get.calls.reset();
      mockHttp.put.calls.reset();
      mockHttp.post.calls.reset();
      mockHttp.remove.calls.reset();
      spyOn(promiseMock, 'then').and.returnValue(promiseMock);
      spyOn(promiseMock, 'defer').and.returnValue({ promise: promiseMock });
      spyOn(promiseMock, 'finally').and.returnValue(promiseMock);
      spyOn(promiseMock, 'when').and.returnValue(promiseMock);
   });

   it('get not to be undefined or null', () => {
      expect(service.get).not.toBeUndefined();
      expect(service.get).not.toBeNull();
   });

   it('clearCache not to be undefined or null', () => {
      expect(service.clearCache).not.toBeUndefined();
      expect(service.clearCache).not.toBeNull();
   });

   it('get testing: httpService has only one calling for simple urls', () => {
      let url = 'users/';
      times(2, () => {
         service.get(url);
         executeThen([]);
      });
      expect(mockHttp.get.calls.count()).toEqual(1);
      expect(promiseMock.defer.calls.count()).toEqual(1);
   });

   it('testing getFromCache by imitation paralel several get calling for simple url', () => {
      let url = 'locations';
      service.get(url);
      service.get(url);
      executeThen([]);
      executeFinally();
      service.get(url);
      expect(mockHttp.get.calls.count()).toEqual(1);
      expect(promiseMock.defer.calls.count()).toEqual(2);
      expect(promiseMock.when.calls.count()).toEqual(1);
   });

   it('clearCache testing by cheking httpService.get calling after clearCache', () => {
      let url = 'countries';
      service.get(url);
      executeThen([]);
      executeFinally();
      service.clearCache(url);
      service.get(url);
      expect(mockHttp.get.calls.count()).toEqual(2);
   });
});
