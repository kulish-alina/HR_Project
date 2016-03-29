import {
   assignIn
} from 'lodash';

import local from '../config/config.context/local.json';
import development from '../config/config.context/develop.json';
import production from '../config/config.context/production.json';

function _getURLParams(urlParameter) {
   let object = {};

   for (let aItKey, nKeyId = 0, aCouples = window.location.search.substr(1).split('&');
         nKeyId < aCouples.length; nKeyId++) {
      aItKey = aCouples[nKeyId].split('=');
      object[unescape(aItKey[0])] = aItKey.length > 1 ? unescape(aItKey[1]) : '';
   }

   return object;
}

var context = {};
let urlContext = _getURLParams(window.location.search);

if (process.env.NODE_ENV === 'production') {
   assignIn(context, production, local, urlContext);
} else {
   assignIn(context, development, local, urlContext);
}

export default context;
