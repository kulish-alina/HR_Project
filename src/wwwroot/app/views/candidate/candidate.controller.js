import { set } from 'lodash';

const LIST_OF_THESAURUS = ['industries', 'levels', 'locations', 'languages', 'languageLevels',
    'departments', 'typesOfEmployment', 'tags', 'skills', 'stages', 'currencies'];

export default function CandidateController(
   $element,
   $scope,
   $translate,
   CandidateService,
   ValidationService,
   FileService,
   ThesaurusService,
   UserDialogService,
   LoggerService
   ) {
   'ngInject';

   const vm          = $scope;
   vm.keys           = Object.keys;
   vm.candidate      = vm.candidate || {};
   vm.thesaurus      = {};

   vm.candidate.skills     = vm.candidate.skills || [];
   vm.candidate.tags       = vm.candidate.tags || [];
   vm.candidate.files      = vm.candidate.files || [];

   vm.saveCandidate        = saveCandidate;
   vm.clearUploaderQueue   = clearUploaderQueue;

   (function _init() {
      _initThesauruses();
      _createUploader();
   }());

   function clearUploaderQueue() {
      vm.uploader.clearQueue();
      $element[0].querySelector('#upload').value = null;
   }
   function _onError() {
      UserDialogService.notification('Some error was occurred!', 'error');
   }

   function _initThesauruses() {
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then((data) => vm.thesaurus = data);
   }

   function _createUploader() {
      vm.uploader = FileService.getFileUploader({maxSize: 1024000});
      vm.uploader.onSuccessItem = (item, response, status, headers) => {
         LoggerService.log('onSuccessItem', item, response, status, headers);
      };
      vm.uploader.onErrorItem = (fileItem, response, status, headers) => {
         LoggerService.error('onErrorItem', fileItem, response, status, headers);
      };
      vm.uploader.onWhenAddingFileFailed = () => {
         UserDialogService.notification($translate.instant('COMMON.FILE_UPLOADER_ERROR_MESSAGE'), 'error');
      };
   }

   function saveCandidate(form) {
      if (ValidationService.validate(form)) {
         CandidateService.saveCandidate(vm.candidate)
            .then(entity => set(vm, 'candidate', entity))
            .catch(_onError);
      }
   }
}
