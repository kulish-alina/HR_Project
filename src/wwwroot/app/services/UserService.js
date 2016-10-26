import utils  from '../utils.js';

import {
   first,
   includes,
   toLower,
   map,
   cloneDeep,
   filter
} from 'lodash';

const USER_URL = 'user/';
//const PASSWORD_URL = 'password/';

let _HttpService, _$q, _HttpCacheService, _LoggerService, _UserService;
let currentUser = {};

export default class UserService {
   constructor(HttpService, $q, HttpCacheService, LoggerService) {
      'ngInject';
      _HttpService      = HttpService;
      _$q               = $q;
      _HttpCacheService = HttpCacheService;
      _LoggerService    = LoggerService;
      _UserService      = this;
   }

   getUserById(id) {
      return _UserService.getUsers({id}).then(first);
   }

   getCurrentUser() {
      return currentUser;
   }

   setCurrentUser(user) {
      currentUser = user;
   }

   saveUser(entity) {
      let _httpMethod =  entity.id ?
          (user) => _HttpService.put(`${USER_URL}${user.id}`, _convertToServer(user)) :
          (user) => _HttpService.post(USER_URL, _convertToServer(user));

      return _httpMethod(entity).then((user) => {
         if (user.id === _UserService.getCurrentUser().id) {
            _UserService.setCurrentUser(user);
            return user;
         }
      });
   }

   getUsers(predicate) {
      return _HttpCacheService.get(USER_URL).then(users => {
         return filter(map(users, _convertFromServer), predicate);
      });
   }

   autocomplete(searchString) {
      return _UserService.getUsers((user) => {
         return includes(toLower(user.firstName), toLower(searchString)) ||
            includes(toLower(user.lastName), toLower(searchString));
      });
   }

   getFullName(user) {
      return `${user.firstName} ${user.lastName}`;
   }

   removeUser(entity) {
      if (entity.id) {
         _HttpCacheService.clearCache(USER_URL);
         return _HttpService.remove(`${USER_URL}${entity.id}`, entity);
      } else {
         _LoggerService.debug('Can\'t remove user', entity);
         return _$q.reject('there is no user\'s id');
      }
   }

   changePassword(/*oldPassword, newPassword*/) {
      return _$q.reject('does not implemented');
      //return _HttpService.post(PASSWORD_URL, {oldPassword, newPassword});
   }
}

function _convertFromServer(user) {
   let clonedUser = cloneDeep(user);
   clonedUser.birthDate = utils.formatDateFromServer(user.birthDate);
   return clonedUser;
}

function _convertToServer(user) {
   let clonedUser = cloneDeep(user);
   clonedUser.birthDate = utils.formatDateToServer(user.birthDate);
   return clonedUser;
}
