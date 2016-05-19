import './thesauruses.scss';

export default function ThesaurusesController($scope, ThesaurusService) {
   'ngInject';
   const vm = $scope;
   vm.thesaurusNames = ThesaurusService.getThesaurusNames();
   vm.currentThesaurusName = '';
}
