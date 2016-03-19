export default class VacancyService {
   constructor(HttpService) {
      'ngInject';
      this.HttpService = HttpService;
   }

   getVacancies(urlId) {
      return this.HttpService.getVacancies(urlId);
   }
}
