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
      const additionalUrl = VACANCY_URL + vacancyId;
      return _HttpService.get(additionalUrl);
   }

   saveVacancy(entity) {
      if (entity.id) {
         const additionalUrl = VACANCY_URL + entity.id;
         return _HttpService.put(additionalUrl, entity);
      } else {
         return _HttpService.post(VACANCY_URL, entity);
      }
   }

   deleteVacancy(entity) {
      if (entity.id === undefined) {
         throw new Error('Id should be specified');
      } else {
         const additionalUrl = VACANCY_URL + entity.id;
         _HttpService.remove(additionalUrl, entity);
      }
   }
}
