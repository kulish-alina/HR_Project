export function getUrlParameters() {
   return window.location.search;
}

let utils = {}
utils.getUrlParameters = getUrlParameters;
export default utils;
