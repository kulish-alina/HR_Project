'use strict';

import UserService from './UserService';

describe('UserService testing', () => {
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

   let mockHttpCache = {
      get         : jasmine.createSpy(),
      clearCache  : jasmine.createSpy()
   };

   function executeThen(dataObject) {
      let callback = promiseMock.then.calls.first().args[0];
      callback(dataObject);
      promiseMock.then.calls.reset();
   }

   beforeEach(() => {
      service = new UserService(mockHttp, promiseMock, mockHttpCache);
      mockHttp.get.calls.reset();
      mockHttp.put.calls.reset();
      mockHttp.post.calls.reset();
      mockHttp.remove.calls.reset();
      spyOn(service, 'getUsers').and.callThrough();
      spyOn(promiseMock, 'then').and.returnValue(promiseMock);
   });

   it('getCurrentUser not to be undefined or null', () => {
      expect(service.getCurrentUser).not.toBeUndefined();
      expect(service.getCurrentUser).not.toBeNull();
   });

   it('setCurrentUser not to be undefined or null', () => {
      expect(service.setCurrentUser).not.toBeUndefined();
      expect(service.setCurrentUser).not.toBeNull();
   });

   it('getUserById not to be undefined or null', () => {
      expect(service.getUserById).not.toBeUndefined();
      expect(service.getUserById).not.toBeNull();
   });

   it('saveUser not to be undefined or null', () => {
      expect(service.saveUser).not.toBeUndefined();
      expect(service.saveUser).not.toBeNull();
   });

   it('getUsers not to be undefined or null', () => {
      expect(service.getUsers).not.toBeUndefined();
      expect(service.getUsers).not.toBeNull();
   });

   it('saveUser test server query for new user saving', () => {
      let user = {};
      service.saveUser(user);
      executeThen();
      expect(mockHttp.post).toHaveBeenCalled();
      expect(mockHttpCache.clearCache.calls.count()).toEqual(1);
   });

   it('saveUser test server query for user editing', () => {
      let user = {
         id: 1
      };
      service.saveUser(user);
      expect(mockHttp.put).toHaveBeenCalled();
   });

});
