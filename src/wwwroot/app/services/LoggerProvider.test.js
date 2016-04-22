'use strict';

import loggerProvider from './LoggerProvider.js';

describe('Unit: Log Level -', () => {
   let logger;
   let mock;

   function _teardown(logLevel) {
      beforeEach(() => {
         mock = {
            log: jasmine.createSpy(),
            debug: jasmine.createSpy(),
            warn: jasmine.createSpy(),
            error: jasmine.createSpy()
         };

         angular.module('test', []).provider('LoggerService', loggerProvider);
         angular.mock.module('test');
         angular.mock.module($provide => {
            $provide.value('$log', mock);
         });
         angular.mock.module((LoggerServiceProvider) => {
            LoggerServiceProvider.changeLogLevel(logLevel);
         });
      });

      beforeEach(inject((LoggerService) => {
         logger = LoggerService;
      }));
   }

   describe('OFF.', () => {

      _teardown('OFF');

      it('Log test', () => {
         logger.log('Test', 'on log', 'with', 'OFF option');
         expect(mock.log).not.toHaveBeenCalled();
      });

      it('Debug test', () => {
         logger.debug('Test', 'on debug', 'with', 'OFF option');
         expect(mock.debug).not.toHaveBeenCalled();
      });

      it('Warn test', () => {
         logger.warn('Test', 'on warn', 'with', 'OFF option');
         expect(mock.warn).not.toHaveBeenCalled();
      });

      it('Error test', () => {
         logger.error('Test', 'on error', 'with', 'OFF option');
         expect(mock.error).not.toHaveBeenCalled();
      });
   });

   describe('ERROR.', () => {

      _teardown('ERROR');

      it('Log test', () => {
         logger.log('Test', 'on log', 'with', 'ERROR option');
         expect(mock.log).not.toHaveBeenCalled();
      });

      it('Debug test', () => {
         logger.debug('Test', 'on debug', 'with', 'ERROR option');
         expect(mock.debug).not.toHaveBeenCalled();
      });

      it('Warn test', () => {
         logger.warn('Test', 'on warn', 'with', 'ERROR option');
         expect(mock.warn).not.toHaveBeenCalled();
      });

      it('Error test', () => {
         logger.error('Test', 'on error', 'with', 'ERROR option');
         expect(mock.error).toHaveBeenCalled();
      });
   });

   describe('WARN.', () => {

      _teardown('WARN');

      it('Log test', () => {
         logger.log('Test', 'on log', 'with', 'WARN option');
         expect(mock.log).not.toHaveBeenCalled();
      });

      it('Debug test', () => {
         logger.debug('Test', 'on debug', 'with', 'WARN option');
         expect(mock.debug).not.toHaveBeenCalled();
      });

      it('Warn test', () => {
         logger.warn('Test', 'on warn', 'with', 'WARN option');
         expect(mock.warn).toHaveBeenCalled();
      });

      it('Error test', () => {
         logger.error('Test', 'on error', 'with', 'WARN option');
         expect(mock.error).toHaveBeenCalled();
      });
   });

   describe('LOG.', () => {

      _teardown('LOG');

      it('Log test', () => {
         logger.log('Test', 'on log', 'with', 'LOG option');
         expect(mock.log).toHaveBeenCalled();
      });

      it('Debug test', () => {
         logger.debug('Test', 'on debug', 'with', 'LOG option');
         expect(mock.debug).not.toHaveBeenCalled();
      });

      it('Warn test', () => {
         logger.warn('Test', 'on warn', 'with', 'LOG option');
         expect(mock.warn).toHaveBeenCalled();
      });

      it('Error test', () => {
         logger.error('Test', 'on error', 'with', 'LOG option');
         expect(mock.error).toHaveBeenCalled();
      });
   });

   describe('DEBUG.', () => {

      _teardown('DEBUG');

      it('Log test', () => {
         logger.log('Test', 'on log', 'with', 'DEBUG option');
         expect(mock.log).toHaveBeenCalled();
      });

      it('Debug test', () => {
         logger.debug('Test', 'on debug', 'with', 'DEBUG option');
         expect(mock.debug).toHaveBeenCalled();
      });

      it('Warn test', () => {
         logger.warn('Test', 'on warn', 'with', 'DEBUG option');
         expect(mock.warn).toHaveBeenCalled();
      });

      it('Error test', () => {
         logger.error('Test', 'on error', 'with', 'DEBUG option');
         expect(mock.error).toHaveBeenCalled();
      });
   });
});
