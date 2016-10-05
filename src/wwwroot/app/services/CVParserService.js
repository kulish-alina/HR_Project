const CV_PARSER_URL = 'CVParser/';
let _HttpService;

export default class CVParserService {
   constructor(HttpService) {
      'ngInject';
      _HttpService = HttpService;
   }
   parseCandidateCV(localPath) {
      debugger;
      let cvPath = `${localPath}`;
      return _HttpService.post(CV_PARSER_URL, { path: cvPath});
   }
}
