import './recover.scss';

export default function recoverController(
   $scope,
   $state,
   $translate,
   AccountService,
   ValidationService,
   UserDialogService) {
   'ngInject';
   let vm = $scope;
   vm.loginOrEmail = '';
   vm.send = _send;

   function _send(form) {
      ValidationService
         .validate(form).then(() => {
            return AccountService.recoverAccount(vm.loginOrEmail);
         }).then(() => {
            UserDialogService.notification($translate.instant('RECOVER.CHANGED'), 'success');
            $state.go('login');
         }).catch(error => {
            if (error.data) {
               UserDialogService.notification(error.data, 'error');
            }
         });
   }
};
