const CANDIDATE_URL = 'candidates/';
let _HttpService;

export default class CandidateService {
   constructor(HttpService) {
      'ngInject';
      _HttpService = HttpService;
   }

   getCandidates() {
      return _HttpService.get(CANDIDATE_URL);
   }

   getCandidate(id) {
      var additionalUrl = CANDIDATE_URL + id;
      return _HttpService.get(additionalUrl);
   }

   saveCandidate(entity) {
      if (entity.Id !== undefined) {
         var additionalUrl = CANDIDATE_URL + entity.Id;
         return _HttpService.put(additionalUrl, entity);
      }
      else {
         return _HttpService.post(CANDIDATE_URL, entity);
      }
   }

   deleteCandidate(entity) {
      var additionalUrl = CANDIDATE_URL + entity.Id;
      _HttpService.remove(additionalUrl, entity);
   }
}
