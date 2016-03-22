export default function VacanciesController($scope, VacancyService) {
    'ngInject';

    var vm = $scope;
	 vm.vacancies = [];

    vm.getVacancies = getVacancies;
	 vm.getVacancy = getVacancy;
	 vm.deleteVacancy = deleteVacancy;

    function getVacancies() {
        VacancyService.getVacancies().then(value => vm.vacancies = value);
    }
	
	 function getVacancy(){
		 VacancyService.getVacancy(vacancyId).then(value => vm.vacancies = [value]);
	 }
	
	 function editVacancy(vacancy) {
        VacancyService.editVacancy(vacancy);
    }
	
	function deleteVacancy(vacancy){
		VacancyService.getVacancy(vacancy);
	}
}