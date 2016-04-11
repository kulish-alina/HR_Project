import { generateContext } from './context.js';

describe('inital test specs', () => {
   it('false is not true', () => {
      expect(false).not.toBe(true);
   });

   it('true to be true', () => {
      expect(true).toBe(true);
   })
});


describe('urlContext parsing test', () => {
   it('generateContext not to be null or undefined', () => {
      expect(generateContext).not.toBeUndefined();
      // expect(generateContext).no.toBeNull();
   });

   it('Debug and 52031 serverPort', () => {
      let urlMock = spyOn(generateContext, '_getUrlContext')
         .and.returnValue('logLevel=DEBUG&serverUrl=http://localhost:52031/api/');
      let context = generateContext();
      console.log(context);
      expect(context).toEqual({
         logLevel: 'DEBUG',
         logPattern: '*',
         serverUrl: 'http://localhost:52031/api/'
      });
   });
});
