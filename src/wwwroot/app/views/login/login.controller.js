import './login.scss';

export default function loginController($scope, $state, $timeout, LoginService,
   SessionService, ValidationService, UserDialogService, spinnerService) {
   'ngInject';
   let vm = $scope;
   vm.login = _login;
   vm.credentials = {};

   vm.token = LoginService.token;

   $timeout(_loading, 1000);

   function _loading() {
      if (vm.token) {
         spinnerService.show('html5spinner');
         LoginService.setCurrentUser().then(() => {
            let state = SessionService.getStateToRedirect();
            let param = SessionService.getRedirectParams();
            $state.go(state.name ? state.name : 'home', param);
         }).catch(error => {
            if (error) {
               UserDialogService.notification(error.data.message, 'error');
            }
            LoginService.token = undefined;
         }).finally(() => {
            spinnerService.hide('html5spinner');
         });
      }
   }

   function _login(form) {
      ValidationService
         .validate(form).then(() => {
            spinnerService.show('html5spinner');
            return LoginService.login(vm.credentials);
         }).then(() => {
            return LoginService.setCurrentUser();
         }).then(() => {
            let state = SessionService.getStateToRedirect();
            $state.go(state.name ? state.name : 'home');
         }).catch(error => {
            if (error) {
               UserDialogService.notification(error.data.message, 'error');
            }
         }).finally(() => {
            spinnerService.hide('html5spinner');
         });
   }
};
