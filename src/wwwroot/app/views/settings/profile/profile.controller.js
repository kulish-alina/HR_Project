import {
   floor
} from 'lodash';

export default function ProfileController (
   $q,
   $scope,
   $element,
   $state,
   UserService,
   SettingsService,
   ValidationService) {
   'ngInject';

   /*---api---*/
   let vm    = $scope;
   vm.form   = {};
   vm.user   = {};
   vm.age    = _age;

   /*---impl---*/
   function _init() {
      SettingsService.addOnSubmitListener(_onSubmit);
      SettingsService.addOnCancelListener(_onCancel);
      SettingsService.addOnEditListener(_onEdit);
      $element.on('$destroy', _onDestroy);
      _getCurrentUser ();
   }
   _init();

   function _onSubmit() {
      if (ValidationService.validate(vm.form.userEdit)) {
         $state.go('profile');
         return UserService.saveUser(vm.user);
      } else {
         return $q.reject();
      };
   }

   function _onCancel() {
      return $state.go('profile');
   }

   function _onEdit() {
      return $state.go('profile.edit');
   }

   function _age () {
      return _calcAge(vm.user.BirthDate);
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
   let birthday = new Date(dateString);
   return floor((Date.now() - birthday) / (msInYear));
}
