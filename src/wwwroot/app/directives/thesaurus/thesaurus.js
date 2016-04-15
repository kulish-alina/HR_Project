import template from './thesaurus.directive.html';
import { has, clone, assign, map, forEach, find, filter } from 'lodash';

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

function ThesaurusController($scope, ThesaurusService, $q) {

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
   vm.additionThesaurusesStore = { };
   vm.topicsSelectedOptions = { };
   vm.getSelected = getSelected;
   vm.change = change;
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
      forEach(vm.structure.fields,
               field => {
                  if (has(field, 'refTo')) {
                     ThesaurusService.getThesaurusTopics(field.refTo.thesaurusName)
                        .then(topics =>
                           vm.additionThesaurusesStore[field.refTo.thesaurusName] = topics)
                        .catch(_onError);
                  };
               });
   }

   function _getThesaurusTopics() {
      ThesaurusService.getThesaurusTopics(vm.name)
         .then(topics => {
            vm.topics = topics;
            var refFields = filter(vm.structure.fields, field => has(field, 'refTo'));
            forEach(topics, topic => {
               forEach(refFields, field => {
                  vm.topicsSelectedOptions[topic[field.refTo.labelFieldName]] = { };
                  vm.topicsSelectedOptions[topic[field.refTo.labelFieldName]][field.name] =
                     find(vm.additionThesaurusesStore[field.refTo.thesaurusName],
                        s => {
                           return s.id === topic[field.name];
                        });
               });
            });
         })
         .catch(_onError);
   }

   function getSelected(id, thesaurusRef) {
      return find(vm.additionThesaurusesStore[thesaurusRef], s => s.id === id);
   }

   function change(topic, field) {
      topic[field.name] =
         vm.topicsSelectedOptions[topic[field.refTo.labelFieldName]][field.name].id;
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
