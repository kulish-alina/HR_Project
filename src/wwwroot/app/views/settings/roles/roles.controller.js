export default function RolesController($scope, $element, $state, RolesService, SettingsService) {
   'ngInject';

   /*---api---*/
   let vm = $scope;
   vm.roles = {};
   vm.permissions = {};
   vm.getFlag = _getFlag;
   vm.setFlag = _setFlag;

   /*---impl---*/
   function _init() {
      SettingsService.addOnSubmitListener(_onSubmit);
      SettingsService.addOnCancelListener(_onCancel);
      $element.on('$destroy', _onDestroy);
      _getRoles();
      _getPermissions();
   }
   _init();

   function _onSubmit() {
      return RolesService.saveRoles(vm.roles);
   }

   function _onCancel() {
      return $state.reload('roles');
   }

   function _onDestroy() {
      SettingsService.removeOnSubmitListener(_onSubmit);
      SettingsService.removeOnCancelListener(_onCancel);
   }

   function _getRoles() {
      vm.roles = RolesService.getRoles();
   }

   function _getPermissions() {
      RolesService.getPermissions()
         .then((value) => vm.permissions = value);
   }

   function _getFlag(role, bitNumber) {
      let value = 1 << bitNumber;             // eslint-disable-line no-bitwise
      return (role & value) === value; // eslint-disable-line no-bitwise
   }

   function _setFlag(roleName, bitNumber) {
      let value = 1 << bitNumber; // eslint-disable-line no-bitwise
      if (_getFlag(vm.roles[roleName], bitNumber)) {
         vm.roles[roleName] = vm.roles[roleName] ^ value; // eslint-disable-line no-bitwise
      } else {
         vm.roles[roleName] = vm.roles[roleName] | value; // eslint-disable-line no-bitwise
      }
   }
}
