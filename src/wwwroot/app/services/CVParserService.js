import {
   cond,
   matches,
   forEach,
   filter,
   toArray,
   stubString
} from 'lodash';
const moment = require('moment');


const CV_PARSER_URL = 'CVParser/';
let _HttpService;

const SYMBOL_TYPE = {
   None: 0,
   Word: 1,
   Digit: 2
};

export default class CVParserService {
   constructor(HttpService) {
      'ngInject';
      _HttpService = HttpService;
   }
   parseCandidateCV(localPath) {
      let cvPath = `${localPath}`;
      return _HttpService.post(CV_PARSER_URL, { path: cvPath});
   }

   getWordsArray(line) {
      let wordsArray = [];
      let symbArray = [];
      let spaceSymbolWasMet = false;
      let lastWriteSymbolType = SYMBOL_TYPE.None;
      let lineToSymbols = toArray(line);

      let wordRegexp = /[a-z]/i;
      let spaceAndSeparatorRegexp = /[\s-_;]/;
      let numberRegexp = /[0-9+()]/;

      forEach(lineToSymbols, (symb) => {
         if (spaceAndSeparatorRegexp.test(symb)) {
            spaceSymbolWasMet = true;
         } else {
            if (wordRegexp.test(symb)) {
               if (spaceSymbolWasMet && lastWriteSymbolType === SYMBOL_TYPE.Word ||
               lastWriteSymbolType === SYMBOL_TYPE.Digit) {
                  wordsArray.push(symbArray.join(stubString()));
                  symbArray = [];
               }
               lastWriteSymbolType = SYMBOL_TYPE.Word;
            } else if (numberRegexp.test(symb)) {
               if (spaceSymbolWasMet && lastWriteSymbolType === SYMBOL_TYPE.Word) {
                  wordsArray.push(symbArray.join(stubString()));
                  symbArray = [];
               }
               lastWriteSymbolType = SYMBOL_TYPE.Digit;
            }
            spaceSymbolWasMet = false;
            symbArray.push(symb);
         }
      });
      if (symbArray.length) {
         wordsArray.push(symbArray.join(stubString()));
      }
      return wordsArray;
   };


   resolveWordClass (word, candidate) {
      let phoneNumbersProperty = 'phoneNumbers';
      let birthDateProperty = 'birthDate';
      let keyWordClass = 'key-word';
      let simpleWordClass = 'simple-word';

      let specialPropertyClassResolve = cond([
         [matches(phoneNumbersProperty),              () => {
            return !!(filter(candidate[phoneNumbersProperty], (phone) => {
               let phoneRegexp = /[(]?0[()]?\s?\d\d[)\s-]?\s?\d\d\d[\s-]?\d\d[\s-]?\d\d/;
               if (phoneRegexp.test(phone.number) && phoneRegexp.test(word)) {
                  return true;
               }
            }).length);
         }],
         [matches(birthDateProperty),                  () => {
            return moment(candidate[birthDateProperty]).isSame(moment(word));
         }]
      ]);
      for (let property in candidate) {
         if (candidate[property] === word || specialPropertyClassResolve(property)) {
            return keyWordClass;
         }
      }
      return simpleWordClass;
   };
}


