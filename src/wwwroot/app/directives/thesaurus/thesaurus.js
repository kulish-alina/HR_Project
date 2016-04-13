import template from './thesaurus.directive.html';
import { has, clone, assign } from 'lodash';

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

function ThesaurusController($scope, ThesaurusService) {

   'ngInject';

   const vm = $scope;
   vm.topics = [];
   vm.structure = { } ;
   vm.newTresaurusTopic = { } ;
   vm.editThesaurusTopic = editThesaurusTopic;
   vm.saveEditTopic = saveEditTopic;
   vm.addNewTopic = addNewTopic;
   vm.cancelThesaurusTopicEditing = cancelThesaurusTopicEditing;
   vm.removeThesaurusTopic = deleteThesaurusTopic;
   vm.isEditTopic = isEditTopic;
   vm.isTopicEditAllow = isTopicEditAllow;
   _getThesaurusStructure();
   _getThesaurusTopics();
   var editTopicClone = null;

   function isEditTopic(topic) {
      return _isHasEditTopic() && _isEditTopic(topic);
   }

   function isTopicEditAllow(topic) {
      return !_isHasEditTopic() || _isEditTopic(topic);
   }

   function editThesaurusTopic(topic) {
      editTopicClone = clone(topic);
   }

   function cancelThesaurusTopicEditing(topic) {
      assign(topic, editTopicClone);
      _deleteClone();
   }

   function addNewTopic(topic) {
      _saveThesaurusTopic(topic);
      vm.newTresaurusTopic = { } ;
   }

   function saveEditTopic(topic) {
      _saveThesaurusTopic(topic);
      _deleteClone();
   }

   function deleteThesaurusTopic(topic) {
      ThesaurusService.deleteThesaurusTopic(vm.name, topic)
         .catch(_onError);
   }

   function _getThesaurusStructure() {
      vm.structure = ThesaurusService.getThesaurusStructure(vm.name);
   }

   function _getThesaurusTopics() {
      ThesaurusService.getThesaurusTopics(vm.name)
         .then(topics => vm.topics = topics)
         .catch(_onError);
   }

   function _isEditTopic(topic) {
      return editTopicClone.id === topic.id;
   }

   function _isHasEditTopic() {
      return editTopicClone !== null;
   }

   function _saveThesaurusTopic(topic) {
      ThesaurusService.saveThesaurusTopic(vm.name, topic)
         .catch(_onError);
   }

   function _deleteClone() {
      editTopicClone = null;
   }

   function _onError(message) {
      vm.message = 'Sorry! Some error occurred: ' + message;
   }
}
