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
      let additionalUrl = USER_URL + id;
      return _HttpService.get(additionalUrl);
   }
}
