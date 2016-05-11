import {
   reduce,
   forEach
} from 'lodash';

export default function RolesController($scope, $element, $state, RolesService, SettingsService) {
   'ngInject';

   /*---api---*/
   let vm = $scope;
   vm.roles = {};
   vm.permissions = {};
   vm.getFlag = _getFlag;
   vm.setFlag = _setFlag;
   vm.setAll = _setAll;

   vm.selectRole = function selectRole(roleName) {
      vm.currentRoleName = roleName;

      vm.currentRole = reduce(vm.permissions, (memo, perm, name) => {
         memo[name] = _getFlag(roleName, perm.id);
         return memo;
      }, {});
   };
   //vm.setAll = _setAll;

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
      vm.roles[vm.currentRoleName] = 0;
      forEach(vm.currentRole, (value, key) => {
         if (value) {
            _setFlag(vm.currentRoleName, vm.permissions[key].id);
         }
      });
      let role = {};
      role[vm.currentRoleName] = vm.roles[vm.currentRoleName];
      return RolesService.saveRole(role);
   }

   function _onCancel() {
      return $state.reload('roles');
   }

   function _onDestroy() {
      SettingsService.removeOnSubmitListener(_onSubmit);
      SettingsService.removeOnCancelListener(_onCancel);
   }

   function _setAll(value) {
      vm.currentRole = reduce(vm.permissions, (memo, val, name) => {
         memo[name] = value;
         return memo;
      }, {});

   }
   function _getRoles() {
      vm.roles = RolesService.getRoles();
   }

   function _getPermissions() {
      RolesService.getPermissions().then((value) => {
         vm.permissions = value;
         vm.selectRole(Object.keys(vm.roles)[0]);
      });
   }

   function _getFlag(roleName, bitNumber) {
      let value = 1 << bitNumber;             // eslint-disable-line no-bitwise
      return (vm.roles[roleName] & value) === value; // eslint-disable-line no-bitwise
   }

   function _setFlag(roleName, bitNumber) {
      let value = 1 << bitNumber; // eslint-disable-line no-bitwise
      vm.roles[roleName] = vm.roles[roleName] | value; // eslint-disable-line no-bitwise
   }
}
