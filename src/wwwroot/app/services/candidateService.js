const CANDIDATE_URL = 'candidates/';

export default class CandidateService {
   constructor(HttpService) {
      'ngInject';
      this.HttpService = HttpService;
   }

   getCandidates() {
      return this.HttpService.get(CANDIDATE_URL);
   }
   getCandidate(id) {
      var additionalUrl = CANDIDATE_URL + id;
      return this.HttpService.get(additionalUrl);
   }
   saveCandidate(entity) {
      if (entity.Id !== undefined) {
         var additionalUrl = CANDIDATE_URL + entity.Id;
         return this.HttpService.put(additionalUrl, entity);
      }
      else {
         return this.HttpService.post(CANDIDATE_URL, entity);
      }
   }
   deleteCandidate(entity) {
      var additionalUrl = CANDIDATE_URL + entity.Id;
      this.HttpService.remove(additionalUrl, entity);
   }
}
