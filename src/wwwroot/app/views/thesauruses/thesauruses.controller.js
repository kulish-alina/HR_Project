export default function ThesaurusesController($scope, ThesaurusService) {
   'ngInject';
   var vm = $scope;
   vm.thesaurusesNames = ThesaurusService.getThesaurusNames();
}
