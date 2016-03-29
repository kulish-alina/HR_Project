const VACANCY_URL = 'vacancies/';
let _HttpService;

export default class VacancyService {
   constructor(HttpService) {
      'ngInject';
      _HttpService = HttpService;
   }

   getVacancies() {
      return _HttpService.get(VACANCY_URL);
   }

   getVacancy(vacancyId) {
      var additionalUrl = VACANCY_URL + vacancyId;
      return _HttpService.get(additionalUrl);
   }

   saveVacancy(entity) {
      if (entity.Id !== undefined) {
         var additionalUrl = VACANCY_URL + entity.Id;
         return _HttpService.put(additionalUrl, entity);
      } else {
         return _HttpService.post(VACANCY_URL, entity);
      }
   }

   deleteVacancy(entity) {
      var additionalUrl = VACANCY_URL + entity.Id;
      _HttpService.remove(additionalUrl, entity);
   }
}
