import template from './thesaurus.directive.html';
import { has, clone, assign, forEach, find, filter } from 'lodash';
import './thesaurus.scss';

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

function ThesaurusController($scope, ThesaurusService, $translate) {
   'ngInject';

   const vm = $scope;

   /* --- api --- */
   vm.topics      = [];
   vm.filterdFields     = {};
   vm.newTresaurusTopic = {};
   vm.fields            = [];
   vm.thesaurusNameLabel          = '';
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
      _initThesaurusStructure();
      _initThesaurusTopics();
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

   function _initThesaurusStructure() {
      let structure = ThesaurusService.getThesaurusStructure(vm.name);
      vm.thesaurusNameLabel = $translate.instant(structure.thesaurusName);
      vm.fields = filter(structure.fields, isShowField);
      forEach(vm.fields, _fillAdditionThesauruses);
   }

   function _initThesaurusTopics() {
      ThesaurusService.getThesaurusTopics(vm.name)
         .then(topics => vm.topics = topics).catch(_onError);
   }

   function getSelected(id, thesaurusRef) {
      return find(vm.additionThesaurusesStore[thesaurusRef], {id});
   }

   function change(topic, field) {
      topic[field.name] = vm.selectedObjectsOfEditeTopic[field.name].id;
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
         vm.selectedObjectsOfEditeTopic[field.name] = getSelected(topic[field.name], field.refTo);
      });
   }

   function _getSelectFields() {
      return filter(vm.fields, {type: 'select'});
   }

   function _deleteClone() {
      editTopicClone = null;
   }

   function _fillAdditionThesauruses(field) {
      if (has(field, 'refTo')) {
         ThesaurusService.getThesaurusTopics(field.refTo)
            .then(topics => vm.additionThesaurusesStore[field.refTo] = topics).catch(_onError);
      };
   }

   function _onError(message) {
      vm.message = $translate.instant('ERRORS.someErrorMsg') + message;
   }
}