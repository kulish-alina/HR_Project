import template from './thesaurus.directive.html';
import { has, clone, assign, forEach, filter, isEmpty, find } from 'lodash';
import './thesaurus.scss';
const MAX_ICON_FILE_SIZE = 5120;

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

function ThesaurusController($element, $scope, ThesaurusService, $translate,
      FileService, LoggerService, UserDialogService) {
   'ngInject';

   const vm = $scope;

   /* --- api --- */
   vm.topics      = [];
   vm.uploader    = _createNewUploader();
   vm.filterdFields     = {};
   vm.newTresaurusTopic = {};
   vm.fields            = [];
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
      vm.uploader.clearQueue();
   }

   function addNewTopic(topic) {
      if (isEmpty(vm.uploader.getNotUploadedItems())) {
         _saveThesaurusTopic(topic).finally(clearNewThesaurusTopic);
      } else {
         vm.uploader.uploadAll();
      }
   }

   function saveEditTopic(topic) {
      if (isEmpty(vm.uploader.getNotUploadedItems())) {
         _saveThesaurusTopic(topic).then(_deleteClone).catch(() => cancelThesaurusTopicEditing(topic));
      } else {
         vm.uploader.uploadAll();
      }
   }

   function deleteThesaurusTopic(topic) {
      ThesaurusService.deleteThesaurusTopic(vm.name, topic).then(_initThesaurusTopics).catch(_onError);
   }

   function _initThesaurusStructure() {
      let structure = ThesaurusService.getThesaurusStructure(vm.name);
      vm.fields = filter(structure.fields, isShowField);
      vm.orderField = getOrderField();
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
      return ThesaurusService.saveThesaurusTopic(vm.name, topic).then(_initThesaurusTopics).catch(_onError);
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

   function _getEditTopic() {
      return editTopicClone ? find(vm.topics, {id: editTopicClone.id}) : vm.newThesaurusTopic;
   }

   function clearNewThesaurusTopic() {
      vm.newThesaurusTopic = {};
      let fileInput = $element[0].querySelector('.new-topic-uploader');
      if (fileInput) {
         vm.uploader.clearQueue();
         fileInput.value = null;
      }
   }

   function _createNewUploader() {
      let newUploader = FileService.getFileUploader({maxSize: MAX_ICON_FILE_SIZE});

      function saveTopic(topic) {
         _saveThesaurusTopic(topic)
            .then(_deleteClone)
            .catch(() => cancelThesaurusTopicEditing(topic))
            .finally(clearNewThesaurusTopic);
      }

      newUploader.onSuccessItem = (item, response, status, headers) => {
         let editTopic = _getEditTopic();
         let imageField = find(vm.fields, {type: 'img'});
         editTopic[imageField.name] = `${response.filePath}`;
         saveTopic(editTopic);
         LoggerService.log('onSuccessItem', item, response, status, headers);
      };
      newUploader.onErrorItem = (fileItem, response, status, headers) => {
         let editTopic = _getEditTopic();
         saveTopic(editTopic);
         vm.uploader.clearQueue();
         LoggerService.error('onErrorItem', fileItem, response, status, headers);
      };
      newUploader.onWhenAddingFileFailed = () => {
         UserDialogService.notification($translate.instant('COMMON.FILE_UPLOADER_ERROR_MESSAGE'), 'error');
      };

      return newUploader;
   }

   function _onError(message) {
      LoggerService.error(message);
      vm.message = $translate.instant('ERRORS.someErrorMsg') + message;
   }

   function getOrderField() {
      let orderField = find(vm.fields, {type : 'number'});
      return orderField ? orderField.name : 'createdOn';
   }
}
