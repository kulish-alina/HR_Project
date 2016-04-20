import { pick, keys } from 'lodash';
import { generateContext } from './context.js';
import utils from './utils.js';

describe('generateContext function test : ', () => {
   it('Function not to be null or undefined', () => {
      expect(generateContext).not.toBeUndefined();
      expect(generateContext).not.toBeNull();
   });
});

describe('UrlContext parsing test. Expected parameters', () => {
   function _contextTestFabric(urlParameters, expectationObject) {
      spyOn(utils, 'getUrlParameters').and.returnValue(urlParameters);
      let context = pick(generateContext(), keys(expectationObject));
      expect(context).toEqual(expectationObject);
   };

   it('52031 serverPort on localhost', () => {
      let urlParameters = '&serverUrl=http://localhost:52031/api/';
      let expected = { 'serverUrl': 'http://localhost:52031/api/' };
      _contextTestFabric(urlParameters, expected);
   });

   it('8093 on bot.com', () => {
      let urlParameters = '$dasvd=dasfsd&gdfgdf=jghkjghj&serverUrl=http://bot.com:8093/api/';
      let expected = { serverUrl: 'http://bot.com:8093/api/' };
      _contextTestFabric(urlParameters, expected);
   });

   it('INFO logLeel and 10000 on localhost', () => {
      let urlParameters = '&foo=bar&serverUrl=http://localhost:10000/api/&logLevel=INFO';
      let expected = {serverUrl : 'http://localhost:10000/api/', logLevel : 'INFO'};
      _contextTestFabric(urlParameters, expected);
   });
});
