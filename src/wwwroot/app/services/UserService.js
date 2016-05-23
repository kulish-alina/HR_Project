import {
   filter,
   first,
   isEmpty
} from 'lodash';

const USER_URL = 'users/';
let _HttpService, _$q;
let cache = [];
let currentUser = {};

export default class UserService {
   constructor(HttpService, $q) {
      'ngInject';
      _HttpService = HttpService;
      _$q = $q;
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
         return _HttpService.post(USER_URL, entity).then(pushUserToCache);
      }
   }

   getUsers(predicate) {
      if (isEmpty(cache)) {
         return _HttpService.get(USER_URL)
         .then(users => {
            cache = users;
            filter(cache, predicate);
         });
      } else {
         return _$q.when(filter(cache, predicate));
      }
   }
}

function pushUserToCache(user) {
   cache.push(user);
   return user;
}
