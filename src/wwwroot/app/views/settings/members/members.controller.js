import './members.scss';
import inviteDialogView from './invite-dialog.view.html';
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
   vm.setMovedUser   = _setMovedUser;
   vm.currentGroup   = null;
   vm.currentGroupId = null;

   let movedUser     = null;

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

   function _changeRole(role, user = movedUser) {
      user.roleId = role.id;
      UserService.saveUser(user).then(() => {
         remove(vm.users[vm.currentGroupId], user);
         vm.users[user.roleId] = vm.users[user.roleId] || [];
         vm.users[user.roleId].push(user);
         UserDialogService.notification($translate.instant('MEMBERS.CHANGED'), 'success');
      });
   }

   function _showInvite(role) {
      let buttons = [{
         name:         $translate.instant('COMMON.CANCEL')
      },{
         name:         $translate.instant('MEMBERS.INVITE_BUT'),
         func:         _createUser,
         needValidate: true
      }];

      vm.newUser = {
         login: '',
         email: ''
      };

      UserDialogService.dialog($translate.instant('MEMBERS.INVITE_MEMBER', {roleTitle: role.title}),
                               inviteDialogView,
                               buttons,
                               {newUser: vm.newUser});
   }

   function _createUser() {
      UserService.saveUser(vm.newUser).then((user) => {
         vm.users[user.roleId].push(user);
      });
   }

   function _removeUser(user) {
      UserDialogService.confirm($translate.instant('MEMBERS.CONFIRM', {login: user.login})).then(() => {
         UserService.removeUser(user).then(() => {
            remove(vm.users[vm.currentGroupId], user);
            UserDialogService.notification($translate.instant('MEMBERS.REMOVED'), 'success');
         });
      });
   }

   function _setMovedUser(user) {
      movedUser = user;
   }
}
