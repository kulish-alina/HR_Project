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
         url: serverUrl + additionalUrl,
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
   _LoggerService.log('Response status:', response.status);
   return response.data;
}

function _errorCallback(response) {
   _LoggerService.error('Response status:', response.status);
   return _$q.reject(response);
}
