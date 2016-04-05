// describe('inital test specs', function() {
//    it('true is true', function() {
//       expect(true).toBe(true);
//    });

//    it('false is not true', function() {
//       expect(false).not.toBe(true);
//    })
// });

import { generateContext } from '../app/context.js';

describe('inital test specs', () => {
   it('false is not true', () => {
      expect(false).not.toBe(true);
   });

   it('true to be true', () => {
      expect(true).toBe(true);
   })
});
