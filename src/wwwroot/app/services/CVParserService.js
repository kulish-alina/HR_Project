const CV_PARSER_URL = 'CVParser/';
let _HttpService;
let moment = require('moment');

const OUTPUT_DATE_FORMAT = 'DD-MM-YYYY';
const INPUT_DATE_FORMATS = [
   'DD-MM-YYYY',
   'DD.MM.YYYY',
   'DD/MM/YYYY'
];

export default class CVParserService {
   constructor(HttpService) {
      'ngInject';
      _HttpService = HttpService;
   }
   parseCandidateCV(localPath) {
      let cvPath = `${localPath}`;
      return _HttpService.post(CV_PARSER_URL, { path: cvPath}).then(candidate => {
         candidate.birthDate = _convertBirthDateToFrontendFormat(candidate.birthDate);
         return candidate;
      });
   }
}

function _convertBirthDateToFrontendFormat(birthDateSource) {
   let parsedBirthDate = moment(birthDateSource, INPUT_DATE_FORMATS);
   return parsedBirthDate.isValid() ? parsedBirthDate.format(OUTPUT_DATE_FORMAT) : undefined;
}
