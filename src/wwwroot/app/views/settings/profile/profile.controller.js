export default function ProfileController($scope, $element, UserService, SettingsService) {
   'ngInject';

   let vm = $scope;

   vm.user = {};
   vm.asEdit = false;

   function _init() {
      SettingsService.addOnSubmitListener(_onSubmit);
      SettingsService.addOnCancelListener(_onCancel);
      SettingsService.addOnEditListener(_onEdit);
      SettingsService.setAsEdit(vm.asEdit);
      $element.on('$destroy', _onDestroy);
   }
   _init();

   function _onDestroy() {
      SettingsService.removeOnSubmitListener(_onSubmit);
   }

   function _onSubmit() {
      console.log('submit');
      vm.asEdit = false;
      SettingsService.setAsEdit(vm.asEdit);
   }

   function _onCancel() {
      console.log('cancel');
   }

   function _onEdit() {
      console.log('edit');
      vm.asEdit = true;
      SettingsService.setAsEdit(vm.asEdit);
   }

   function _getAuthUser () {
      vm.user.FirstName = 'Administrator';
      vm.user.LastName = 'Admin';
      vm.user.MiddleName = 'Adminovich';
      vm.user.IsMale = 'true';
   }
   _getAuthUser();
}
