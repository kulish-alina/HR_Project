const LIST_OF_THESAURUS = [ 'stages' ];

export default function VacancyProfileController(
   $scope,
   $state,
   ThesaurusService,
   UserService
   ) {
   'ngInject';

   const vm = $scope;
   vm.thesaurus = [];
   vm.responsibles = [];
   vm.vacancy =  $state.params._data || {} ;

   UserService.getUsers().then((users) => {
      vm.responsibles = users;
   });
   console.log('vacancy in view', vm.vacancy);

   ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then((data) => vm.thesaurus = data);
   console.log('vm.thesaurus', vm.thesaurus);

//   function _onError() {
//      vm.errorMessage = 'Sorry! Some error occurred';
//   }
}
