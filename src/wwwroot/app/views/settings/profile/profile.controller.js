export default function ProfileController($scope, UserService) {
   'ngInject';

   var vm = $scope;

   vm.user = {}

   function _getAuthUser () {
      vm.user = {
         FirstName : 'Administrator',
         LastName : 'Admin',
         MiddleName : 'Adminovich',
         IsMale : 'true'
      }
      //vm.user.FirstName = 'Administrator';
      //vm.user.LastName = 'Admin';
      //vm.user.MiddleName = 'Adminovich';
      //vm.user.IsMale = new Boolean(true);
   }
   _getAuthUser();
}
