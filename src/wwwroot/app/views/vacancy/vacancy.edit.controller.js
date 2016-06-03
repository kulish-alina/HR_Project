const LIST_OF_THESAURUS = ['industries', 'levels', 'locations', 'languages',
    'departments', 'tags', 'skills', 'typesOfEmployment', 'languageLevels'];
import {
   remove,
   set,
   each
} from 'lodash';

export default function VacancyController(
   $scope,
   $translate,
   $state,
   VacancyService,
   ValidationService,
   ThesaurusService,
   UserService,
   UserDialogService,
   FileService,
   LoggerService
) {
   'ngInject';

   const vm = $scope;

   /* --- api --- */
   vm.clear                        = clear;
   vm.saveVacancy                  = saveVacancy;
   vm.vacancy                      = {} ;
   vm.vacancy.files                = $state.params._data ? $state.params._data.files : [];
   vm.thesaurus                    = [];
   vm.responsibles                 = [];
   vm.uploader                     = createNewUploader();
   vm.vacancy.requiredSkills       = vm.vacancy.requiredSkills || [];
   vm.vacancy.tags                 = vm.vacancy.tags || [];
   vm.addFilesForRemove            = addFilesForRemove;
   vm.queueFilesForRemove          = [];
   vm.errorMessageFromFileUploader = '';
   vm.isFilesUploaded              = false;
   /* === impl === */

   function _initCurrentVacancy() {
      if ($state.params._data) {
         vm.vacancy = $state.params._data;
      } else if ($state.params.vacancyId) {
         VacancyService.getVacancy($state.params.vacancyId).then(vacancy => set(vm, 'vacancy', vacancy));
      } else {
         vm.vacancy = {};
      }
   }

   _initCurrentVacancy();

   ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then(topics => set(vm, 'thesaurus', topics));

   UserService.getUsers().then(users => set(vm, 'responsibles', users));

   function createNewUploader() {
      let newUploader = FileService.getFileUploader({ onCompleteAllCallBack : _vs, maxSize : 2048000 });
      newUploader.onSuccessItem = function onSuccessUpload(item) {
         let response = JSON.parse(item._xhr.response);
         vm.vacancy.files.push(response);
         vm.isFilesUploaded = true;
      };
      newUploader.onWhenAddingFileFailed = function onAddingFileFailed() {
         vm.errorMessageFromFileUploader = $translate.instant('COMMON.FILE_UPLOADER_ERROR_MESSAGE');
      };
      return newUploader;
   }

   function addFilesForRemove(file) {
      vm.queueFilesForRemove.push(file);
      remove(vm.vacancy.files, {id: file.id});
      vm.isChanged = true;
   }

   function clear() {
      $state.go('vacancyEdit', {_data: null, vacancyId: null});
   }

   function saveVacancy(ev, form) {
      ev.preventDefault();
      if (ValidationService.validate(form)) {
         if (vm.uploader.getNotUploadedItems().length) {
            vm.uploader.uploadAll();
         } else if (vm.queueFilesForRemove) {
            each(vm.queueFilesForRemove, (file) => _removeFile(file));
            vm.queueFilesForRemove = [];
            _vs();
         } else {
            _vs();
         }
      }
      return false;
   }

   function _removeFile(file) {
      FileService.remove(file);
   }

   function _vs() {
      VacancyService.save(vm.vacancy).then(vacancy => {
         vm.vacancy = vacancy;
         UserDialogService.notification($translate.instant('DIALOG_SERVICE.SUCCESSFUL_SAVING'), 'success');
      }).catch((error) => {
         UserDialogService.notification($translate.instant('DIALOG_SERVICE.ERROR_SAVING'), 'error');
         LoggerService.error(error);
      });
   }
}
