import {
   reduce,
   split,
   trimEnd
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
   let partsOfDate = split(entityDate, '.');
   return `${partsOfDate[2]}-${partsOfDate[1]}-${partsOfDate[0]}T00:00:00`; // eslint-disable-line no-magic-numbers
}

function formatDateFromServer(entityDate) {
   let partsOfDate = split(trimEnd(trimEnd(entityDate, '00:00:00'), 'T'), '-');
   return `${partsOfDate[2]}.${partsOfDate[1]}.${partsOfDate[0]}`; // eslint-disable-line no-magic-numbers
}
