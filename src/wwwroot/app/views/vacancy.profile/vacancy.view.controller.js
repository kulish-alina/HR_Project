const LIST_OF_THESAURUS = ['stage', 'industry'];
import {
   remove,
   set,
   each
} from 'lodash';

export default function VacancyProfileController(
   $scope,
   $state,
   $translate,
   $element,
   ThesaurusService,
   UserService,
   VacancyService,
   UserDialogService,
   FileService,
   LoggerService
   ) {
   'ngInject';

   const vm                = $scope;
   vm.thesaurus            = [];
   vm.responsibles         = [];
   vm.edit                 = edit;
   vm.uploader             = createNewUploader();
   vm.addFilesForRemove    = addFilesForRemove;
   vm.queueFilesForRemove  = [];
   vm.saveChanges          = saveChanges;
   vm.changed              = changed;
   vm.isChanged            = false;
   vm.selectStage          = selectStage;
   vm.currentStage         = '';

   vm.vacancy              = {
      files : $state.params._data ? $state.params._data.files : []
   };
   vm.vacancy.comments     = [];

   function _initCurrentVacancy() {
      if ($state.params._data) {
         vm.vacancy = $state.params._data;
      } else {
         VacancyService.getVacancy($state.params.vacancyId).then(vacancy => set(vm, 'vacancy', vacancy));
      }
   }
   _initCurrentVacancy();

   UserService.getUsers().then(users => set(vm, 'responsibles', users));

   ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then(topics => set(vm, 'thesaurus', topics));

   function createNewUploader() {
      let newUploader = FileService.getFileUploader({ onCompleteAllCallBack : saveChanges, maxSize : 2048000 });
      newUploader.onSuccessItem = function onSuccessUpload(item) {
         let response = JSON.parse(item._xhr.response);
         vm.vacancy.files.push(response);
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

   function addFilesForRemove(file) {
      vm.queueFilesForRemove.push(file);
      remove(vm.vacancy.files, {id: file.id});
      vm.isChanged = true;
   }

   function edit() {
      $state.go('vacancyEdit', {_data: vm.vacancy, vacancyId: vm.vacancy.id});
   }

   function saveChanges() {
      if (vm.uploader.getNotUploadedItems().length) {
         vm.uploader.uploadAll();
      } else if (vm.queueFilesForRemove) {
         each(vm.queueFilesForRemove, (file) => FileService.remove(file));
         vm.queueFilesForRemove = [];
         _vs();
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
         UserDialogService.notification($translate.instant('DIALOG_SERVICE.SUCCESSFUL_SAVING'), 'success');
         vm.isChanged = false;
         vm.uploader.clearQueue();
      }).catch((error) => {
         UserDialogService.notification($translate.instant('DIALOG_SERVICE.ERROR_SAVING'), 'error');
         LoggerService.error(error);
      });
   }
}
