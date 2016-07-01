import loginDialog from './login.dialog.html';


export default function loginController($state, $translate, UserDialogService, LoginService) {
   'ngInject';
   let credentials = {
   };
   function _showLoginForm() {
      let buttons = [
         {
            name: $translate.instant('LOGIN.CANCEL'),
            func: () => $state.reload()
         },
         {
            name: $translate.instant('LOGIN.OK'),
            func: _login,
            needValidate: true
         }
      ];

      UserDialogService
         .dialog($translate.instant('LOGIN.MESSAGE'),
         loginDialog,
         buttons,
         { credentials });
   }

   function _login() {
      LoginService.login(credentials).then(() => {
         $state.go('home');
      });
   }

   _showLoginForm();
};
