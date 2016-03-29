import {
   assignIn
} from 'lodash';

import local from '../config/config.context/local.json';
import development from '../config/config.context/develop.json';
import production from '../config/config.context/production.json';

var context = {};

if (process.env.NODE_ENV === 'development') {
   assignIn(context, local, development);
} else {
   assignIn(context, local, production);
}

export default context;
