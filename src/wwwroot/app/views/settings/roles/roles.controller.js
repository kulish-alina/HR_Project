import './roles.scss';
import addRoleDialog from './add-role-dialog.view.html';
import {
   reduce,
   forEach,
   values,
   flatten,
   keys,
   first,
   omit,
   set,
   keyBy,
   flow
} from 'lodash';

export default function RolesController(
   $scope,
   $element,
   $state,
   $filter,
   $translate,
   $window,
   RolesService,
   SettingsService,
   UserDialogService,
   UserService,
   LocalStorageService
   ) {
   'ngInject';

   /*---api---*/
   let vm = $scope;

   vm.roles           = null;
   vm.permissions     = null;
   vm.currentRole     = {};
   vm.newRole         = {title: ''};
   vm.currentRoleName = LocalStorageService.get('currentRoleName') || '';
   vm.getFlag         = _getFlag;
   vm.setFlag         = _setFlag;
   vm.setAll          = _setAll;
   vm.selectRole      = _selectRole;
   vm.removeRole      = _removeRole;
   vm.showAddDialog   = _showAddDialog;
   vm.clearModalModel = _clearModalModel;

   /*---impl---*/
   (function _init() {
      SettingsService.addOnSubmitListener(_onSubmit);
      SettingsService.addOnCancelListener(_onCancel);
      $element.on('$destroy', _onDestroy);
      $window.onbeforeunload = _onDestroy;
      _initRoles();
      _initPermissions();
   }());

   function _onDestroy() {
      SettingsService.removeOnSubmitListener(_onSubmit);
      SettingsService.removeOnCancelListener(_onCancel);
      LocalStorageService.set('currentRoleName', vm.currentRoleName);
   }

   function _onSubmit() {
      vm.roles[vm.currentRoleName].permissions = 0;
      forEach(vm.currentRole, (value) => {
         if (value.flag) {
            _setFlag(vm.currentRoleName, value.id);
         }
      });
      return RolesService.saveRole(vm.roles[vm.currentRoleName]);
   }

   function _onCancel() {
      return $state.reload('roles');
   }

   function _initRoles() {
      RolesService.getRoles()
         .then((rol) => set(vm, 'roles', keyBy(rol, 'title')))
         .then(vm.currentRoleName ? vm.selectRole(vm.currentRoleName) : _selectFirstRole);
   }

   function _initPermissions() {
      RolesService.getPermissions().then((value) => {
         vm.permissions = value;
      });
   }
   function _selectRole(roleName) {
      vm.currentRoleName = roleName;
      vm.currentRole = reduce(flatten(values(vm.permissions)), (memo, perm) => {
         memo[perm.id] = {};
         memo[perm.id].flag = _getFlag(roleName, perm.id);
         memo[perm.id].id = perm.id;
         return memo;
      }, {});
   };

   function _createNewRole() {
      vm.newRole.permissions = 0;
      RolesService.saveRole(vm.newRole).then(() => {
         vm.roles[vm.newRole.title] = vm.newRole;
         _clearModalModel();
         UserDialogService.notification(
            $translate.instant('ROLES.CREATED', {title: vm.newRole.title}), 'success');
      });
   }

   function _showAddDialog() {
      let buttons = [ {
         name:         $translate.instant('COMMON.CREATE'),
         func:         _createNewRole,
         needValidate: true
      } ];
      vm.newRole = {title: ''};
      let scope = {
         newRole: vm.newRole
      };
      UserDialogService.dialog(
         $translate.instant('ROLES.NEW'),
         addRoleDialog,
         buttons,
         scope);
   }

   function _removeRole(roleName) {
      UserService.getUsers((user) => {
         return user.roleId === vm.roles[roleName].id;
      }).then((val) => {
         if (val.length > 0) {
            let users = $filter('arrayAsString')(val, 'login', '; ');
            UserDialogService.confirm($translate.instant('ROLES.REDIRECT', {users})).then(() => {
               $state.go('members');
            });
         } else {
            RolesService.removeRole(vm.roles[roleName]).then(() => {
               UserDialogService.confirm(
                  $translate.instant('ROLES.CONFIRM', {title: roleName})).then(() => {
                     vm.roles = omit(vm.roles, roleName);
                     _selectFirstRole();
                     UserDialogService.notification($translate.instant('ROLES.REMOVED'), 'success');
                  });
            });
         }
      });
   }

   function _selectFirstRole() {
      let _fnc = flow(keys, first, _selectRole);
      _fnc(vm.roles);
   }

   function _clearModalModel() {
      vm.newRole = { title : '' };
   }

   function _getFlag(roleName, bitNumber) {
      let value = 1 << bitNumber;                               // eslint-disable-line no-bitwise
      return (vm.roles[roleName].permissions & value) === value; // eslint-disable-line no-bitwise
   }

   function _setFlag(roleName, bitNumber) {
      let value = 1 << bitNumber;                // eslint-disable-line no-bitwise
      vm.roles[roleName].permissions =
         vm.roles[roleName].permissions | value; // eslint-disable-line no-bitwise
   }

   function _setAll(value) {
      forEach(vm.currentRole, (val) => {
         val.flag = value;
      });
   }
}
