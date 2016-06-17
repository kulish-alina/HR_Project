import './profile.scss';
import utils from '../../../utils.js';

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
   vm.uploader = {};

   /*---impl---*/
   function _init() {
      SettingsService.addOnSubmitListener(_onSubmit);
      SettingsService.addOnCancelListener(_onCancel);
      SettingsService.addOnEditListener(_onEdit);
      $element.on('$destroy', _onDestroy);
      _initCurrentUser();
   }
   _init();

   function _onDestroy() {
      SettingsService.removeOnSubmitListener(_onSubmit);
      SettingsService.removeOnCancelListener(_onCancel);
      SettingsService.removeOnEditListener(_onEdit);
   }

   function _onSubmit() {
      if (ValidationService.validate(vm.form.userEdit)) {
         vm.user.birthDate = utils.formatDateToServer(vm.user.birthDate);
         return UserService.saveUser(vm.user).then(() => {
            $state.go('profile');
            vm.user.birthDate = utils.formatDateFromServer(vm.user.birthDate);
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
      UserService.getCurrentUser()
         .then((val) => {
            vm.user = val;
            vm.user.phoneNumbers = vm.user.phoneNumbers || [ {} ];
            vm.user.birthDate = utils.formatDateFromServer(vm.user.birthDate);
         });
   }
}

