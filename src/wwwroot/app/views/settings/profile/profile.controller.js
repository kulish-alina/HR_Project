import './profile.scss';
import passwordChangePopup from './changePassword.view.html';
import utils from '../../../utils.js';

import {
   set,
   find
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
   RolesService,
   FileService,
   LoggerService
   ) {
   'ngInject';

   /*---api---*/
   let passwords         = {};
   let vm                = $scope;
   vm.form               = {};
   vm.user               = {};
   vm.uploader           = {};
   vm.thesaurus          = {};
   vm.convertDate        = utils.formatDateToServer;
   vm.showChangePassword = showChangePassword;
   vm.location           = location;
   vm.uploadPhoto        = uploadPhoto;
   /*---impl---*/
   (function _init() {
      SettingsService.addOnSubmitListener(_onSubmit);
      SettingsService.addOnCancelListener(_onCancel);
      SettingsService.addOnEditListener(_onEdit);
      $element.on('$destroy', _onDestroy);
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS)
         .then(topics => set(vm, 'thesaurus', topics));
      ThesaurusService.getOfficeLocations()
         .then(locations => set(vm, 'locations', locations));
      _initCurrentUser();
      _initImageUploader();
   }());

   function _onDestroy() {
      SettingsService.removeOnSubmitListener(_onSubmit);
      SettingsService.removeOnCancelListener(_onCancel);
      SettingsService.removeOnEditListener(_onEdit);
   }

   function _onSubmit() {
      ValidationService.validate(vm.form.userEdit).then(() => {
         return _saveUser(vm.user);
      }).catch((error) => {
         if (error.data) {
            UserDialogService.notification(error.data.message, 'error');
         }
         return $q.reject();
      });
   }

   function _saveUser(user) {
      return UserService.saveUser(user, true).then(() => {
         $state.go('profile');
         UserDialogService.notification($translate.instant('PROFILE.CHANGED'), 'success');
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
         name         : $translate.instant('COMMON.OK'),
         func         : _changePassword,
         needValidate : true,
         isAsync      : true
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
      return AccountService.changePassword(passwords.oldPass, passwords.newPass)
         .then(() => UserDialogService.notification($translate.instant('PROFILE.PAS_CHANGED'), 'success'))
         .catch((reason) => {
            UserDialogService.notification(reason.data, 'error');
            return $q.reject();
         });
   }

   function location() {
      return find(vm.thesaurus.city, { id : vm.user.cityId }).title;
   }

   function _initImageUploader() {
      let uploader = FileService.getFileUploader({ maxSize: 1024000 });
      uploader.onSuccessItem = (item, response, status, headers) => {
         LoggerService.log('onSuccessItem', item, response, status, headers);
         UserDialogService.notification($translate.instant('Success'), 'success');
         let parsedResponse = JSON.parse(item._xhr.response);
         if (vm.user.photo) {
            FileService.remove(vm.user.photo);
         }
         vm.user.photo = parsedResponse;
         _saveUser(vm.user);
      };
      uploader.onErrorItem = (fileItem, response, status, headers) => {
         LoggerService.error('onErrorItem', fileItem, response, status, headers);
         UserDialogService.notification($translate.instant('COMMON.FILE_UPLOADER_ERROR_MESSAGE'), 'error');
      };
      uploader.onAfterAddingFile = (item) => {
         uploader.uploadItem(item);
      };
      uploader.onWhenAddingFileFailed = () => {
         UserDialogService.notification($translate.instant('COMMON.FILE_UPLOADER_ERROR_MESSAGE'), 'error');
      };
      vm.imageUploader = uploader;
   }

   function uploadPhoto() {
      let nativeUploadButton = document.querySelector('#imageUploader');
      nativeUploadButton.click();
   }
}

