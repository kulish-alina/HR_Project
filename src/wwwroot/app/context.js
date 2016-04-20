import {
   assignIn
} from 'lodash';

import utils from './utils.js';

import local from '../config/config.context/local.json';
import development from '../config/config.context/develop.json';
import production from '../config/config.context/production.json';
import common from '../config/config.context/common.json';

function _getUrlContext(urlParameter) {
   let object = {};

   for (let aItKey, nKeyId = 0, aCouples = urlParameter.substr(1).split('&');
      nKeyId < aCouples.length; nKeyId++) {
      aItKey = aCouples[nKeyId].split('=');
      object[unescape(aItKey[0])] = aItKey.length > 1 ? unescape(aItKey[1]) : '';
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
