import {
   isString
} from 'lodash';
let _$http, _$q, _LoggerService, serverUrl;

export default class HttpProvider {
   changeApiUrl(url) {
      serverUrl = url;
   }

   $get($http, $q, LoggerService) {
      'ngInject';
      _$http = $http;
      _$q = $q;
      _LoggerService = LoggerService;
      return new HttpService();
   }
}

class HttpService {
   get(additionalUrl, entity) {
      return this.ajax('get', additionalUrl, entity);
   }

   post(additionalUrl, entity) {
      return this.ajax('post', additionalUrl, entity);
   }

   put(additionalUrl, entity) {
      return this.ajax('put', additionalUrl, entity);
   }

   remove(additionalUrl, entity) {
      return this.ajax('delete', additionalUrl, entity);
   }

   ajax(method, additionalUrl, entity) {
      const options = {
         method,
         url: serverUrl + additionalUrl,
         headers: {
            Accept : 'application/json'
         }
      };
      if (method === 'get') {
         options.params = entity;
      } else if (entity) {
         options.headers['Content-Type'] = 'application/json';
         options.data = isString(entity) ? JSON.stringify(entity) : entity;
      }
      return _$http(options).then(_successCallback, _errorCallback);
   }
}

function _successCallback(response) {
   _LoggerService.debug('Response status:', response.status);
   return response.data;
}

function _errorCallback(response) {
   _LoggerService.error('Response status:', response.status);
   return _$q.reject(response);
}
