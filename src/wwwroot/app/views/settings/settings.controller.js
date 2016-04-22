export default function SettingsController($scope, ValidationService, SettingsService) {
   'ngInject';

   let vm = $scope;

   /* --- api --- */
   vm.submit   = SettingsService.onSubmit;
   vm.edit     = SettingsService.onEdit;
   vm.cancel   = SettingsService.onCancel;
   vm.$watch(() => {
      return SettingsService.asEdit;
   },
   () => {
      vm.asEdit   = SettingsService.asEdit;
   });
}
