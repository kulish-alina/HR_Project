'use strict';

import config from './config-validation';

import { first, constant } from 'lodash';

const fields = ['getExpression', 'setDefaultMsg', 'setExpression', 'setValidMethod'];

describe('Validation must', () => {

   let mock    = _createMock();
   let ValidationProvider = { resetCallback: constant('fake') };
   let origin  = jasmine.createSpy();

   beforeEach(() => {
      mock.getExpression = () => origin;
      config(mock, ValidationProvider);
   });

   it('checking when user already out of the field', () => {
      expect(mock.setValidMethod).toHaveBeenCalledWith('blur');
   });

   it('don\'t shown success message on valid value', () => {
      expect(mock.showSuccessMessage).toBe(false);
   });

   it('use own `resetCallback`', () => {
      expect(mock.resetCallback).toBe(ValidationProvider.resetCallback);
   });

   describe('have rule "minlength". ', () => {
      let fnc;
      const length = 3;
      beforeEach(() => {
         let args = mock.setExpression.calls.first().args;
         fnc = first(args).minlength;
      });

      it('', () => {
         expect(fnc).toBeDefined();
      });

      it('Return true if value is empty ', () => {
         let res = fnc('', null, null, null, length);
         expect(res).toBe(true);
         expect(origin).not.toHaveBeenCalled();
      });

      it('Call original function if value is not empty ', () => {
         fnc('23', null, null, null, length);
         expect(origin).toHaveBeenCalledWith('23', null, null, null, length);
      });
   });

   describe('have rule "maxlength". ', () => {
      let fnc;
      const length = 3;
      beforeEach(() => {
         let args = mock.setExpression.calls.first().args;
         fnc = first(args).maxlength;
      });

      it('', () => {
         expect(fnc).toBeDefined();
      });

      it('Return true if value is empty ', () => {
         let res = fnc('', null, null, null, length);
         expect(res).toBe(true);
      });

      it('Call original function if value is not empty ', () => {
         fnc('23', null, null, null, length);
         expect(origin).toHaveBeenCalledWith('23', null, null, null, length);
      });
   });

   describe('have rule "email". ', () => {
      let fnc;
      beforeEach(() => {
         let args = mock.setExpression.calls.first().args;
         fnc = first(args).email;
      });

      it('', () => {
         expect(fnc).toBeDefined();
      });

      it('Return true if value is empty ', () => {
         let res = fnc('');
         expect(res).toBe(true);
      });

      it('Call original function if value is not empty ', () => {
         fnc('isd@mail', null, null, null, null);
         expect(origin).toHaveBeenCalledWith('isd@mail', null, null, null, null);
      });
   });


   describe('have rule "number". ', () => {
      let fnc;
      beforeEach(() => {
         let args = mock.setExpression.calls.first().args;
         fnc = first(args).number;
      });

      it('', () => {
         expect(fnc).toBeDefined();
      });

      it('Return true if value is empty ', () => {
         let res = fnc('');
         expect(res).toBe(true);
      });

      it('Call original function if value is not empty ', () => {
         fnc('45', null, null, null, null);
         expect(origin).toHaveBeenCalledWith('45', null, null, null, null);
      });
   });

   describe('have rule "url". ', () => {
      let fnc;
      beforeEach(() => {
         let args = mock.setExpression.calls.first().args;
         fnc = first(args).url;
      });

      it('', () => {
         expect(fnc).toBeDefined();
      });

      it('Return true if value is empty ', () => {
         let res = fnc('');
         expect(res).toBe(true);
      });

      it('Call original function if value is not empty ', () => {
         fnc('http://isd.dp.ua', null, null, null, null);
         expect(origin).toHaveBeenCalledWith('http://isd.dp.ua', null, null, null, null);
      });
   });

   describe('have rule "title". ', () => {
      let fnc;
      beforeEach(() => {
         let args = mock.setExpression.calls.first().args;
         fnc = first(args).title;
      });

      it('', () => {
         expect(fnc).toBeDefined();
      });

      it('Return true if value is empty ', () => {
         let res = fnc('');
         expect(res).toBe(true);
      });

      it('Return false if value length less then 3 ', () => {
         let res = fnc('ff');
         expect(res).toBe(false);
         res = fnc('f');
         expect(res).toBe(false);
      });

      it('Return false if value length more then 50 ', () => {
         let res = fnc('aaaaaaaaaabbbbbbbbbbccccccccccddddddddddeffffffffff');
         expect(res).toBe(false);
         res = fnc('aaaaaaaaaabbbbbbbbbbccccccccccddddddddddeeeeeeeeeeffffffffff');
         expect(res).toBe(false);
      });

      it('Return true if value length more then 2 and less then 50 ', () => {
         let res = fnc('aaaaaaaaaabbbbbbbbbbccccccccccddddddddddffffffffff');
         expect(res).toBe(true);
         res = fnc('abc');
         expect(res).toBe(true);
         res = fnc('abcdefg');
         expect(res).toBe(true);
      });
   });

   describe('have rule "date". ', () => {
      let fnc;
      beforeEach(() => {
         let args = mock.setExpression.calls.first().args;
         fnc = first(args).date;
      });

      it('', () => {
         expect(fnc).toBeDefined();
      });

      it('Return true if value is empty ', () => {
         let res = fnc('');
         expect(res).toBe(true);
      });

      it('Return false if value is invalid date', () => {
         let res = fnc('13-13-1025');
         expect(res).toBe(false);
         res = fnc('40-12-2015');
         expect(res).toBe(false);
      });

      it('Return true if value is valid date, format', () => {
         let res = fnc('10-10-2012');
         expect(res).toBe(true);
         res = fnc('11-11-2015');
         expect(res).toBe(true);
      });
   });
});

function _createMock() {
   let mock = jasmine.createSpyObj('mock', fields);
   fields.forEach(field => mock[field].and.returnValue(mock));
   return mock;
}
