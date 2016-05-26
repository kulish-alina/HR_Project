export default function MembersController($scope, $element, SettingsService) {
   'ngInject';

   let vm   = $scope;
   vm.users = [];
   function _init() {
      SettingsService.addOnSubmitListener(_onSubmit);
      SettingsService.addOnCancelListener(_onCancel);
      $element.on('$destroy', _onDestroy);
   }
   _init();

   function _onDestroy() {
      SettingsService.removeOnSubmitListener(_onSubmit);
      SettingsService.removeOnCancelListener(_onCancel);
   }

   function _onSubmit() {
      console.log('submit');
   }

   function _onCancel() {
      console.log('cancell');
   }

   
}
