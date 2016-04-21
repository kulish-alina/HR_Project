export default function SettingsController($scope, ValidationService, SettingsService) {
   'ngInject';

   var vm = $scope;

   /* --- api --- */
   vm.submit   = SettingsService.onSubmit;
   vm.edit     = SettingsService.onEdit;
   vm.cancel   = SettingsService.onCancel;
}
