import template from './login.directive.html';
import './login.scss';


export default class LoginDirective {
   constructor() {
      this.restrict = 'E';
      this.template = template;
      this.scope = true;
      this.controller = LoginController;
      this.replace = true;
   }

   static createInstance() {
      'ngInject';
      LoginDirective.instance = new LoginDirective();
      return LoginDirective.instance;
   }
}

function LoginController(
   $q,
   $scope,
   $state,
   $translate,
   UserService,
   LoginService,
   LocalStorageService
   ) {
   'ngInject';

   let vm = $scope;
   vm.currentUser = UserService.getCurrentUser();
   vm.logout = _logout;
   vm.redirect = _redirectToProfile;

   function _logout() {
      LoginService.logout().then(() => {
         vm.currentUser = {};
         LocalStorageService.clear();
         $state.go('login');
      });
   }

   function _redirectToProfile() {
      $state.go('profile');
   }
}
