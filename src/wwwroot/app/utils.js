import {
   reduce
} from 'lodash';

let utils = {
   getUrlParameters,
   array2map
};

export default utils;

function getUrlParameters() {
   return window.location.search;
}

function array2map(arr, it) {
   return reduce(arr, (memo, key) => {
      memo[key] = it(key);
      return memo;
   }, {});
}
