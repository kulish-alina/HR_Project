import {
   set
} from 'lodash';

export default function UsersReportController(
   $scope,
   UserService
) {
   'ngInject';

   const vm                   = $scope;
   vm.users                   = [];
   vm.listOfLocations         = ['Dnipro', 'Zaporіzhzhya', 'Lviv', 'Berdyansk'];

   (function init() {
      UserService.getUsers().then(users => set(vm, 'users', users));
   }());

}
