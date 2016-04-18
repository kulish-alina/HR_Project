'use strict';

import loggerProvider from './LoggerProvider.js';

describe('Unit: Log Level -', () => {
   let logger;
   let mock;

   function _test(logLevel) {

      let _init = () => {
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

      let _run = () => {
         var tests = {
            off: 0,
            error: 1,
            warn: 2,
            log: 3,
            debug: 4
         };

         let _case = (value, key, level) => {
            if (key === 'off') {
               return;
            }

            it(`${key} test`, () => {
               logger[key]('test', 'for', key, 'with', logLevel, 'level', 'option');
               if (value <= level) {
                  expect(mock[key]).toHaveBeenCalled();
               } else {
                  expect(mock[key]).not.toHaveBeenCalled();
               }
            });
         }

         var level = tests[logLevel.toLowerCase()];

         for (var key in tests) {
            _case(tests[key], key, level);
         }
      }

      describe(`${logLevel}.`, () => {
         _init();
         _run();
      });
   }

   _test('OFF');
   _test('ERROR');
   _test('WARN');
   _test('LOG');
   _test('DEBUG');
});
