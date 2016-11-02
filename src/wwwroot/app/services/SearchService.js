import {
   isEqual,
   cloneDeep
} from 'lodash';

let defaultTimeout = 60000;

let _VacancyService, _CandidateService;
let cache = {};

export default class SearchService {
   constructor(VacancyService, CandidateService) {
      _VacancyService   = VacancyService;
      _CandidateService = CandidateService;
   }

   fetchVacancies(options, updateTimeout) {
      return _search(_VacancyService, options, 'vacancies', updateTimeout);
   }

   fetchCandidates(options, updateTimeout) {
      return _search(_CandidateService, options, 'candidates', updateTimeout);
   }

   invalidateVacancies() {
      cache.vacancies = null;
   }

   invalidateCandidates() {
      cache.candidates = null;
   }
}

function _search(searchService, options, cacheKey, updateTimeout) {
   if (!cache[cacheKey] ||
       (Date.now() - cache[cacheKey].timeStamp) > (updateTimeout || defaultTimeout) ||
       !isEqual(cache[cacheKey].options, options)) {
      return saveToCache(cacheKey, searchService.search(options, true), options);
   } else {
      return cache[cacheKey].value;
   };
}

function saveToCache(key, value, options) {
   let cacheObj = {};
   cacheObj.value = value;
   cacheObj.options = cloneDeep(options);
   cacheObj.timeStamp = Date.now();
   cache[key] = cacheObj;
   return cacheObj.value;
}
