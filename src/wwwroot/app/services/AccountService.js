const PASSWORD_URL = 'account/password/';
const RECOVER_URL  = 'account/recover/';

let _HttpService;

export default class AccountService {
   constructor(HttpService) {
      'ngInject';
      _HttpService = HttpService;
   }

   changePassword(oldPassword, newPassword) {
      return _HttpService.post(PASSWORD_URL, {oldPassword, newPassword});
   }

   recoverAccount(loginOrEmail) {
      return _HttpService.post(RECOVER_URL, loginOrEmail);
   }
}
