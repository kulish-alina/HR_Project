import './members.scss';
import {
   groupBy,
   keyBy,
   set
} from 'lodash';
export default function MembersController(
   $scope,
   $element,
   SettingsService,
   UserService,
   RolesService) {
   'ngInject';

   let vm   = $scope;
   vm.users = {};
   vm.selectGroup = _selectGroup;
   vm.currentGroup = null;

   function _init() {
      SettingsService.addOnSubmitListener(_onSubmit);
      SettingsService.addOnCancelListener(_onCancel);
      $element.on('$destroy', _onDestroy);
      _initMembers();
   }
   _init();

   function _onDestroy() {
      SettingsService.removeOnSubmitListener(_onSubmit);
      SettingsService.removeOnCancelListener(_onCancel);
   }

   function _onSubmit() {
   }

   function _onCancel() {
   }

   function _initMembers() {
      UserService.getUsers().then((res) => set(vm, 'users', groupBy(res, 'roleId')));
      RolesService.getRoles().then((res) => set(vm, 'roles', keyBy(res, 'id')));
   }

   function _selectGroup(id) {
      vm.currentGroup = vm.users[id];
   }
}
