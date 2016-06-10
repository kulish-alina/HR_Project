import { remove } from 'lodash';
import template from './comments.directive.html';
import './comments.scss';
export default class CommentsDirective {
   constructor() {
      this.restrict = 'E';
      this.template = template;
      this.scope = {
         type : '@',
         comments: '=ngModel'
      };
      this.controller = CommentsController;
   }
   static createInstance() {
      'ngInject';
      CommentsDirective.instance = new CommentsDirective();
      return CommentsDirective.instance;
   }
}

function CommentsController($scope, $translate, UserService, CommentService) {

   const vm            = $scope;
   vm.typeWrap         = _getType();
   vm.currentComment   = {};
   vm.comments         = vm.comments || [];
   vm.addComment       = addComment;
   vm.editComment      = editComment;
   vm.removeComment    = removeComment;

   function addComment() {
      if (vm.type === 'note') {
         UserService.getUserById(10).then((user) => {
            vm.currentComment.userId = user.id;
         });
         CommentService.save(vm.currentComment).then((comment) => {
            vm.comments.push(comment);
         });
         vm.currentComment   = {};
      } else {
         vm.comments.push(vm.currentComment);
         vm.currentComment   = {};
      }
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
