export default function MembersController($element, $scope, UserService, SettingsService) {
   'ngInject';

   var vm = $scope;


      SettingsService.addOnSubmitListener(_onSubmit)
      $element.on('$destroy', _onDestroy);
   }
      SettingsService.addOnSubmitListener(_onSubmit)
      $element.on('$destroy', _onDestroy);

   function _onDestroy() {
      SettingsService.removeOnSubmitListener(_onSubmit)
   }

   function _onSubmit() {
      console.log('foo');
   }

}
