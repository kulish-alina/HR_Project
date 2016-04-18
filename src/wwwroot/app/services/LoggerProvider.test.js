'use strict';

import loggerProvider from './LoggerProvider.js';

describe('Unit: Log Level -', () => {
   let logger;
   let mock = {
      log: jasmine.createSpy(),
      debug: jasmine.createSpy(),
      warn: jasmine.createSpy(),
      error: jasmine.createSpy()
   };

   describe('OFF. ', () => {
      beforeEach(() => {
         angular.module('test', []).provider('LoggerService', loggerProvider);
         angular.mock.module('test');
         angular.mock.module($provide => {
            $provide.value('$log', mock);
         });
         angular.mock.module((LoggerServiceProvider) => {
            LoggerServiceProvider.changeLogLevel('OFF');
         });
      });

      beforeEach(inject((LoggerService) => {
         logger = LoggerService;
      }));

      it('Log test', () => {
         logger.log('First test on log with off option');
         expect(mock.log).not.toHaveBeenCalled();
      });

      it('Debug test', () => {
         logger.debug('First test on debug with off option');
         expect(mock.debug).not.toHaveBeenCalled();
      });

      it('Warning test', () => {
         logger.warning('First test on warning with off option');
         expect(mock.warn).not.toHaveBeenCalled();
      });

      it('Error test', () => {
         logger.error('First test on error with off option');
         expect(mock.error).not.toHaveBeenCalled();
      });
   });
});
