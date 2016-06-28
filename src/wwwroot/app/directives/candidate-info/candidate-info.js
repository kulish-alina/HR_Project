import template from './candidate-info.directive.html';
import './candidate-info.scss';
export default class CandidateInfoDirective {
   constructor() {
      this.restrict   = 'E';
      this.template   = template;
      this.scope      = {
         candidate: '='
      };
      this.controller = CandidateInfoController;
   }
   static createInstance() {
      'ngInject';
      CandidateInfoDirective.instance = new CandidateInfoDirective();
      return CandidateInfoDirective.instance;
   }
}

function CandidateInfoController() {

}
