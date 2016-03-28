const BASE_URL = 'http://localhost:53031/api/';

export default class HttpService {
   constructor($http, $q, LoggerService) {
      'ngInject';
      this.http = $http;
      this.$q = $q;
      this.LoggerService = LoggerService;
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
      return this.http(options).then(this._successCallback.bind(this),
                                     this._errorCallback.bind(this));
   }

   _successCallback(response) {
      this.LoggerService.information(new Date (), 'Response status:', response.status,
                                     response.data);
      return response.data;
   }
   _errorCallback(response) {
      this.LoggerService.error(new Date (), 'Response status:', response.status);
      return this.$q.reject(response);
   }
}
