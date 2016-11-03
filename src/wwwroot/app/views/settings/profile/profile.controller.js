import './profile.scss';
import passwordChangePopup from './changePassword.view.html';
import utils from '../../../utils.js';

import {
   set,
   filter,
   first
} from 'lodash';

const LIST_OF_THESAURUS = [ 'city' ];

export default function ProfileController (
   $q,
   $scope,
   $element,
   $state,
   $translate,
   UserService,
   SettingsService,
   ValidationService,
   ThesaurusService,
   UserDialogService,
   AccountService,
   RolesService) {
   'ngInject';

   /*---api---*/
   let passwords         = {};
   let vm                = $scope;
   vm.form               = {};
   vm.user               = {};
   vm.uploader           = {};
   vm.convertDate        = utils.formatDateToServer;
   vm.showChangePassword = showChangePassword;
   vm.location           = location;

   /*---impl---*/
   (function _init() {
      SettingsService.addOnSubmitListener(_onSubmit);
      SettingsService.addOnCancelListener(_onCancel);
      SettingsService.addOnEditListener(_onEdit);
      $element.on('$destroy', _onDestroy);
      _initCurrentUser();
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS)
         .then(topics => set(vm, 'thesaurus', topics));
   }());

   function _onDestroy() {
      SettingsService.removeOnSubmitListener(_onSubmit);
      SettingsService.removeOnCancelListener(_onCancel);
      SettingsService.removeOnEditListener(_onEdit);
   }

   function _onSubmit() {
      ValidationService.validate(vm.form.userEdit).then(() => {
         return UserService.saveUser(vm.user, true).then(() => {
            $state.go('profile');
            UserDialogService.notification($translate.instant('PROFILE.CHANGED'), 'success');
         });
      }).catch((error) => {
         if (error.data) {
            UserDialogService.notification(error.data.message, 'error');
         }
         return $q.reject();
      });
   }

   function _onCancel() {
      _initCurrentUser();
      return $state.go('profile');
   }

   function _onEdit() {
      return $state.go('profile.edit');
   }

   function _initCurrentUser() {
      return UserService.getUserById(UserService.getCurrentUser().id, true)
         .then(user => {
            set(vm, 'user', user);
            RolesService.getById(user.roleId).then((role) => set(vm, 'role', role));
         });
   }

   function showChangePassword() {
      let buttons = [{
         name: $translate.instant('COMMON.CANCEL')
      }, {
         name: $translate.instant('COMMON.OK'),
         func: _changePassword,
         needValidate: true
      }];

      passwords = {
         oldPass : '',
         newPass : '',
         confPass: ''
      };

      let scope = {
         passwords
      };
      UserDialogService.dialog($translate.instant('PROFILE.CHANGE_PASS'),
                               passwordChangePopup, buttons, scope);
   }

   function _changePassword() {
      AccountService.changePassword(passwords.oldPass, passwords.newPass)
         .then(() => UserDialogService.notification($translate.instant('PROFILE.PAS_CHANGED'), 'success'))
         .catch((reason) => UserDialogService.notification(reason.data, 'error'));
   }

   function location() {
      return first(filter(vm.thesaurus.city, { id : vm.user.cityId })).title;
   }
}

