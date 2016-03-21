export default class VacancyService {
    constructor(HttpService) {
        'ngInject';
        this.HttpService = HttpService;
    }

    getVacancies(additionalUrl) {
       return this.HttpService.getVacancies(additionalUrl);
    }
}