export default class CandidateService {
   constructor(HttpService) {
      'ngInject';
      this.HttpService = HttpService;
   }

   getCandidates(urlId) {
      return this.HttpService.getEntity(urlId);
   }

   addCandidate(urlId, entity) {
      this.HttpService.addEntity(urlId, entity);
   }
}
