const LIST_OF_THESAURUS = ['industries', 'levels', 'locations', 'languages', 'languageLevels',
    'departments', 'typesOfEmployment', 'tags', 'skills', 'stages'];
export default function CandidateController(
   $element,
   $scope,
   $translate,
   CandidateService,
   ValidationService,
   FileUploaderService,
   ThesaurusService,
   UserDialogService
   ) {
   'ngInject';

   const vm = $scope;
   vm.saveCandidate = saveCandidate;
   vm.keys = Object.keys;
   vm.clearUploaderQueue = clearUploaderQueue;

   ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then((data) => vm.thesaurus = data);

   vm.uploader = FileUploaderService.getFileUploader({maxSize: 1024000});

   function clearUploaderQueue() {
      vm.uploader.clearQueue();
      $element[0].querySelector('#upload').value = null;
   }

   function _onError() {
      UserDialogService.notification('Some error was occurred!', 'error');
   }

   function saveCandidate(form) {
      if (ValidationService.validate(form)) {
         CandidateService.saveCandidate(vm.candidate).catch(_onError);
      }
   }
}
