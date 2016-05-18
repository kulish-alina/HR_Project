import {
   reduce,
   isArray
} from 'lodash';
export default function() {
   return function _arrayShowFilter(arr, key) {
      let res = '';
      if (key) {
         res = reduce(arr, (memo, value) => {
            memo = isArray(memo) ? memo : [];
            memo.push(value[key]);
            return memo;
         },{});
      } else {
         res = arr;
      }
      return res.toString();
   };
}

