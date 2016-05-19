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
      });
   }

   saveUser(entty) {
      _$q.when(console.log('user saved', entty));
   }

   getUsers() {
      return _HttpService.get(USER_URL);
   }

   getUser(id) {
      let additionalUrl = USER_URL + id;
      return _HttpService.get(additionalUrl);
   }
}
