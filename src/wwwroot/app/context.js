import {
   assignIn
} from 'lodash';

import local from '../config/config.context/local.json';
import development from '../config/config.context/develop.json';
import production from '../config/config.context/production.json';

function _getURLParams(urlParameter) {
     
}

var context = {};
let url = window.location.search;
let urlContext = _getURLParams(url);

if (process.env.NODE_ENV === 'production') {
   assignIn(context, production, local, urlContext);
} else {
   assignIn(context, development, local, urlContext);
}

export default context;
