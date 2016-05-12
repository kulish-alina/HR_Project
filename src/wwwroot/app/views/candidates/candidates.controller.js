import utils from '../../utils';
export default function CandidatesController($scope, CandidateService, ThesaurusService, $q) {
   'ngInject';
   const vm = $scope;
   vm.candidates = [];
   vm.getCandidate = getCandidate;
   vm.deleteCandidate = deleteCandidate;
   vm.editCandidate = editCandidate;
   vm.getCandidates = getCandidates;

   let listOfThesaurus = ['industries', 'levels', 'locations', 'languages', 'languageLevels',
    'departments', 'typesOfEmployment', 'statuses', 'tags', 'skills'];

   let map = utils.array2map(listOfThesaurus, ThesaurusService.getThesaurusTopics);
   $q.all(map).then((data) => vm.thesaurus = data);

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
      vm.errorMessage = 'Sorry! Some error occurred';
   }
}
