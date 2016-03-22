export default function VacanciesController($scope, VacancyService) {
    'ngInject';

    var vm = $scope;
	 vm.vacancies = [];

    vm.getVacancies = getVacancies;
	 vm.getVacancy = getVacancy;
	 vm.deleteVacancy = deleteVacancy;
	 vm.editVacancy = editVacancy;

    function getVacancies() {
        VacancyService.getVacancies().then(value => vm.vacancies = value);
    }
	
	 function getVacancy(vacancyId){
		 VacancyService.getVacancy(vacancyId).then(value => vm.vacancies = [value]);
	 }
	
	 function editVacancy(vacancy) {
        VacancyService.saveVacancy(vacancy);
    }
	
	function deleteVacancy(vacancy){
		VacancyService.deleteVacancy(vacancy);
	}
}