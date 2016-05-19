import {
   map,
   join
} from 'lodash';
export default function() {
   return function _arrayShowFilter(array, prop, delim = ', ') {
      array = prop ? map(array, prop) : array;
      return join(array, delim);
   };
}
