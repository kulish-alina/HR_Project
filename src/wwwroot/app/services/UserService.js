import {
   find,
   filter
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
      let userFromCache = find(cache, {id});
      if (userFromCache) {
         return _$q.when(userFromCache);
      } else {
         return _HttpService.get(`${USER_URL}/${id}`).then(pushUserToCache);
      }
   }

   saveUser(entity) {
      if (entity.id) {
         return _HttpService.put(`${USER_URL}/${entity}`);
      } else {
         return _HttpService.post(USER_URL, entity).then(pushUserToCache);
      }
   }

   getUsers(predicate) {
      return _HttpService.get(USER_URL)
         .then(users =>  cache = users)
         .then(() => filter(cache, predicate));
   }
}

function pushUserToCache(user) {
   cache.push(user);
   return user;
}
