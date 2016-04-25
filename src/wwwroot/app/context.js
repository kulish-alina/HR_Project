import {
   assignIn,
   unescape,
   isEmpty
} from 'lodash';

import utils from './utils.js';

import local         from '../config/config.context/local.json';
import development   from '../config/config.context/develop.json';
import production    from '../config/config.context/production.json';
import common        from '../config/config.context/common.json';

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
         console.error('Can not attempt empty params string');
         return {};
      }

      if (params.length > maxLength) {
         console.error(`It can not be more than one '=' in the ${paramGroup}`);
         return {};
      }

      let key = unescape(params[keyIndex]),
         value = unescape(params[valueIndex]);

      if (isEmpty(key)) {
         console.error('Key can not be empty string');
         return {};
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
