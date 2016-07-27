import { first } from 'lodash';

import { filter } from 'lodash/fp';

const USER_URL = 'user/';
let _HttpService, _$q, _HttpCacheService, _LoggerService;
let currentUser = {};

export default class UserService {
   constructor(HttpService, $q, HttpCacheService, LoggerService) {
      'ngInject';
      _HttpService      = HttpService;
      _$q               = $q;
      _HttpCacheService = HttpCacheService;
      _LoggerService    = LoggerService;
   }

   getUserById(id) {
      return this.getUsers({id}).then(first);
   }

   getCurrentUser() {
      return currentUser;
   }

   setCurrentUser(user) {
      currentUser = user;
   }

   saveUser(entity) {
      if (entity.id) {
         return _HttpService.put(`${USER_URL}${entity.id}`, entity);
      } else {
         return _HttpService.post(USER_URL, entity).then(user => {
            _HttpCacheService.clearCache(USER_URL);
            return user;
         });
      }
   }

   getUsers(predicate) {
      return _HttpCacheService.get(USER_URL).then(filter(predicate));
   }

   removeUser(entity) {
      if (entity.id) {
         _HttpCacheService.clearCache(USER_URL);
         return _HttpService.remove(`${USER_URL}${entity.id}`, entity);
      } else {
         _LoggerService.debug('Can\'t remove user', entity);
         return _$q.reject();
      }
   }
}
