const USER_URL = 'users/';
let _HttpService;

export default class UserService {
   constructor(HttpService) {
      'ngInject';
      _HttpService = HttpService;
   }

   getUsers() {
      return _HttpService.get(USER_URL);
   }

   getUser(id) {
      var additionalUrl = USER_URL + id;
      return _HttpService.get(additionalUrl);
   }

   saveUser(entity) {
      if (!!entity.Id) {
         var additionalUrl = USER_URL + entity.Id;
         return _HttpService.put(additionalUrl, entity);
      }
      else {
         return _HttpService.post(USER_URL, entity);
      }
   }

   deleteUser(entity) {
      var additionalUrl = USER_URL + entity.Id;
      _HttpService.remove(additionalUrl, entity);
   }
}
