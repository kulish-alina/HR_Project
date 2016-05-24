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
      get: jasmine.createSpy().and.returnValue(promiseMock),
      post: jasmine.createSpy().and.returnValue(promiseMock),
      put: jasmine.createSpy().and.returnValue(promiseMock),
      remove: jasmine.createSpy().and.returnValue(promiseMock)
   };

   function executeThen(dataObject) {
      let callback = promiseMock.then.calls.first().args[0];
      callback(dataObject);
      promiseMock.then.calls.reset();
   }

   beforeEach(() => {
      service = new UserService(mockHttp, promiseMock);
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

   it('getUserById test when not exist to cache', () => {
      const userId = 1;
      service.getUserById(userId);
      executeThen({id: userId});
      expect(mockHttp.get.calls.count()).toEqual(1);
   });

   it('getUserById test when exist to cache', () => {
      const userId = 1;
      service.getUserById(userId);
      executeThen({id: userId});
      expect(mockHttp.get.calls.count()).toEqual(0);
   });

   it('saveUser test server query for new user saving', () => {
      let user = {};
      service.saveUser(user);
      expect(mockHttp.post).toHaveBeenCalled();
   });

   it('saveUser test server query for user editing', () => {
      let user = {
         id: 1
      };
      service.saveUser(user);
      expect(mockHttp.put).toHaveBeenCalled();
   });

});
