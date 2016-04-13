import { has } from 'lodash';
import { generateContext } from './context.js';
import utils from './utils.js';

describe('generateContext function test : ', () => {
   it('Function not to be null or undefined', () => {
      expect(generateContext).not.toBeUndefined();
      expect(generateContext).not.toBeNull();
   });
});


describe('UrlContext parsing test. Expected parameters', () => {
   let contextTestFabric = (urlParameters, expectationObject) => {

      spyOn(utils, 'getUrlParameters').and.returnValue(urlParameters);
      console.log(utils.getUrlParameters());
      let context = generateContext();
      console.log(context);
      for (var key in expectationObject) {
         if (has(context, key)) {
            var element = expectationObject[key];
            expect(context[key]).toEqual(element);
         } else {
            console.error(`The field ${key} is not defined in context`);
         }
      }
   };

   it('52031 serverPort on localhost', () => {
      contextTestFabric('&serverUrl=http://localhost:52031/api/',
         { 'serverUrl': 'http://localhost:52031/api/' })
   });

   it('8093 on bot.com', () => {
      contextTestFabric('$dasvd=dasfsd&gdfgdf=jghkjghj&serverUrl=http://bot.com:8093/api/',
         { serverUrl: 'http://bot.com:8093/api/' })
   });

   it('INFO logLeel and 10000 on localhost', () => {
      contextTestFabric('&foo=bar&serverUrl=http://localhost:10000/api/&logLevel=INFO',
         {serverUrl : 'http://localhost:10000/api/', logLevels : 'INFO'})
   });
});
