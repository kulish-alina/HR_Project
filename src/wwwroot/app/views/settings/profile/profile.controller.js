import './profile.scss';

import {
   includes
} from 'lodash';

export default function ProfileController (
   $q,
   $scope,
   $element,
   $state,
   UserService,
   SettingsService,
   ValidationService,
   FileUploader) {
   'ngInject';

   /*---api---*/
   let vm    = $scope;
   vm.form   = {};
   vm.user   = {};
   vm.uploader = {};
   vm.addNewPhone        = _addNewPhone;
   vm.cantAddNewPhone    = _cantAddNewPhone;
   vm.updatePhoneNumbers = _updatePhoneNumbers;

   /*---impl---*/
   function _init() {
      SettingsService.addOnSubmitListener(_onSubmit);
      SettingsService.addOnCancelListener(_onCancel);
      SettingsService.addOnEditListener(_onEdit);
      $element.on('$destroy', _onDestroy);
      _initCurrentUser();
      vm.uploader = _createNewUploader();
   }
   _init();

   function _onDestroy() {
      SettingsService.removeOnSubmitListener(_onSubmit);
      SettingsService.removeOnCancelListener(_onCancel);
      SettingsService.removeOnEditListener(_onEdit);
   }

   function _createNewUploader() {
      let newUploader = new FileUploader({
         url: './api/files'
      });
      return newUploader;
   }

   function _onSubmit() {
      if (ValidationService.validate(vm.form.userEdit)) {
         return UserService.saveUser(vm.user).then(() => {
            $state.go('profile');
         });
      } else {
         return $q.reject();
      };
   }

   function _onCancel() {
      _initCurrentUser().then(() => {
         return $state.go('profile');
      });
   }

   function _onEdit() {
      return $state.go('profile.edit');
   }

   function _initCurrentUser() {
      return UserService.getCurrentUser()
         .then((val) => {
            vm.user = val;
            vm.user.phoneNumbers = vm.user.phoneNumbers || [ '' ];
         });
   }

   function _addNewPhone() {
      vm.user.phoneNumbers.push('');
   }

   function _cantAddNewPhone() {
      return includes(vm.user.phoneNumbers, '');
   }

   function _updatePhoneNumbers(number, index) {
      if (number && !includes(vm.user.phoneNumbers, number)) {
         vm.user.phoneNumbers[index] = number;
      } else {
         vm.user.phoneNumbers.splice(index, 1);
      }
   }
}
