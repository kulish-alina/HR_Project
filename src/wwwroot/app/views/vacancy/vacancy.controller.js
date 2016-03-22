export default function VacancyController($scope, VacancyService) {
    'ngInject';

    var vm = $scope;
    vm.vacancy = {};
	 vm.addVacancy = addVacancy;

    function addVacancy(title, status, location) {
        vm.vacancy.vacancyStatus = {};
        vm.vacancy.location = {};
		  vm.vacancy.name = title;
        vm.vacancy.vacancyStatus.title = status;
		  vm.vacancy.location.city = location;
        VacancyService.saveVacancy(vm.vacancy);
    }
}