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

   getUserById(id, needToConvert) {
      return _UserService.getUsers({id}, needToConvert).then(first);
   }

   getCurrentUser() {
      return currentUser;
   }

   setCurrentUser(user) {
      currentUser = user;
   }

   saveUser(entity, needToConvert) {
      let _httpMethod = entity.id ?
          (user) => _HttpService.put(`${USER_URL}${user.id}`, user) :
          (user) => _HttpService.post(USER_URL, user);

      let _convertedUser = needToConvert ? _convertToServer(entity) : entity;

      return _httpMethod(_convertedUser).then((user) => {
         if (user.id === _UserService.getCurrentUser().id) {
            _UserService.setCurrentUser(user);
         }
         _HttpCacheService.clearCache(USER_URL);
         return user;
      });
   }

   getUsers(predicate, needToConvert) {
      return _HttpCacheService.get(USER_URL).then(users => {
         return filter(map(users, (user) => {
            return needToConvert ? _convertFromServer(user) : user;
         }), predicate);
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
