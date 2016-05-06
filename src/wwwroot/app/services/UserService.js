const USER_URL = 'users/';
let _HttpService;

export default class UserService {
   constructor(HttpService) {
      'ngInject';
      _HttpService = HttpService;
   }

   getUsers() {
      'ngInject';
      console.log(_HttpService.get(USER_URL));
      return _HttpService.get(USER_URL);
   }
}
