import {
   isString
} from 'lodash';

let _$http, _$q, _LoggerService, _$serializer, serverUrl;

const dataStructure = {
   'json': {
      'contentType': 'application/json',
      'dataTransform' : (entity) => {
         return isString(entity) ? JSON.stringify(entity) : entity;
      }
   },
   'url': {
      'contentType': 'application/x-www-form-urlencoded',
      'dataTransform' : (entity) => {
         return _$serializer(entity);
      }
   }
};

export default class HttpProvider {
   changeApiUrl(url) {
      serverUrl = url;
   }

   $get($http, $q, LoggerService, $httpParamSerializerJQLike) {
      'ngInject';
      _$http = $http;
      _$q = $q;
      _LoggerService = LoggerService;
      _$serializer = $httpParamSerializerJQLike;
      return new HttpService();
   }
}

class HttpService {
   get(additionalUrl, entity, type = 'json') {
      return this.ajax('get', additionalUrl, entity, type);
   }

   post(additionalUrl, entity, type = 'json') {
      return this.ajax('post', additionalUrl, entity, type);
   }

   put(additionalUrl, entity, type = 'json') {
      return this.ajax('put', additionalUrl, entity, type);
   }

   remove(additionalUrl, entity, type = 'json') {
      return this.ajax('delete', additionalUrl, entity, type);
   }

   ajax(method, additionalUrl, entity, type) {
      const options = {
         method,
         url: serverUrl + additionalUrl,
         headers: {
            Accept: 'application/json'
         }
      };
      if (method === 'get') {
         options.params = entity;
      } else if (entity) {
         options.headers['Content-Type'] = dataStructure[type].contentType;
         options.data = dataStructure[type].dataTransform(entity);
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
