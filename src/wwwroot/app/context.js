import {
   assignIn
} from 'lodash';

import local from './config/local.json';
import development from './config/develop.json';
import production from './config/production.json';

var context = {};

if (process.env.NODE_ENV === 'development') {
   assignIn(context, local, development);
} else {
   assignIn(context, local, production);
}

export default context;
