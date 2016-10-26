import './profile.scss';
import passwordChangePopup from './changePassword.view.html';
import utils from '../../../utils.js';

import {
   set
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
   UserDialogService) {
   'ngInject';

   /*---api---*/
   let passwords         = {};
   let vm                = $scope;
   vm.form               = {};
   vm.user               = {};
   vm.uploader           = {};
   vm.convertDate        = utils.formatDateToServer;
   vm.showChangePassword = showChangePassword;

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
         return UserService.saveUser(vm.user).then(() => {
            $state.go('profile');
         });
      }).catch(() => $q.reject);
   }

   function _onCancel() {
      _initCurrentUser();
      return $state.go('profile');
   }

   function _onEdit() {
      return $state.go('profile.edit');
   }

   function _initCurrentUser() {
      UserService.getUserById(UserService.getCurrentUser().id).then(response => set(vm, 'user', response));
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

      UserDialogService.dialog($translate.instant('PROFILE.CHANGE_PASS'),
                               passwordChangePopup, buttons, passwords);
   }

   function _changePassword() {
      UserService.changePassword(passwords.oldPass, passwords.newPass)
         .then(() => UserDialogService.notification($translate.instant('PROFILE.CHANGED'), 'success'))
         .catch((reason) => UserDialogService.notification(reason, 'error'));
   }
}

