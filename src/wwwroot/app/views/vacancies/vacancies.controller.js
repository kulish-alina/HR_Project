import utils from '../../utils';
export default function VacanciesController($scope, VacancyService, ThesaurusService, $q) {
   'ngInject';

   const vm = $scope;
   vm.vacancies = [];
   vm.getVacancies = getVacancies;
   vm.getVacancy = getVacancy;
   vm.deleteVacancy = deleteVacancy;
   vm.editVacancy = editVacancy;

   let listOfThesaurus = ['industries', 'levels', 'locations', 'languages', 'languageLevels',
    'departments', 'typesOfEmployment', 'statuses', 'tags', 'skills'];

   let map = utils.array2map(listOfThesaurus, ThesaurusService.getThesaurusTopics);
   $q.all(map).then((data) => vm.thesaurus = data);

   function getVacancies() {
      VacancyService.getVacancies().then(value => vm.vacancies = value).catch(_onError);
   }

   function getVacancy(vacancyId) {
      VacancyService.getVacancy(vacancyId).then(value => vm.vacancies = [ value ]).catch(_onError);
   }

   function editVacancy(vacancy) {
      VacancyService.saveVacancy(vacancy).catch(_onError);
   }

   function deleteVacancy(vacancy) {
      VacancyService.deleteVacancy(vacancy);
   }

   function _onError() {
      vm.errorMessage = 'Sorry! Some error occurred';
   }
}
