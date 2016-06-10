import { remove } from 'lodash';
import template from './comments.directive.html';
import './comments.scss';
export default class CommentsDirective {
   constructor() {
      this.restrict = 'E';
      this.template = template;
      this.scope = {
         type : '@',
         comments: '=',
         save: '='
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

   const vm            = $scope;
   vm.typeWrap         = _getType();
   vm.currentComment   = {};
   vm.comments         = vm.comments || [];
   vm.addComment       = addComment;
   vm.editComment      = editComment;
   vm.removeComment    = removeComment;

   function addComment() {
      console.log(vm);
      console.log(vm.save);
      vm.save(vm.currentComment).then(() => {
         vm.comments.push(vm.currentComment);
         vm.currentComment   = {};
      });
   }

   function removeComment(comment) {
      remove(vm.comments, comment);
   }

   function editComment(comment) {
      vm.currentComment = comment;
      remove(vm.comments, comment);
   }

   function _getType() {
      if (vm.type === 'note') {
         return $translate.instant('COMMON.NOTES');
      } else {
         return $translate.instant('COMMON.COMMENTS');
      }
   }
}
