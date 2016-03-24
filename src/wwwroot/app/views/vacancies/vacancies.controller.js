export default function VacanciesController($scope, VacancyService, LoggerService) {
    'ngInject';

    var vm = $scope;
    vm.vacancies = [];

    vm.getVacancies = getVacancies;
    vm.getVacancy = getVacancy;
    vm.deleteVacancy = deleteVacancy;
    vm.editVacancy = editVacancy;

    function getVacancies() {
        VacancyService.getVacancies().then(value => vm.vacancies = value).catch(_onError);
    }

    function getVacancy(vacancyId) {
        VacancyService.getVacancy(vacancyId).then(value => vm.vacancies = [value]).catch(_onError);
    }

    function editVacancy(vacancy) {
        VacancyService.saveVacancy(vacancy).catch(_onError);
    }

    function deleteVacancy(vacancy) {
        VacancyService.deleteVacancy(vacancy).catch(_onError);
    }

    function _onError(message) {
        if (message.status === -1) {
            LoggerService.error(new Date(), 'You cannot get a response from the server');
        }
    }
}
