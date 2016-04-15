'use strict';

import loggerProvider from './LoggerProvider.js';

describe('Unit: Log Level -', () => {
   var _provider = null;
   var _logger = null;
   let mockProvider = {
      changeLogLevel: jasmine.createSpy()
   };

   describe('OFF. ', () => {
      beforeEach(() => {
         angular.module('test', []).service('LoggerProvider', loggerProvider);
         angular.mock.module('test');
      });

      beforeEach(inject((LoggerProvider) => {
         _provider = LoggerProvider;
         _provider.changeLogLevel('OFF');
         expect(mockProvider.changeLogLevel).toHaveBeenCalled();
      }));

      beforeEach(() => {
         console.log(_provider.$get);

         console.log = jasmine.createSpy();
         console.debug = jasmine.createSpy();
         console.warn = jasmine.createSpy();
         console.error = jasmine.createSpy();
         _logger = _provider.$get.last();
      })

      it('Log test', () => {
         _logger.log('First test on log with off option');
         expect(console.log).not.toHaveBeenCalled();
      });

      it('Debug test', () => {
         _logger.debug('First test on debug with off option');
         expect(console.debug).not.toHaveBeenCalled();
      });

      it('Warning test', () => {
         _logger.warning('First test on warning with off option');
         expect(console.warn).not.toHaveBeenCalled();
      });

      it('Error test', () => {
         _logger.error('First test on error with off option');
         expect(console.error).not.toHaveBeenCalled();
      });
   });
});
