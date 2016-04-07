import { hasIn, find, concat, remove } from 'lodash';

let _HttpService;

const cache = { };

const THESAURUS_STRUCTURES = {
   'countries' :  {
      thesaurusName : 'THESAURUSES.COUNTRIES',
      fields : [ {name : 'title', outputName : 'title' } ]
   },
   'socials' : {
      thesaurusName : 'THESAURUSES.SOCIALS',
      fields : [
         {name : 'title', outputName : 'title' },
         {name : 'url', outputName : 'url' }
      ]
   },
   'languages' : {
      thesaurusName : 'THESAURUSES.LANGUAGES',
      fields : [ {name : 'title', outputName : 'title' } ]
   }
};

export default class ThesaurusService {
   constructor(HttpService) {
      'ngInject';
      _HttpService = HttpService;
   }

   getThesaurusTopics(thesaurusName) {
      if (hasIn(thesaurusName, cache)) {
         return cache[thesaurusName];
      }
      else {
         cache[thesaurusName] = _HttpService.get(thesaurusName + '/');
         return cache[thesaurusName];
      }
   }

   getThesaurusTopic(thesaurusName, id) {
      if (thesaurusName in cache) {
         return cache[thesaurusName];
      }
   }

   saveThesaurusTopic(thesaurusName, entity) {
      if (entity.Id !== undefined) {
         var additionalUrl = thesaurusName + '/' + entity.Id;
         return _HttpService.put(additionalUrl, entity);
      }
      else {
         return _HttpService.post(thesaurusName + '/', entity);
      }
   }

   deleteThesaurusTopic(thesaurusName, entity) {
      var additionalUrl = thesaurusName + '/' + entity.Id;
      _HttpService.remove(additionalUrl, entity);
   }

   getThesaurusNames() {
      return THESAURUS_STRUCTURES.keys;
   }

   getThesaurusStructure(thesaurusName) {
      return THESAURUS_STRUCTURES[thesaurusName];
   }
}
