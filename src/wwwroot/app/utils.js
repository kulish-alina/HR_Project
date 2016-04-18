let utils = {
   getUrlParameters,
   last
};

export default utils;

function getUrlParameters() {
   return window.location.search;
}

function last(array) {
   return array[array.length - 1];
   //return array.slice(-1)[0]
}
