export default function VacancyController($scope, VacancyService, ValidationService, FileUploader, ThesaurusService) {
   'ngInject';

   const vm = $scope;
   vm.saveVacancy = saveVacancy;
   vm.industries = [
      {id: '1', title: 'IT'},
      {id: '2', title: 'Security'},
      {id: '3', title: 'Accounting'}
   ];
   vm.levels = [
      {id: '1', title: 'Trainee'},
      {id: '2', title: 'Junior'},
      {id: '3', title: 'Middle'},
      {id: '4', title: 'Senior'}
   ];
   vm.cities = [
      {id: '1', title: 'Dnipropetrovsk'},
      {id: '2', title: 'Zaporizhzhia'},
      {id: '3', title: 'Berdyansk'},
      {id: '4', title: 'Lviv'}
   ];
   vm.languages = [];
   ThesaurusService.getThesaurusTopics('languages')
      .then(topics => vm.languages = topics);

   vm.languageLevels = [
      {id: '1', title: 'Pre-Intermediate'},
      {id: '2', title: 'Intermediate'},
      {id: '3', title: 'Upper Intermediate'},
      {id: '4', title: 'Advanced'},
      {id: '5', title: 'Fluent'}
   ];
   vm.departments = [
      {id: '1', title: 'Accounting', departmentGroup: 'Nonprod'},
      {id: '2', title: 'Managers', departmentGroup: 'Prod'},
      {id: '3', title: 'Contract Programming', departmentGroup: 'Contract'}
   ];
   vm.responsibles = [
      {id: '1', lastName: 'vbre'},
      {id: '2', lastName: 'tkas'},
      {id: '3', lastName: 'vles'}
   ];
   vm.typesOfEmployment = [
      {id: '1', title: 'Full-time'},
      {id: '2', title: 'Part-time'},
      {id: '3', title: 'Remote'},
      {id: '4', title: 'Practice'},
      {id: '5', title: 'Courses'},
      {id: '6', title: 'Project'}
   ];
   vm.statuses = [
      {id: '1', title: 'Open'},
      {id: '2', title: 'Processing'},
      {id: '3', title: 'Closed'},
      {id: '4', title: 'Cancelled'}
   ];
   vm.skills = [
      {id: '1', title: 'SQL'},
      {id: '2', title: 'WinForms'},
      {id: '3', title: 'DevExpress'},
      {id: '4', title: '.Net'},
      {id: '5', title: 'C#'},
      {id: '6', title: 'Spring .Net'},
      {id: '7', title: 'JQuery'},
      {id: '8', title: 'JavaScript'},
      {id: '9', title: 'ASP .NET MVC'},
      {id: '10', title: 'HTML5+CSS3'}
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
