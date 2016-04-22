export default function MembersController($element, $scope, UserService, SettingsService) {
   'ngInject';

   var vm = $scope;


   function _init() {
      SettingsService.addOnSubmitListener(_onSubmit)
      SettingsService.addOnCancelListener(_onCancel)
      $element.on('$destroy', _onDestroy);
   }
   _init();
   /*(function _init() {
      SettingsService.addOnSubmitListener(_onSubmit)
      $element.on('$destroy', _onDestroy);
   })();*/

   function _onDestroy() {
      SettingsService.removeOnSubmitListener(_onSubmit)
   }

   function _onSubmit() {
      console.log('submit');
   }

   function _onCancel() {
      console.log('cancell');
   }

}
