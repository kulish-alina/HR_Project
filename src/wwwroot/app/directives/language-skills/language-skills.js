import template from './language-skills.directive.html';

import './language-skills.scss';

import { forEach, set, curry } from 'lodash';

const LIST_OF_THESAURUS = ['language', 'languageLevel'];

let curriedSet = curry(set, 3);

export default class LanguageSkillsDirective {
   constructor() {
      this.restrict = 'E';
      this.template = template;
      this.scope = {
         selectedSkills : '='
      };
      this.controller = LanguageSkillsController;
   };

   static createInstance($templateCache) {
      'ngInject';
      LanguageSkillsDirective.instance = new LanguageSkillsDirective($templateCache);
      return LanguageSkillsDirective.instance;
   }
}

function LanguageSkillsController($scope, ThesaurusService) {
   'ngInject';

   const vm = $scope;

   (function _init() {
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS)
         .then(curriedSet(vm, 'thesauruses'))
         .then(_initLanguages);
   }());

   function _initLanguages() {
      vm.languages = [];
      forEach(vm.thesauruses.language, language => {
         vm.languages.push({
            language,
            languageLevel: {}
         });
         forEach(vm.thesauruses.languageLevel, languageLevel => {
            vm.languages.push({
               language,
               languageLevel
            });
         });
      });
   }
}
