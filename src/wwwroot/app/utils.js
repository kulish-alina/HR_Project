import {
   reduce,
   each,
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
   let indexOfParts = [2, 1, 0];
   let newDate = '';
   each(indexOfParts, (index) => {
      newDate += `${partsOfDate[index]}-`;
   });
   return `${trimEnd(newDate, '-')}T00:00:00`;
}

function formatDateFromServer(entityDate) {
   let partsOfDate = split(trimEnd(entityDate, 'T00:00:00'), '-');
   let indexOfParts = [2, 1, 0];
   let newDate = '';
   each(indexOfParts, (index) => {
      newDate += `${partsOfDate[index]}.`;
   });
   return trimEnd(newDate, '.');
}
