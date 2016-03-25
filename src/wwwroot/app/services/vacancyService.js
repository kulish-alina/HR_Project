const VACANCY_URL = 'vacancies/';

export default class VacancyService {
   constructor(HttpService) {
      'ngInject';
      this.HttpService = HttpService;
   }

    getVacancies() {
        var additionalUrl = VACANCY_URL;
        return this.HttpService.get(additionalUrl);
    }

    getVacancy(vacancyId) {
        var additionalUrl = VACANCY_URL + vacancyId;
        return this.HttpService.get(additionalUrl);
    }

    saveVacancy(entity) {
        if (entity.Id !== undefined) {
            var additionalUrl = VACANCY_URL + entity.Id;
            return this.HttpService.put(additionalUrl, entity);
        } else {
            var additionalUrl = VACANCY_URL;
            return this.HttpService.post(additionalUrl, entity);
        }
    }

    deleteVacancy(entity) {
        var additionalUrl = VACANCY_URL + entity.Id;
        this.HttpService.remove(additionalUrl, entity);
   }
}
