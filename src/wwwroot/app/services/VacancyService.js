const VACANCY_URL = 'vacancy';
let _HttpService;
let _ThesaurusService;
let _$q;

export default class VacancyService {
   constructor(HttpService, ThesaurusService, $q) {
      'ngInject';
      _HttpService = HttpService;
      _ThesaurusService = ThesaurusService;
      _$q = $q;
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

   saveNewTopicsToThesaurus (skills, tags) {
      let arrayLength = 0;
      if (skills.length === arrayLength && tags.length === arrayLength) {
         return _$q.when(true);
      }
      let promises = [
         _ThesaurusService.saveThesaurusTopics('skills', skills),
         _ThesaurusService.saveThesaurusTopics('tags', tags)
      ];
      return _$q.all(promises);
   }
}
