const  CANDIDATE_URL = 'candidate/';
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
      let additionalUrl = CANDIDATE_URL + id;
      return _HttpService.get(additionalUrl);
   }

   saveCandidate(entity) {
      if (entity.Id) {
         let additionalUrl = CANDIDATE_URL + entity.Id;
         return _HttpService.put(additionalUrl, entity);
      } else {
         return _HttpService.post(CANDIDATE_URL, entity);
      }
   }

   deleteCandidate(entity) {
      let additionalUrl = CANDIDATE_URL + entity.Id;
      _HttpService.remove(additionalUrl, entity);
   }
}
