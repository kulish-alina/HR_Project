const LIST_OF_THESAURUS = ['stages', 'industries'];
import {
   find,
   remove
} from 'lodash';

export default function VacancyProfileController(
   $scope,
   $state,
   $translate,
   ThesaurusService,
   UserService,
   FileUploaderService,
   VacancyService,
   UserDialogService
   ) {
   'ngInject';

   const vm = $scope;
   vm.thesaurus = [];
   vm.responsibles = [];
   vm.edit = edit;
   vm.vacancy =  $state.params._data || {} ;
   vm.vacancy.files = $state.params._data.files || [];
   vm.uploader = createNewUploader();
   vm.removeFile = removeFile;
   vm.saveChanges = saveChanges;
   vm.changed = changed;
   vm.isChanged = false;
   vm.selectStage = selectStage;
   vm.currentStage = '';

   UserService.getUsers().then((users) => {
      vm.responsibles = users;
   });
   console.log('vacancy in view', vm.vacancy);

   ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then((data) => {
      vm.thesaurus = data;
      console.log('vm.thesaurus1', vm.thesaurus);
   });
   console.log('vm.thesaurus2', vm.thesaurus);

   function createNewUploader() {
      let newUploader = FileUploaderService.getFileUploader({ onCompleteAllCallBack : saveChanges, maxSize : 2048000 });
      newUploader.onSuccessItem = function onSuccessUpload(item) {
         let response = JSON.parse(item._xhr.response);
         vm.vacancy.files.push(response);
      };
      newUploader.onWhenAddingFileFailed = function onAddingFileFailed() {
         vm.errorMessageFromFileUploader = $translate.instant('COMMON.FILE_UPLOADER_ERROR_MESSAGE');
      };
      return newUploader;
   }
   function removeFile(file) {
      let currentFileId = JSON.parse(file._xhr.response).id;
      let removedFile = find(vm.vacancy.files, {id: currentFileId});
      removedFile.state = 1;
      remove(vm.vacancy.files, {id: currentFileId});
      vm.vacancy.files.push(removedFile);
      file.remove();
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
      }).catch(() => {
         UserDialogService.notification($translate.instant('DIALOG_SERVICE.ERROR_SAVING'), 'error');
      });
   }

//   function _onError() {
//      vm.errorMessage = 'Sorry! Some error occurred';
//   }
}
