import utils from '../../utils';

export default function VacancyController($scope, VacancyService, ValidationService,
                                           FileUploader, ThesaurusService, $q) {
   'ngInject';

   const vm = $scope;
   vm.saveVacancy = saveVacancy;

   let listOfThesaurus = ['industries', 'levels', 'locations', 'languages', 'languageLevels',
    'departments', 'typesOfEmployment', 'statuses', 'tags', 'skills'];

   let map = utils.array2map(listOfThesaurus, ThesaurusService.getThesaurusTopics);
   $q.all(map).then((data) => vm.thesaurus = data);

   vm.responsibles = [
      {id: '1', title: 'vbre'},
      {id: '2', title: 'tkas'},
      {id: '3', title: 'vles'}
   ];
   vm.uploader = new FileUploader({
      url: '/foo/url',
      onCompleteAll: _vs
   });

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
      VacancyService.saveVacancy(vm.vacancy).catch(_onError);
   }

   function _onError() {
      vm.errorMessage = 'Sorry! Some error occurred';
   }
}
