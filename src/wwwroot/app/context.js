import {
   assignIn,
   unescape,
   isEmpty
} from 'lodash';

import utils from './utils.js';

import local from '../config/config.context/local.json';
import development from '../config/config.context/develop.json';
import production from '../config/config.context/production.json';
import common from '../config/config.context/common.json';

function _getUrlContext(urlParameter) {
   let object = {};
   let firstSymbolIndex = 1,
      keyIndex = 0,
      valueIndex = 1,
      minLength = 1,
      maxLength = 2;

   if (isEmpty(urlParameter)) {
      return {};
   }

   for (let groupId = 0, paramGroup = urlParameter.substr(firstSymbolIndex).split('&');
      groupId < paramGroup.length; groupId++) {

      let params = paramGroup[groupId].split('=');

      if (params.length < minLength) {
         throw new Error('Can not attempt empty params string');
      }

      if (params.length > maxLength) {
         throw new Error(`It can not be more than one \'=\' in the ${paramGroup}`);
      }

      let key = unescape(params[keyIndex]),
         value = unescape(params[valueIndex]);

      if (isEmpty(key)) {
         throw new Error('Key can not be empty string');
      }

      object[key] = value;
   }

   return object;
}

export function generateContext() {
   let context = {};
   let urlParameters = utils.getUrlParameters();
   let urlContext = _getUrlContext(urlParameters);

   if (process.env.NODE_ENV === 'production') {
      assignIn(context, common, production, local, urlContext);
   } else {
      assignIn(context, common, development, local, urlContext);
   }
   return context;
}

const context = generateContext();
export default context;
