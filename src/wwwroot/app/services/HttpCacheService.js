import { has } from 'lodash';

let _HttpService;
const cache = {};

export default class HttpCacheService {
   consructor(HttpService) {
      'ngInject';
      _HttpService = HttpService;
   }

   get(_url) {
      if (has(cache, _url)) {
         return cache[_url].httpRequest;
      } else {
         cache[_url].httpRequest = _HttpService.get(_url).then(entities => cache[_url].data = entities);
      }
   }

   clearCache(_url) {

   }
}
