import './settings.scss';
export default function SettingsController(
   $scope,
   ValidationService,
   SettingsService) {
   'ngInject';

   let vm = $scope;

   /* --- api --- */
   vm.submit = _submit;
   vm.edit   = _edit;
   vm.cancel = _cancel;

   /* --- impl --- */

   function _submit() {
      SettingsService.onSubmit().then(() => {
         vm.asEdit = false;
      });
   }

   function _edit() {
      SettingsService.onEdit().then(() => {
         vm.asEdit = true;
      });
   }

   function _cancel() {
      SettingsService.onCancel().then(() => {
         vm.asEdit = false;
      });
   }
}
