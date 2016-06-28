const LIST_OF_THESAURUS = ['stage', 'industry'];
import {
   remove,
   set,
   each,
   cloneDeep,
   find
} from 'lodash';

export default function VacancyProfileController(
   $scope,
   $q,
   $state,
   $translate,
   $element,
   $timeout,
   ThesaurusService,
   UserService,
   VacancyService,
   UserDialogService,
   FileService,
   LoggerService
   ) {
   'ngInject';

   const vm                = $scope;
   vm.vacancy              = {};
   vm.vacancy.comments     = $state.params._data ? $state.params._data.comments : vm.vacancy.comments;
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
   vm.saveComment          = _saveComment;
   vm.removeComment        = _removeComment;
   vm.editComment          = _editComment;
   vm.comments             = cloneDeep(vm.vacancy.comments);
   vm.vacancy              = {
      files : $state.params._data ? $state.params._data.files : []
   };
   vm.vacancy.comments     = [];
   let initializing        = true;

   function _initCurrentVacancy() {
      if ($state.params._data) {
         vm.vacancy = $state.params._data;
      } else {
         VacancyService.getVacancy($state.params.vacancyId).then(vacancy => {
            set(vm, 'vacancy', vacancy);
            vm.comments = cloneDeep(vm.vacancy.comments);
         });
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

   vm.$watchGroup(['vacancy.startDate', 'vacancy.endDate', 'vacancy.deadlineDate'], function watchDates() {
      if (initializing) {
         $timeout(function timeout() {
            initializing = false;
         });
      } else {
         changed();
      }
   });

   function selectStage(stageName) {
      vm.currentStage = stageName;
   }

   function _saveComment(comment) {
      vm.isChanged = true;
      return $q.when(vm.comments.push(comment));
   }

   function _removeComment(comment) {
      vm.isChanged = true;
      let commentForRemove = find(vm.comments, comment);
      if (comment.id) {
         commentForRemove.state = 1;
         remove(vm.comments, comment);
         return $q.when(vm.comments.push(commentForRemove));
      } else {
         return $q.when(remove(vm.comments, comment));
      }
   }

   function _editComment(comment) {
      vm.isChanged = true;
      return $q.when(remove(vm.comments, comment));
   }

   function _vs() {
      let memo = vm.vacancy.comments;
      vm.vacancy.comments = vm.comments;
      VacancyService.save(vm.vacancy).then(vacancy => {
         vm.vacancy = vacancy;
         vm.comments = cloneDeep(vm.vacancy.comments);
         UserDialogService.notification($translate.instant('DIALOG_SERVICE.SUCCESSFUL_SAVING'), 'success');
         vm.isChanged = false;
         vm.uploader.clearQueue();
      }).catch((error) => {
         vm.vacancy.comments = memo;
         UserDialogService.notification($translate.instant('DIALOG_SERVICE.ERROR_SAVING'), 'error');
         LoggerService.error(error);
      });
   }
}
