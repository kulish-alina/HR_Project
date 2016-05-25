const LIST_OF_THESAURUS = ['industries', 'levels', 'locations', 'languages',
    'departments', 'tags', 'skills', 'typesOfEmployment', 'languageLevels'];
import {
   find,
   remove
} from 'lodash';

export default function VacancyController(
   $scope,
   $translate,
   $state,
   $element,
   VacancyService,
   ValidationService,
   FileUploaderService,
   ThesaurusService,
   UserService
) {
   'ngInject';

   const vm = $scope;

   /* --- api --- */
   vm.cancel = cancel;
   vm.saveVacancy = saveVacancy;
   vm.vacancy =  $state.params._data || {} ;
   vm.vacancy.files = [];
   vm.thesaurus = [];
   vm.responsibles = [];
   vm.uploader = createNewUploader();
   vm.vacancy.requiredSkills = vm.vacancy.requiredSkills || [];
   vm.vacancy.tags = vm.vacancy.tags || [];
   vm.removeFile = removeFile;
   vm.errorMessageFromFileUploader = '';
   /* === impl === */
   ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then((data) => vm.thesaurus = data);

   UserService.getUsers().then(users => vm.responsibles = users);

   function createNewUploader() {
      let newUploader = FileUploaderService.getFileUploader({ onCompleteAllCallBack : _vs, maxSize : 2048000 });
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

   function cancel() {
      $state.params._data = null;
      $state.reload();
   }

   function saveVacancy(ev, form) {
      ev.preventDefault();
      if (ValidationService.validate(form)) {
         if (vm.uploader.getNotUploadedItems().length) {
            vm.uploader.uploadAll();
         } else {
            _vs();
         }
      }
      return false;
   }

   function _vs() {
      VacancyService.save(vm.vacancy).then(vacancy => {
         vm.vacancy = vacancy;
         vm.vacancy.files = vacancy.files;
      });
   }
}
