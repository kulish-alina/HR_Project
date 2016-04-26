import utils from '../../utils';
import {
   filter,
   remove,
   each
} from 'lodash';

export default function VacancyController($scope, VacancyService, ValidationService,
                                           FileUploader, ThesaurusService, $q) {
   'ngInject';

   const vm = $scope;
   vm.saveVacancy = saveVacancy;
   vm.vacancy = {};
   vm.vacancy.File = {};
   vm.vacancy.File.Ids = [];

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
      url: './api/files',
      onCompleteAll: _vs
   });

   let maxSizeOfFile = 5120;

   vm.uploader.filters.push({
      name: 'sizeFilter',
      fn: function sizeFilter (item) {
         if (item.size <= maxSizeOfFile) {
            return true;
         }
      }
   });

   vm.uploader.onSuccessItem = function onSuccessUpload (item) {
      vm.File.Ids.push(item.id);
      console.log(item.id);
   };

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
      _saveThesaurusItems().then(() => {
         return VacancyService.saveVacancy(vm.vacancy);
      }).catch(_onError);
   }

   function _saveThesaurusItems() {
      let thesaurusSlills = filter(vm.vacancy.Skills, {id: null});
      let thesaurusTags = filter(vm.vacancy.Tags, {id: null});
      let arrayLength = 0;

      if (thesaurusSlills.length === arrayLength && thesaurusTags.length === arrayLength) {
         return $q.when(true);
      }
      let promises = [
         ThesaurusService.saveThesaurusTopics('skills', thesaurusSlills).then((newSkills) => {
            remove(vm.vacancy.Skills, {id: null});
            each(newSkills, skill => vm.vacancy.Skills.push(skill));
         }),
         ThesaurusService.saveThesaurusTopics('tags', thesaurusTags).then((newTags) => {
            remove(vm.vacancy.Tags, {id: null});
            each(newTags, tag => vm.vacancy.Tags.push(tag));
         })
      ];
      return $q.all(promises);
   }

   function _onError() {
      vm.errorMessage = 'Sorry! Some error occurred';
   }
}
