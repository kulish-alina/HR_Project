import './roles.scss';
import {
   reduce,
   forEach,
   values,
   flatten
} from 'lodash';

export default function RolesController($scope, $element, $state, RolesService, SettingsService) {
   'ngInject';

   /*---api---*/
   let vm = $scope;
   vm.roles = {};
   vm.permissions = {};
   vm.currentRole = {};
   vm.currentRoleName = '';
   vm.getFlag = _getFlag;
   vm.setFlag = _setFlag;
   vm.setAll = _setAll;
   vm.selectRole = _selectRole;

   /*---impl---*/
   function _init() {
      SettingsService.addOnSubmitListener(_onSubmit);
      SettingsService.addOnCancelListener(_onCancel);
      $element.on('$destroy', _onDestroy);
      _initRoles();
      _initPermissions();
   }
   _init();

   function _onDestroy() {
      SettingsService.removeOnSubmitListener(_onSubmit);
      SettingsService.removeOnCancelListener(_onCancel);
   }

   function _onSubmit() {
      vm.roles[vm.currentRoleName] = 0;
      forEach(vm.currentRole, (value) => {
         if (value.flag) {
            _setFlag(vm.currentRoleName, value.id);
         }
      });
      let role = {};
      role.title = vm.currentRoleName;
      role.value = vm.roles[vm.currentRoleName];
      return RolesService.saveRole(role);
   }

   function _onCancel() {
      return $state.reload('roles');
   }

   function _initRoles() {
      RolesService.getRoles().then((value) => {
         vm.roles = value;
      });
   }

   function _initPermissions() {
      RolesService.getPermissions().then((value) => {
         vm.permissions = value;
         vm.selectRole(Object.keys(vm.roles)[0]);
      });
   }
   function _selectRole(roleName) {
      vm.currentRoleName = roleName;
      vm.currentRole = reduce(flatten(values(vm.permissions)), (memo, perm) => {
         memo[perm.name] = {};
         memo[perm.name].flag = _getFlag(roleName, perm.id);
         memo[perm.name].id = perm.id;
         return memo;
      }, {});
   };

   function _getFlag(roleName, bitNumber) {
      let value = 1 << bitNumber;                    // eslint-disable-line no-bitwise
      return (vm.roles[roleName] & value) === value; // eslint-disable-line no-bitwise
   }

   function _setFlag(roleName, bitNumber) {
      let value = 1 << bitNumber;                      // eslint-disable-line no-bitwise
      vm.roles[roleName] = vm.roles[roleName] | value; // eslint-disable-line no-bitwise
   }

   function _setAll(value) {
      forEach(vm.currentRole, (val) => {
         val.flag = value;
      });
   }
}
