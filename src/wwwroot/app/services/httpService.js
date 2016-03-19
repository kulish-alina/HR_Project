const BASE_URL = 'http://localhost:53031/api/';

export default class HttpService {
   constructor($http) {
      'ngInject';
      this.http = $http;
   }

   getEntity(urlId) {
      return this.http({
         method: 'get',
         url: BASE_URL + urlId
      }).then(
         function successCallback(response) {
            console.log(response.data);
            return response.data;
         },
         function errorCallback(response) {
            console.log(response.status);
         });
   }

   addEntity(urlId, entity) {
      this.http({
         method: 'post',
         data: entity,
         url: BASE_URL + urlId,
         headers: { 'Content-Type': 'application/json' }
      }).then(
         function successCallback(response) {
            console.log(response.status);
         },
         function errorCallback(response) {
            console.log(response.status);
         });
   }
}
