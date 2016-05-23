const USER_URL = 'users/';
let _HttpService, _$q;

export default class UserService {
   constructor(HttpService, $q) {
      'ngInject';
      _HttpService = HttpService;
      _$q = $q;
   }

   getCurrentUser() {
      return _$q.when({
         lastName     : 'Antonov',
         firstName    : 'Dmitriy',
         middleName   : 'Valentinovich',
         isMale       : 'true',
         birthDate    : '07.06.1989',
         location     : 'Dnniepropetrovsk',
         email        : 'antonov@mail.be',
         skype        : 'antonov_skype',
         login        : 'dant',
         role         : 'Manager',
         phoneNumbers : ['380680686868', '380505055505']
      });
   }

   saveUser(entty) {
      return _$q.when(console.log('user saved', entty));
   }

   getUsers() {
      return _HttpService.get(USER_URL);
   }
}
