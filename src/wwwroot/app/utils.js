import {
   reduce
} from 'lodash';

let utils = {
   getUrlParameters,
   array2map,
   formatDateToServer,
   formatDateFromServer
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

function formatDateToServer(entityDate) {
   if (entityDate) {
      let date = new Date(entityDate);
      return new Date(date.valueOf() - date.getTimezoneOffset() * 60000);
   }
}

function formatDateFromServer(entityDate) {
   if (entityDate) {
      let date = new Date(entityDate);
      return new Date(date.valueOf() + date.getTimezoneOffset() * 60000);
   }
}
