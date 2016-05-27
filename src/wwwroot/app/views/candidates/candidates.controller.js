const LIST_OF_THESAURUS = ['industries', 'levels', 'locations', 'languages', 'languageLevels',
    'departments', 'typesOfEmployment', 'tags', 'skills', 'stages'];

export default function CandidatesController($scope, CandidateService, ThesaurusService, UserDialogService) {
   'ngInject';
   const vm = $scope;
   vm.candidates = [];
   vm.getCandidate = getCandidate;
   vm.deleteCandidate = deleteCandidate;
   vm.editCandidate = editCandidate;
   vm.getCandidates = getCandidates;
   vm.thesaurus = [];
   vm.slider = {
      min: 21,
      max: 45,
      options: {
         floor: 15,
         ceil: 65
      }
   };

   ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then((data) => vm.thesaurus = data);

   function getCandidates() {
      CandidateService.getCandidates()
        .then(value => vm.candidates = value)
        .catch(_onError);
   }

   function getCandidate(candidateId) {
      CandidateService.getCandidate(candidateId)
        .then(value => vm.candidates = [ value ])
        .catch(_onError);
   }

   function deleteCandidate(candidate) {
      CandidateService.deleteCandidate(candidate);
   }

   function editCandidate(candidate) {
      CandidateService.saveCandidate(candidate).catch(_onError);
   }

   function _onError() {
      UserDialogService.notification('Some error was occurred!', 'error');
   }
}
