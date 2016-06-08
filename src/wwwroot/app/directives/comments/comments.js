import template from './comments.directive.html';
import './comments.scss';
export default class CommentsDirective {
   constructor() {
      this.restrict = 'E';
      this.template = template;
      this.scope = {
         type : '='
      };
      this.controller = CommentsController;
   }
   static createInstance() {
      'ngInject';
      CommentsDirective.instance = new CommentsDirective();
      return CommentsDirective.instance;
   }
}

function CommentsController($scope, $translate) {
   'ngInject';
   const vm = $scope;
   
   let commentType {
      note: _$translate.instant('COMMON.NOTES'),
      comment: _$translate.instant('COMMON.COMMENTS')
   };
   

}
