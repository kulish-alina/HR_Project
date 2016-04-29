/*const USER_URL = 'users/';
let _HttpService;*/

export default class UserService {
   constructor(/*HttpService*/) {
      'ngInject';
      /*_HttpService = HttpService;*/
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
         phoneNumber : '0680686868'
      };
   }

   saveUser(entty) {
      console.log('user saved', entty);
   }
   /*getUsers() {
      return _HttpService.get(USER_URL);
   }

   getUser(id) {
      let additionalUrl = USER_URL + id;
      return _HttpService.get(additionalUrl);
   }

   saveUser(entity) {
      if (entity.Id) {
         let additionalUrl = USER_URL + entity.Id;
         return _HttpService.put(additionalUrl, entity);
      } else {
         return _HttpService.post(USER_URL, entity);
      }
   }

   deleteUser(entity) {
      let additionalUrl = USER_URL + entity.Id;
      _HttpService.remove(additionalUrl, entity);
   }*/
}
