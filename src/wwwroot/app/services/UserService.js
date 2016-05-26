import {
   filter,
   first
} from 'lodash';

const USER_URL = 'users/';
let _HttpService, _$q, _HttpCacheService;
let currentUser = {};

export default class UserService {
   constructor(HttpService, $q, HttpCacheService) {
      'ngInject';
      _HttpService = HttpService;
      _$q = $q;
      _HttpCacheService = HttpCacheService;
   }

   getCurrentUser() {
      return _$q.when(currentUser);
   }

   setCurrentUser(user) {
      currentUser = user;
   }

   getUserById(id) {
      return this.getUsers({id}).then(first);
   }

   saveUser(entity) {
      if (entity.id) {
         return _HttpService.put(`${USER_URL}/${entity}`);
      } else {
         return _HttpService.post(USER_URL, entity).then(user => {
            UserService.clearCache(USER_URL);
            return user;
         });
      }
   }

   getUsers(predicate) {
      return _HttpCacheService.get(USER_URL)
         .then(users => filter(users, predicate));
   }
}
