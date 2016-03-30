const BASE_URL = 'http://localhost:53031/api/';

let _$http, _$q, _LoggerService;

export default class HttpService {
   constructor($http, $q, LoggerService) {
      'ngInject';
      _$http = $http;
      _$q = $q;
      _LoggerService = LoggerService;
   }

   get(additionalUrl) {
      return this.ajax('get', additionalUrl);
   }

   post(additionalUrl, entity) {
      return this.ajax('post', additionalUrl, entity);
   }

   put(additionalUrl, entity) {
      return this.ajax('put', additionalUrl, entity);
   }

   remove(additionalUrl, entity) {
      this.ajax('delete', additionalUrl, entity);
   }

   ajax(method, additionalUrl, entity) {
      var options = {
         method: method,
         url: BASE_URL + additionalUrl,
         headers: {
            'Content-Type': 'application/json'
         }
      };
      if (entity) {
         options.data = entity;
      }
      return _$http(options).then(_successCallback, _errorCallback);
   }
}

function _successCallback(response) {
   _LoggerService.information('Response status:', response.status);
   return response.data;
}

function _errorCallback(response) {
   _LoggerService.error('Response status:', response.status);
   return _$q.reject(response);
}
