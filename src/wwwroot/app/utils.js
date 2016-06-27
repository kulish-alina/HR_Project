import {
   reduce,
   split,
   isString
} from 'lodash';

let utils = {
   getUrlParameters,
   array2map,
   formatDateToServer,
   formatDateFromServer,
   formatDateTimeFromServer
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
   if (isString(entityDate)) {
      let splitDate = split(entityDate, ' ');
      let partsOfDate = split(splitDate[0], '-');
      let partsOfTime = split(splitDate[1], ':');
      console.log(partsOfDate);
      let date = new Date(partsOfDate[2], partsOfDate[1], partsOfDate[0], partsOfTime[0],
                 partsOfTime[1], partsOfTime[2]);
      console.log(date);
      return new Date(date.valueOf() + date.getTimezoneOffset() * 60000);
   } else {
      return entityDate;
   }
}

function formatDateFromServer(entityDate) {
   if (entityDate) {
      let trimOfDate = split(entityDate, 'T');
      let partsOfDate = split(trimOfDate[0], '-');
      return `${partsOfDate[2]}-${partsOfDate[1]}-${partsOfDate[0]}`; // eslint-disable-line no-magic-numbers
   }
}

function formatDateTimeFromServer(entityDate) {
   if (entityDate) {
      let trimOfDate = split(entityDate, 'T');
      let partsOfDate = split(trimOfDate[0], '-');
      let partsOfTime = split(trimOfDate[1], ':');
      return `${partsOfDate[2]}-${partsOfDate[1]}-${partsOfDate[0]} ${partsOfTime[0]}:${partsOfDate[1]}`; // eslint-disable-line max-len
   }
}
