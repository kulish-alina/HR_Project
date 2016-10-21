const url = 'account/';

let _$q,
   _httpService,
   _userService,
   _storageService,
   _loggerService;

const tokenInfo = 'access_token';

import utils from '../utils';

import {
   mapValues
} from 'lodash';

export default class LoginService {
   constructor($q, HttpService, UserService, LocalStorageService, LoggerService) {
      'ngInject';
      _$q = $q;
      _httpService = HttpService;
      _storageService = LocalStorageService;
      _userService = UserService;
      _loggerService = LoggerService;
   }

   login(credentials) {
      let en = _convertToBase64(credentials);
      let entity = credentials;

      _loggerService.debug(entity);
      _loggerService.debug(en);

      return _httpService.post(`${url}signin`, entity).then(user => {
         _loggerService.debug('Logging into user', user);

         _userService.setCurrentUser(user);
         _storageService.clear();
         _storageService.set(tokenInfo, user.token);
         return user;
      }).catch(error => {
         _loggerService.debug('Loggin failed', error.data.message);
         return _$q.reject(error);
      });
   };

   logout() {
      return _httpService.post(`${url}logout`).then(logoutResult => {
         if (logoutResult) {
            _storageService.remove(tokenInfo);
            _userService.setCurrentUser({});
         }
         return logoutResult;
      });
   }

   getUser(token) {
      return _httpService.post(`${url}`, { token }).then(user => {
         _loggerService.debug('Incoming user', user);

         _userService.setCurrentUser(user);
         _storageService.set(tokenInfo, user.token);
         return user;
      });
   }
}

function _convertToBase64(userEntity) {
   return mapValues(userEntity, prop => utils.toBase64(prop));
}
