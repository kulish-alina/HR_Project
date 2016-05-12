const USER_URL = 'users/';
let _HttpService;

export default class UserService {
   constructor(HttpService) {
      'ngInject';
      _HttpService = HttpService;
   }

   getCurrentUser() {
      return {
         lastName    : 'Antonov',
         firstName   : 'Dmitriy',
         middleName  : 'Valentinovich',
         isMale      : 'true',
         birthDate   : '07.06.1989',
         location    : 'Dnniepropetrovsk',
         email       : 'antonov@mail.be',
         skype       : 'antonov_skype',
         login       : 'dant',
         role        : 'Manager',
         phoneNumber : '380680686868'
      };
   }

   saveUser(entty) {
      console.log('user saved', entty);
   }

   getUsers() {
      return _HttpService.get(USER_URL);
   }
}
