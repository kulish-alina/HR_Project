const CV_PARSER_URL = 'CVParser/';
let _HttpService;
let moment = require('moment');

export default class CVParserService {
   constructor(HttpService) {
      'ngInject';
      _HttpService = HttpService;
   }
   parseCandidateCV(localPath) {
      let cvPath = `${localPath}`;
      return _HttpService.post(CV_PARSER_URL, { path: cvPath}).then(candidate => {
         candidate.birthDate = moment(candidate.birthDate).format('DD-MM-YYYY');
         return candidate;
      });
   }
}
