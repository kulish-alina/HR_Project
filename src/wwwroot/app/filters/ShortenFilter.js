import {
    slice
} from 'lodash';
const THRESHOLD = 25;

export default function() {
   return function _shorten(text) {
      return text.length > THRESHOLD ?
            [...slice(text, 0, THRESHOLD), '...'].join('') :
            text;
   };
}
