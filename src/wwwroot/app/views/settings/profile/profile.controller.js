import {
   floor
} from 'lodash';
const MAX_SIZE_OF_FILE = 5120;
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
   vm.age    = _age;
   vm.uploader = {};

   /*---impl---*/
   function _init() {
      SettingsService.addOnSubmitListener(_onSubmit);
      SettingsService.addOnCancelListener(_onCancel);
      SettingsService.addOnEditListener(_onEdit);
      $element.on('$destroy', _onDestroy);
      _getCurrentUser ();
      vm.uploader = _createNewUploader();
   }
   _init();

   function _createNewUploader() {
      let newUploader = new FileUploader({
         url: './api/files'
      });
      newUploader.filters.push({
         name: 'sizeFilter',
         fn: function sizeFilter(item) {
            if (item.size <= MAX_SIZE_OF_FILE) {
               return true;
            }
         }
      });
      return newUploader;
   }

   function _onSubmit() {
      if (ValidationService.validate(vm.form.userEdit)) {
         $state.go('profile');
         return UserService.saveUser(vm.user);
      } else {
         return $q.reject();
      };
   }

   function _onCancel() {
      _getCurrentUser ();
      return $state.go('profile');
   }

   function _onEdit() {
      return $state.go('profile.edit');
   }

   function _age () {
      return _calcAge(vm.user.birthDate);
   }

   function _getCurrentUser () {
      vm.user = UserService.getCurrentUser();
   }

   function _onDestroy() {
      SettingsService.removeOnSubmitListener(_onSubmit);
      SettingsService.removeOnCancelListener(_onCancel);
      SettingsService.removeOnEditListener(_onEdit);
   }
}

function _calcAge(dateString) {
   const msInYear = 31557600000;
   let parts = dateString.split('.');
   const dayBlock = 0;
   const monthBlock = 1;
   const yearBlock = 2;
   const metricSystemIndex = 10;
   const moveToZeroBasedSys = 1;
   let birthday  = new Date(
      parseInt(parts[yearBlock], metricSystemIndex),
      parseInt(parts[monthBlock] -  moveToZeroBasedSys, metricSystemIndex),
      parseInt(parts[dayBlock], metricSystemIndex));
   return floor((Date.now() - birthday) / (msInYear));
}
