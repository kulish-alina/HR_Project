import './members.scss';
import inviteDialog from './invite-dialog.view.html';
import {
   groupBy,
   set,
   remove
} from 'lodash';
export default function MembersController(
   $q,
   $scope,
   $translate,
   SettingsService,
   UserService,
   RolesService,
   UserDialogService) {
   'ngInject';

   let vm            = $scope;
   vm.users          = {};
   vm.roles          = [];
   vm.newUser        = {};
   vm.selectGroup    = _selectGroup;
   vm.changeRole     = _changeRole;
   vm.showInvite     = _showInvite;
   vm.removeUser     = _removeUser;
   vm.currentGroup   = null;
   vm.currentGroupId = null;


   function _initData() {
      RolesService.getRoles().then((res) => set(vm, 'roles', res));
      UserService.getUsers().then((res) => {
         set(vm, 'users', groupBy(res, 'roleId'));
         _selectGroup(vm.roles[0].id);
      });
   }
   _initData();

   function _selectGroup(id) {
      vm.currentGroup = vm.users[id] || [];
      vm.currentGroupId = id;
   }

   function _changeRole(user, role) {
      user.roleId = role.id;
      UserService.saveUser(user).then(() => {
         remove(vm.users[vm.currentGroupId], user);
         vm.users[user.roleId] = vm.users[user.roleId] || [];
         vm.users[user.roleId].push(user);
         UserDialogService.notification('Role was succesfully changed', 'success');
      });
   }

   function _showInvite(role) {
      let buttons = [{
         name:         $translate.instant('COMMON.CREATE'),
         func:         _createUser,
         needValidate: true
      },{
         name:         $translate.instant('COMMON.CANCELL')
      }];

      UserDialogService.dialog($translate.instant('MEMBERS.INVITE_MEMBER', {roleTitle: role.title}),
                               inviteDialog,
                               buttons,
                               vm.newUser);
   }

   function _createUser() {
      UserService.saveUser(vm.newUser).then((user) => {
         vm.users[user.roleId].push(user);
      });
   }

   function _removeUser(user) {
      UserDialogService.confirm('Are you sure that yoy want to delete this user?').then(() => {
         UserService.removeUser(user).then(() => {
            remove(vm.users[vm.currentGroupId], user);
            UserDialogService.notification('User was succesfully removed', 'success');
         });
      });
   }
}
