import utils from '../../utils';

const MAX_SIZE_OF_FILE = 5120;
const LIST_OF_THESAURUS = ['industries', 'levels', 'locations', 'languages', 'languageLevels',
    'departments', 'typesOfEmployment', 'statuses', 'tags', 'skills'];

export default function VacancyController(
   $scope,
   VacancyService,
   ValidationService,
   FileUploader,
   ThesaurusService,
   $q
) {
   'ngInject';

   const vm = $scope;
   vm.cancel = cancel;
   vm.saveVacancy = saveVacancy;
   vm.vacancy = {};
   vm.vacancy.fileIds = [];
   vm.uploader = createNewUploader();
   vm.vacancy.requiredSkills = [];
   vm.vacancy.tags = [];

   let map = utils.array2map(LIST_OF_THESAURUS, ThesaurusService.getThesaurusTopics);
   $q.all(map).then((data) => vm.thesaurus = data);

   vm.responsibles = [
      {id: '1', title: 'vbre'},
      {id: '2', title: 'tkas'},
      {id: '3', title: 'vles'}
   ];

   function createNewUploader() {
      let newUploader = new FileUploader({
         url: './api/files',
         onCompleteAll: _vs
      });
      newUploader.filters.push({
         name: 'sizeFilter',
         fn: function sizeFilter (item) {
            if (item.size <= MAX_SIZE_OF_FILE) {
               return true;
            }
         }
      });
      newUploader.onSuccessItem = function onSuccessUpload (item) {
         vm.vacancy.fileIds.push(item.id);
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
      return VacancyService.saveVacancy(vm.vacancy);
   }
}
