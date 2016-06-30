const url = 'account/';

let httpService;
let userService;
let storageService;
let loggerService;

const tokenInfo = 'access_token';

import utils from '../utils.js';

import {
   mapValues
} from 'lodash';

export default class LoginService {
   constructor(HttpService, UserService, LocalStorageService, LoggerService) {
      'ngInject';
      httpService = HttpService;
      storageService = LocalStorageService;
      userService = UserService;
      loggerService = LoggerService;
   }

   login(credentials) {
      let en = _convertToBase64(credentials);
      let entity = credentials;

      loggerService.debug(entity);
      loggerService.debug(en);

      return httpService.post(`${url}signin`, entity).then(user => {
         loggerService.debug('Logging into user', user);

         userService.setCurrentUser(user);
         storageService.set(tokenInfo, user.token);
         return user;
      });
   };

   logout() {
      return httpService.post(`${url}logout`).then(flag => {
         if (flag) {
            storageService.remove(tokenInfo);
            userService.setCurrentUser(null);
         }
         return flag;
      });
   }

   getUser(token) {
      return httpService.post(`${url}`, {token}).then(user => {
         loggerService.debug('Incoming user', user);

         userService.setCurrentUser(user);
         storageService.set(tokenInfo, user.token);
         return user;
      });
   }
}

function _convertToBase64(userEntity) {
   return mapValues(userEntity, prop => utils.toBase64(prop));
}
