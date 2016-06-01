const LIST_OF_THESAURUS = ['stages', 'industries'];
import {
   remove
} from 'lodash';

export default function VacancyProfileController(
   $scope,
   $state,
   $translate,
   $element,
   ThesaurusService,
   UserService,
   FileUploaderService,
   VacancyService,
   UserDialogService,
   HttpService
   ) {
   'ngInject';

   const vm = $scope;
   vm.thesaurus = [];
   vm.responsibles = [];
   vm.edit = edit;
   vm.vacancy =  $state.params._data || {} ;
   vm.vacancy.files = $state.params._data ? $state.params._data.files : [];
   vm.uploader = createNewUploader();
   vm.removeFile = removeFile;
   vm.saveChanges = saveChanges;
   vm.changed = changed;
   vm.isChanged = false;
   vm.selectStage = selectStage;
   vm.currentStage = '';
   vm.isFilesUploaded = false;

   UserService.getUsers().then((users) => {
      vm.responsibles = users;
   });
   console.log('vacancy in view', vm.vacancy);

   ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then((data) => vm.thesaurus = data);

   function createNewUploader() {
      let newUploader = FileUploaderService.getFileUploader({ onCompleteAllCallBack : saveChanges, maxSize : 2048000 });
      newUploader.onSuccessItem = function onSuccessUpload(item) {
         let response = JSON.parse(item._xhr.response);
         vm.vacancy.files.push(response);
         $element[0].querySelector('.file-upload').value = null;
         vm.isFilesUploaded = true;
         vm.isChanged = false;
      };
      newUploader.onWhenAddingFileFailed = function onAddingFileFailed() {
         vm.errorMessageFromFileUploader = $translate.instant('COMMON.FILE_UPLOADER_ERROR_MESSAGE');
      };
      newUploader.onAfterAddingAll = function onAfterAddingAl() {
         vm.isChanged = true;
      };
      return newUploader;
   }
   function removeFile(file) {
      let url = `files/${file.id}`;
      HttpService.remove(url, file).then(() => {
         remove(vm.vacancy.files, {id: file.id});
         vm.isChanged = true;
      });
   }

   function edit() {
      $state.go('vacancy', {_data: vm.vacancy});
   }

   function saveChanges() {
      if (vm.uploader.getNotUploadedItems().length) {
         vm.uploader.uploadAll();
      } else {
         _vs();
      }
   }

   function changed() {
      vm.isChanged = true;
   }

   function selectStage(stageName) {
      vm.currentStage = stageName;
   }

   function _vs() {
      VacancyService.save(vm.vacancy).then(vacancy => {
         vm.vacancy = vacancy;
         vm.vacancy.files = vacancy.files;
         UserDialogService.notification($translate.instant('DIALOG_SERVICE.SUCCESSFUL_SAVING'), 'success');
         vm.isChanged = false;
      }).catch(() => {
         UserDialogService.notification($translate.instant('DIALOG_SERVICE.ERROR_SAVING'), 'error');
      });
   }
}
