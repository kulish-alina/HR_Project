import utils from '../../utils';
export default function CandidateController(
   $scope,
   $translate,
   CandidateService,
   ValidationService,
   FileUploader,
   ThesaurusService,
   $q) {
   'ngInject';

   const vm = $scope;
   vm.saveCandidate = saveCandidate;
   vm.keys = Object.keys;

   let listOfThesaurus = ['industries', 'levels', 'locations', 'languages', 'languageLevels',
    'departments', 'typesOfEmployment', 'statuses', 'tags', 'skills'];

   let map = utils.array2map(listOfThesaurus, ThesaurusService.getThesaurusTopics);
   $q.all(map).then((data) => vm.thesaurus = data);

   vm.uploader = new FileUploader({
      url: './api/files'
   });

   function _onError() {
      vm.errorMessage = $translate.instant('CANDIDATE.ERROR');
   }

   function saveCandidate(form) {
      if (ValidationService.validate(form)) {
         CandidateService.saveCandidate(vm.candidate).catch(_onError);
      }
   }
}
