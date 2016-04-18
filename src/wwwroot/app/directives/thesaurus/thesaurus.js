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

   /* --- api --- */
   vm.topics      = [];
   vm.structure   = {};
   vm.newTresaurusTopic = {};
   vm.additionThesaurusesStore    = {};
   vm.selectedObjectsOfEditeTopic = {};

   vm.isEditTopic       = isEditTopic;
   vm.addNewTopic       = addNewTopic;
   vm.saveEditTopic     = saveEditTopic;
   vm.isTopicEditAllow  = isTopicEditAllow;
   vm.isShowField       = isShowField;
   vm.change            = change;
   vm.editThesaurusTopic          = editThesaurusTopic;
   vm.cancelThesaurusTopicEditing = cancelThesaurusTopicEditing;
   vm.removeThesaurusTopic        = deleteThesaurusTopic;


   /* === impl === */
   let editTopicClone = null;
   (function _init() {
      _getThesaurusStructure();
      _getThesaurusTopics();
   }());

   function isShowField(field) {
      return field.type !== '';
   }

   function isEditTopic(topic) {
      return _isHasEditTopic() && _isEditTopic(topic);
   }

   function isTopicEditAllow(topic) {
      return !_isHasEditTopic() || _isEditTopic(topic);
   }

   function editThesaurusTopic(topic) {
      editTopicClone = clone(topic);
      _setSelectedObjects(topic);
   }

   function cancelThesaurusTopicEditing(topic) {
      assign(topic, editTopicClone);
      vm.additionThesaurusesStore = {};
      _deleteClone();
   }

   function addNewTopic(topic) {
      _saveThesaurusTopic(topic);
      vm.newTresaurusTopic = {};
   }

   function saveEditTopic(topic) {
      _saveThesaurusTopic(topic);
      _deleteClone();
   }

   function deleteThesaurusTopic(topic) {
      ThesaurusService.deleteThesaurusTopic(vm.name, topic).catch(_onError);
   }

   function _getThesaurusStructure() {
      vm.structure = ThesaurusService.getThesaurusStructure(vm.name);
      forEach(vm.structure.fields,
               field => {
                  if (has(field, 'refTo')) {
                     ThesaurusService.getThesaurusTopics(field.refTo)
                        .then(topics =>
                           vm.additionThesaurusesStore[field.refTo] = topics)
                        .catch(_onError);
                  };
               });
   }

   function _getThesaurusTopics() {
      ThesaurusService.getThesaurusTopics(vm.name)
         .then(topics => vm.topics = topics).catch(_onError);
   }

   function getSelected(id, thesaurusRef) {
      return find(vm.additionThesaurusesStore[thesaurusRef], s => s.id === id);
   }

   function change(topic, field) {
      topic[field.name] =
         vm.selectedObjectsOfEditeTopic[field.name].id;
   }

   function _isEditTopic(topic) {
      return editTopicClone.id === topic.id;
   }

   function _isHasEditTopic() {
      return editTopicClone !== null;
   }

   function _saveThesaurusTopic(topic) {
      ThesaurusService.saveThesaurusTopic(vm.name, topic).catch(_onError);
   }

   function _setSelectedObjects(topic) {
      forEach(_getSelectFields(), field => {
         vm.selectedObjectsOfEditeTopic[field.name] =
            find(vm.additionThesaurusesStore[field.refTo], storeTopic =>
               storeTopic.id === topic[field.name])
      });
   }

   function _getSelectFields() {
      return filter(vm.structure.fields, field => has(field, 'refTo'));
   }

   function _deleteClone() {
      editTopicClone = null;
   }

   function _onError(message) {
      vm.message = 'Sorry! Some error occurred: ' + message;
   }
}
