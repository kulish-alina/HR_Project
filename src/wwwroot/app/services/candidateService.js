const CANDIDATE_URL = 'candidates/';

export default class CandidateService {
   constructor(HttpService) {
      'ngInject';
      this.HttpService = HttpService;
   }

    getCandidates() {
		  var additionalUrl = CANDIDATE_URL;
        return this.HttpService.get(additionalUrl);
   }
	
	 getCandidate(id) {
		  var additionalUrl = CANDIDATE_URL + id;
        return this.HttpService.get(additionalUrl);
    }
	
	 saveCandidate(entity){
		 if(entity.Id !== undefined){
			 var additionalUrl = CANDIDATE_URL + entity.Id;
			 return this.HttpService.put(additionalUrl, entity);
		 }
		 else{
			 var additionalUrl =  CANDIDATE_URL;
			 return this.HttpService.post(additionalUrl, entity);
		 }
	 }

	 deleteCandidate(entity) {
		 var additionalUrl = CANDIDATE_URL + entity.Id;
		 this.HttpService.remove(additionalUrl, entity);
   }
}
