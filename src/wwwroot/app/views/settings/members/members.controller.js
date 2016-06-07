export default function MembersController($scope, $element, SettingsService, UserService) {
   'ngInject';

   let vm   = $scope;
   vm.users = [];
   function _init() {
      SettingsService.addOnSubmitListener(_onSubmit);
      SettingsService.addOnCancelListener(_onCancel);
      $element.on('$destroy', _onDestroy);
      _initUsers();
   }
   _init();

   function _onDestroy() {
      SettingsService.removeOnSubmitListener(_onSubmit);
      SettingsService.removeOnCancelListener(_onCancel);
   }

   function _onSubmit() {
   }

   function _onCancel() {
   }

   function _initUsers() {
      UserService.getUsers().then((result) => {
         vm.users = result;
      });
   }
}
