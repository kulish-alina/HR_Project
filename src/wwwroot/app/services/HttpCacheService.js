import { has, cond, constant, invokeMap, curry } from 'lodash';

const httpRequests   = {};
const cache          = {};

let _HttpService, _$q;
let _curryHas = curry(has, 2);

let getFromCache = cond([
   [_curryHas(cache),         getCachePromise],
   [_curryHas(httpRequests),  addToHttpRequestDeferreds],
   [constant(true),           addHttpRequest]
]);

export default class HttpCacheService {
   constructor(HttpService, $q) {
      'ngInject';
      _HttpService = HttpService;
      _$q = $q;
   }

   get(url) {
      return getFromCache(url);
   }

   clearCache(url) {
      delete cache[url];
   }
}

function addToHttpRequestDeferreds(url) {
   let requestDeferred = _$q.defer();
   httpRequests[url].deferreds.push(requestDeferred);
   return requestDeferred.promise;
}

function addHttpRequest(url) {
   httpRequests[url] = {
      deferreds : [],
      request : _HttpService.get(url)
         .then(entities => {
            cache[url] = entities;
            invokeMap(httpRequests[url].deferreds, 'resolve', entities);
         })
         .catch(error => invokeMap(httpRequests[url].deferreds, 'reject', error))
         .finally(() => delete httpRequests[url])
   };
   return addToHttpRequestDeferreds(url);
}

function getCachePromise(url) {
   return  _$q.when(cache[url]);
}
