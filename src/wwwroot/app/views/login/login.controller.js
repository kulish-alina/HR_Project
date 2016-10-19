export default function loginController($scope, SessionService, ValidationService, $state, LoginService) {
   'ngInject';
   let vm = $scope;
   vm.login = _login;
   vm.credentials = {

   };

   function _login(form) {
      ValidationService
         .validate(form).then(() => {
            return LoginService.login(vm.credentials);
         }).then(user => {
            //on undefined redirect to home
            let state = SessionService.getStateToRedirect();
            $state.go(state.name, state.data);
            return user;
         });;
   }
};
