import { generateContext } from '../app/context.js';

describe('inital test specs', () => {
   it('false is not true', () => {
      expect(false).not.toBe(true);
   });

   it('true to be true', () => {
      expect(true).toBe(true);
   })

   it('genaerateContext not to be null or undefined', () => {
      expect(generateContext).not.toBeUndefined();
      //expect(generateContext).no.toBeNull();
   })
});
