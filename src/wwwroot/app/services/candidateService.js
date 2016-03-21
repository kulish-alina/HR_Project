const CANDIDATE_URL = 'Candidates/Get/';

export default class CandidateService {
    constructor(HttpService) {
        'ngInject';
        this.HttpService = HttpService;
    }

    getCandidates() {
		  var additionalUrl = CANDIDATE_URL;
        return this.HttpService.getEntities(additionalUrl);
    }
	
	 getCandidate(id) {
		  var additionalUrl = CANDIDATE_URL + id;
        return this.HttpService.getEntity(additionalUrl);
    }

    addCandidate(entity) {
		  var additionalUrl =  CANDIDATE_URL;
        return this.HttpService.addEntity(additionalUrl, entity);
    }
	
    editCandidate(entity) {
		 var additionalUrl = CANDIDATE_URL + entity.id;
		 return this.HttpService.editEntity(additionalUrl, entity);
    }
	
	 deleteCandidate(entity) {
		 var additionalUrl = CANDIDATE_URL + entity.id;
		 return this.HttpService.deleteEntity(additionalUrl, entity);
    }
}