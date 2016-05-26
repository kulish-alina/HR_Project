const LIST_OF_THESAURUS = ['industries', 'levels', 'locations', 'languages', 'languageLevels',
    'departments', 'typesOfEmployment', 'tags', 'skills', 'stages'];
export default function CandidateController(
   $element,
   $scope,
   $translate,
   CandidateService,
   ValidationService,
   FileUploader,
   ThesaurusService
   ) {
   'ngInject';

   const vm = $scope;
   vm.saveCandidate = saveCandidate;
   vm.keys = Object.keys;
   vm.clearUploaderQueue = clearUploaderQueue;

   ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then((data) => vm.thesaurus = data);

   vm.uploader = new FileUploader({
      url: './api/files'
   });

   function clearUploaderQueue() {
      vm.uploader.clearQueue();
      $element[0].querySelector('#upload').value = null;
   }

   function _onError() {
      vm.errorMessage = $translate.instant('CANDIDATE.ERROR');
   }

   function saveCandidate(form) {
      if (ValidationService.validate(form)) {
         CandidateService.saveCandidate(vm.candidate).catch(_onError);
      }
   }
}
