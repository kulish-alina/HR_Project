import './login.scss';

export default function loginController($scope, $state, LoginService,
   SessionService, ValidationService, UserDialogService) {
   'ngInject';
   let vm = $scope;
   vm.login = _login;
   vm.credentials = {};

   function _login(form) {
      ValidationService
         .validate(form).then(() => {
            return LoginService.login(vm.credentials);
         }).then(() => {
            let state = SessionService.getStateToRedirect();
            $state.go(state.name ? state.name : 'home', state.data);
         }).catch(error => {
            UserDialogService.notification(error.data.message, 'error');
         });
   }
};
