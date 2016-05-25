import context from './../context';

export default function urlFilterCreator() {
   return function _botUrl(url) {
      return context.serverUrl + url;
   };
}
