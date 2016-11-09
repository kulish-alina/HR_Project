import './thesauruses.scss';

import { head } from 'lodash';

export default function ThesaurusesController($scope, ThesaurusService) {
   'ngInject';
   const vm = $scope;

   (function _init() {
      vm.thesaurusNames       = ThesaurusService.getThesaurusNames();
      vm.currentThesaurusName = head(vm.thesaurusNames) || '';
   } ());

}
