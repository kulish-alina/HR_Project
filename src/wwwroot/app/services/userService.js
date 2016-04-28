import {
   cloneDeep
} from  'lodash';
/*const USER_URL = 'users/';
let _HttpService;*/

export default class UserService {
   constructor(/*HttpService*/) {
      'ngInject';
      /*_HttpService = HttpService;*/
   }

   getCurrentUser() {
      return cloneDeep({
         LastName   : 'Antonov',
         FirstName  : 'Dmitriy',
         MiddleName : 'Valentinovich',
         IsMale     : 'true',
         BirthDate  : '07.06.1989',
         Location   : 'Dnniepropetrovsk',
         Email      : 'antonov@mail.be',
         Skype      : 'antonov_skype',
         Login      : 'dant',
         Role       : 'Manager'
      });
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
