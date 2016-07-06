'use strict';

import StorageService from './LocalStorageService.js';

describe('Unit: LocalStorageService test on', () => {
   let service;
   let localStore = {};
   let mockSessionStorage;

   let mockWindow;

   let mockLogger;

   const once = 1;

   const testValues = {
      simpleValue: 'Jhon Snow',
      randomValue: '#4$23/bla\'bla/car//car..//.//I need ☕!',
      simpleUser: {
         userName: 'Jhon Snow',
         isDead: false,
         activeUser: true,
         age: 17
      },
      unicodeUser: {
         userName: 'Jhon ❄',
         isDead: false,
         activeUser: true,
         age: 17
      }
   };

   beforeEach(() => {
      mockSessionStorage = {
         getItem: (key) => localStore[key] || null,
         setItem: (key, value) => {
            localStore[key] = value;
         },
         removeItem: (key) => {
            delete localStore[key];
         }
      };
      spyOn(mockSessionStorage, 'getItem').and.callThrough();
      spyOn(mockSessionStorage, 'setItem').and.callThrough();
      spyOn(mockSessionStorage, 'removeItem').and.callThrough();

      mockWindow = {
         localStorage: mockSessionStorage
      };
      mockLogger = {
         log: jasmine.createSpy(),
         warn: jasmine.createSpy(),
         debug: jasmine.createSpy(),
         error: jasmine.createSpy()
      };
      service = new StorageService(mockWindow, mockLogger);
   });

   describe('set', () => {
      it('username test', () => {
         service.set('userName', testValues.simpleValue);

         expect(mockSessionStorage.setItem).toHaveBeenCalledWith('userName', '"Jhon Snow"');
         expect(mockLogger.log).toHaveBeenCalledTimes(once);
         expect(mockLogger.error).not.toHaveBeenCalled();
      });

      it('simple user test', () => {
         service.set('user', testValues.simpleUser);

         expect(mockSessionStorage.setItem).toHaveBeenCalledTimes(once);
         expect(mockLogger.log).toHaveBeenCalledTimes(once);
         expect(mockLogger.error).not.toHaveBeenCalled();
      });

      it('unicode name user test', () => {
         service.set('user', testValues.unicodeUser);

         expect(mockSessionStorage.setItem).toHaveBeenCalledTimes(once);
         expect(mockLogger.log).toHaveBeenCalledTimes(once);
         expect(mockLogger.error).not.toHaveBeenCalled();
      });

      it('strange data test', () => {
         service.set('foo', testValues.randomValue);

         expect(mockSessionStorage.setItem).toHaveBeenCalledTimes(once);
         expect(mockLogger.log).toHaveBeenCalledTimes(once);
         expect(mockLogger.error).not.toHaveBeenCalled();
      });

      it('multiple sets test', () => {
         service.set('foo', testValues.randomValue);
         service.set('foo', testValues.simpleValue);

         expect(mockSessionStorage.setItem).toHaveBeenCalledTimes(2);
         expect(mockLogger.log).toHaveBeenCalledTimes(2);
         expect(mockLogger.error).not.toHaveBeenCalled();
      });
   });

   describe('get', () => {
      it('\'strange\' value', () => {
         service.set('foo', testValues.randomValue);
         let value = service.get('foo');

         expect(value).toEqual(testValues.randomValue);
         expect(mockSessionStorage.setItem).toHaveBeenCalledTimes(once);
         expect(mockSessionStorage.getItem).toHaveBeenCalledTimes(once);
         expect(mockLogger.log).toHaveBeenCalledTimes(once);
      });

      it('userName value', () => {
         service.set('username', testValues.simpleValue);
         let value = service.get('username');

         expect(value).toEqual(testValues.simpleValue);
         expect(mockSessionStorage.setItem).toHaveBeenCalledTimes(once);
         expect(mockSessionStorage.getItem).toHaveBeenCalledTimes(once);
         expect(mockLogger.log).toHaveBeenCalledTimes(once);
      });

      it('\'simple\' user value', () => {
         service.set('user', testValues.simpleUser);
         let value = service.get('user');

         expect(value).toEqual(testValues.simpleUser);
         expect(mockSessionStorage.setItem).toHaveBeenCalledTimes(once);
         expect(mockSessionStorage.getItem).toHaveBeenCalledTimes(once);
         expect(mockLogger.log).toHaveBeenCalledTimes(once);
      });

      it('\'unicode\' user value', () => {
         service.set('user', testValues.unicodeUser);
         let value = service.get('user');

         expect(value).toEqual(testValues.unicodeUser);
         expect(mockSessionStorage.setItem).toHaveBeenCalledTimes(once);
         expect(mockSessionStorage.getItem).toHaveBeenCalledTimes(once);
         expect(mockLogger.log).toHaveBeenCalledTimes(once);
      });
   });

   describe('remove', () => {
      it('\'strange\' data', () => {
         service.set('foo', testValues.randomValue);
         service.remove('foo');

         expect(service.get('foo')).toBeNull();
         expect(mockSessionStorage.setItem).toHaveBeenCalledTimes(once);
         expect(mockSessionStorage.removeItem).toHaveBeenCalledTimes(once);
         expect(mockLogger.log).toHaveBeenCalledTimes(2);
      });
   });
});
