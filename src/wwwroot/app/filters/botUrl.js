import context from './../context';

export default function() {
   return function _botUrl(url) {
      return context.serverUrl + url;
   };
}
