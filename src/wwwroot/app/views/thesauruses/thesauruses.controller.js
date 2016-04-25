export default function ThesaurusesController($scope, ThesaurusService) {
   'ngInject';
   const vm = $scope;
   vm.thesaurusesNames = ThesaurusService.getThesaurusNames();
}
