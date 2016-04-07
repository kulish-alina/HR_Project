import template from './thesaurus.directive.html';
import { hasIn } from 'lodash';

export default class ThesaurusDirective {
   constructor() {
      this.restrict = 'E';
      this.template = template;
      this.scope = {
         name : '@'
      };
      this.controller = ThesaurusController;
   };

   static createInstance($templateCache) {
      'ngInject';
      ThesaurusDirective.instance = new ThesaurusDirective($templateCache);
      return ThesaurusDirective.instance;
   }
}

function ThesaurusController($scope, ThesauruseService) {

   'ngInject';

   const vm = $scope;
   vm.topics = [ { title : 'ru'} ];
   vm.structure = { } ;
   vm.newTresauruseTopic = { } ;
   vm.editThesaurusTopic = editThesaurusTopic;
   vm.saveThesaurusTopic = saveThesaurusTopic;
   vm.cancelThesaurusTopicEditing = cancelThesaurusTopicEditing;
   vm.deleteThesauruseTopic = deleteThesaurusTopic;
   vm.isEdit = isEdit;
   getThesaurusStructure();
   //getThesaurusTopics();

   function isEdit(topic) {
      return hasIn(topic, 'clone');
   }

   function getThesaurusStructure() {
      vm.structure = ThesauruseService.getThesaurusStructure(vm.name);
   }

   function getThesaurusTopics() {
      vm.topics = ThesauruseService.getThesaurusTopics(vm.name)
         .catch(_onError);
   }

   function editThesaurusTopic(topic) {
      topic.clone = topic;
   }

   function cancelThesaurusTopicEditing(topic) {
      topic = topic.clone;
      delete topic.clone;
   }

   function saveThesaurusTopic(topic) {
      ThesauruseService.saveThesaurusTopic(vm.name, topic)
         .catch(_onError);
      delete topic.clone;
   }

   function deleteThesaurusTopic(topic) {
      ThesauruseService.deleteThesaurusTopic(vm.name, topic)
         .catch(_onError);
   }

   function _onError(message) {
      vm.errorMessage = 'Sorry! Some error occurred';
   }
}
