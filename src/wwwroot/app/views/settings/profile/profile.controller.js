export default function ProfileController($scope, UserService) {
   'ngInject';

   var vm = $scope;

   vm.user = {}

   function _getAuthUser () {
      vm.user.FirstName = 'Administrator';
      vm.user.LastName = 'Admin';
      vm.user.MiddleName = 'Adminovich';
      vm.user.IsMale = 'true';
   }
   _getAuthUser();
}
