import './profile.scss';
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
   UserService,
   SettingsService,
   ValidationService,
   ThesaurusService) {
   'ngInject';

   /*---api---*/
   let vm         = $scope;
   vm.form        = {};
   vm.user        = {};
   vm.uploader    = {};
   vm.convertDate = utils.formatDateToServer;

   /*---impl---*/
   function _init() {
      SettingsService.addOnSubmitListener(_onSubmit);
      SettingsService.addOnCancelListener(_onCancel);
      SettingsService.addOnEditListener(_onEdit);
      $element.on('$destroy', _onDestroy);
      _initCurrentUser();
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS)
         .then(topics => set(vm, 'thesaurus', topics));
   }
   _init();

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
}

