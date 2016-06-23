const LIST_OF_THESAURUS = [ 'stage' ];
import {
   set
} from 'lodash';

export default function CandidateProfileController(
   $scope,
   $translate,
   FileService,
   UserDialogService,
   ThesaurusService
   ) {
   'ngInject';

   const vm        = $scope;
   vm.uploader     = createNewUploader();
   vm.saveChanges  = saveChanges;
   vm.isChanged    = false;

   function _init() {
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then(topics => set(vm, 'thesaurus', topics));
   }
   _init();

   function createNewUploader() {
      let newUploader = FileService.getFileUploader({ onCompleteAllCallBack : saveChanges, maxSize : 2048000 });
      newUploader.onSuccessItem = function onSuccessUpload(item) {
         let response = JSON.parse(item._xhr.response);
         vm.candidate.files.push(response);
         vm.isChanged = false;
      };
      newUploader.onWhenAddingFileFailed = function onAddingFileFailed() {
         UserDialogService.configs($translate.instant('COMMON.FILE_UPLOADER_ERROR_MESSAGE'), 'warning');
      };
      newUploader.onAfterAddingAll = function onAfterAddingAl() {
         vm.isChanged = true;
      };
      return newUploader;
   }

   function saveChanges() {

   }
}
