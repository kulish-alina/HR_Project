const MAX_SIZE_OF_FILE = 5120;
const LIST_OF_THESAURUS = ['industries', 'levels', 'locations', 'languages',
    'departments', 'tags', 'skills', 'typesOfEmployment', 'languageLevels'];

export default function VacancyController(
   $scope,
   $translate,
   $state,
   VacancyService,
   ValidationService,
   FileUploader,
   ThesaurusService,
   UserService
) {
   'ngInject';

   const vm = $scope;

   /* --- api --- */
   vm.cancel = cancel;
   vm.saveVacancy = saveVacancy;
   vm.vacancy =  $state.params._data || {} ;
   vm.vacancy.fileIds = [];
   vm.thesaurus = [];
   vm.responsibles = [];
   vm.uploader = createNewUploader();
   vm.vacancy.requiredSkills = vm.vacancy.requiredSkills || [];
   vm.vacancy.tags = vm.vacancy.tags || [];
   vm.errorMessageFromFileUploader = '';
   /* === impl === */
   ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then((data) => vm.thesaurus = data);

   UserService.getUsers().then(users => vm.responsibles = users);

   function createNewUploader() {
      let newUploader = new FileUploader({
         url: 'http://localhost:53031//api/files',
         onCompleteAll: _vs
      });
      newUploader.filters.push({
         name: 'sizeFilter',
         fn: function sizeFilter(item) {
            if (item.size <= MAX_SIZE_OF_FILE) {
               return true;
            }
         }
      });
      newUploader.onSuccessItem = function onSuccessUpload(item) {
         vm.vacancy.fileIds.push(item.id);
      };
      newUploader.onWhenAddingFileFailed = function onAddingFileFailed() {
         vm.errorMessageFromFileUploader = $translate.instant('COMMON.FILE_UPLOADER_ERROR_MESSAGE');
      };
      return newUploader;
   }

   function cancel(form) {
      ValidationService.reset(form);
      vm.vacancy = {};
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
      });
   }
}
