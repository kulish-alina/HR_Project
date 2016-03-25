const BASE_URL = 'http://localhost:53031/api/';

export default class HttpService {
   constructor($http, $q) {
      'ngInject';
      this.http = $http;
      this.$q = $q;
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
      return this.http(options).then(this._successCallback, this._errorCallback.bind(this));
   }

   _successCallback(response) {
      console.log(response.status);
      return response.data;
   }
   _errorCallback(response) {
      console.log(response.status);
      return this.$q.reject(response);
   }
}
