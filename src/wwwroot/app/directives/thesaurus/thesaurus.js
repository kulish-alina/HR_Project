import template from './thesaurus.directive.html';
import { has, clone, assign, forEach, filter } from 'lodash';
import './thesaurus.scss';

const MAX_SIZE_OF_FILE = 5120;

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

function ThesaurusController($scope, ThesaurusService, $translate, FileUploader) {
   'ngInject';

   const vm = $scope;

   /* --- api --- */
   vm.topics      = [];
   vm.newUploader = createNewUploader();
   vm.filterdFields     = {};
   vm.newTresaurusTopic = {};
   vm.fields            = [];
   vm.thesaurusNameLabel          = '';
   vm.additionThesaurusesStore    = {};

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

   function createNewUploader() {
      let newUploader = new FileUploader({
         url: './api/files'
         //onCompleteAll: _vs
      });
      newUploader.filters.push({
         name: 'sizeFilter',
         fn: function sizeFilter(item) {
            if (item.size <= MAX_SIZE_OF_FILE) {
               return true;
            }
         }
      });
      //newUploader.onSuccessItem = function onSuccessUpload(item) {
         //vm.vacancy.fileIds.push(item.id);
      //};
      return newUploader;
   }

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
   }

   function cancelThesaurusTopicEditing(topic) {
      assign(topic, editTopicClone);
      _deleteClone();
   }

   function addNewTopic(topic) {
      _saveThesaurusTopic(topic);
      vm.newThesaurusTopic = {};
   }

   function saveEditTopic(topic) {
      _saveThesaurusTopic(topic);
      _deleteClone();
   }

   function deleteThesaurusTopic(topic) {
      ThesaurusService.deleteThesaurusTopic(vm.name, topic).then(_initThesaurusTopics).catch(_onError);
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

   function change(topic, field) {
      topic[field.name] = topic[field.refObject].id;
   }

   function _isEditTopic(topic) {
      return editTopicClone.id === topic.id;
   }

   function _isHasEditTopic() {
      return editTopicClone !== null;
   }

   function _saveThesaurusTopic(topic) {
      ThesaurusService.saveThesaurusTopic(vm.name, topic).then(_initThesaurusTopics).catch(_onError);
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
