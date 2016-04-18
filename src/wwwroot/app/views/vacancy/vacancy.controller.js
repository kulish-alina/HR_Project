export default function VacancyController($scope, VacancyService, ValidationService) {
   'ngInject';

   var vm = $scope;
   vm.saveVacancy = saveVacancy;
   vm.industries = [
      {id: '1', name: 'IT'},
      {id: '2', name: 'Security'},
      {id: '3', name: 'Accounting'}
   ];
   vm.levels = [
      {id: '1', name: 'Trainee'},
      {id: '2', name: 'Junior'},
      {id: '3', name: 'Middle'},
      {id: '4', name: 'Senior'}
   ];
   vm.cities = [
      {id: '1', name: 'Dnipropetrovsk'},
      {id: '2', name: 'Zaporizhzhia'},
      {id: '3', name: 'Berdyansk'},
      {id: '4', name: 'Lviv'}
   ];
   vm.languages = [
      {id: '1', name: 'English'}
   ];
   vm.languageLevels = [
      {id: '1', name: 'Pre-Intermediate'},
      {id: '2', name: 'Intermediate'},
      {id: '3', name: 'Upper Intermediate'},
      {id: '4', name: 'Advanced'},
      {id: '5', name: 'Fluent'}
   ];
   vm.departments = [
      {id: '1', name: 'Accounting', departmentGroup: 'Nonprod'},
      {id: '2', name: 'Managers', departmentGroup: 'Prod'},
      {id: '3', name: 'Contract Programming', departmentGroup: 'Contract'}
   ];
   vm.responsibles = [
      {id: '1', lastName: 'vbre'},
      {id: '2', lastName: 'tkas'},
      {id: '3', lastName: 'vles'}
   ];
   vm.typesOfEmployment = [
      {id: '1', name: 'Full-time'},
      {id: '2', name: 'Part-time'},
      {id: '3', name: 'Remote'},
      {id: '4', name: 'Practice'},
      {id: '5', name: 'Courses'},
      {id: '6', name: 'Project'}
   ];
   vm.statuses = [
      {id: '1', name: 'Open'},
      {id: '2', name: 'Processing'},
      {id: '3', name: 'Closed'},
      {id: '4', name: 'Cancelled'}
   ];

   function saveVacancy(form) {
      if (ValidationService.validate(form)) {
         VacancyService.saveVacancy(vm.vacancy).catch(_onError);
      }
   }

   function _onError(message) {
      vm.errorMessage = 'Sorry! Some error occurred';
   }
}
