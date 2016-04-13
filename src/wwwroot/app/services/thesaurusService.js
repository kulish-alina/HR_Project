import { has, find, concat, remove } from 'lodash';

let _HttpService, _$q;

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
   constructor(HttpService, $q) {
      'ngInject';
      _HttpService = HttpService;
      _$q = $q;
   }

   getThesaurusTopics(thesaurusName) {
      if (!has(cache, thesaurusName)) {
         return _HttpService.get(thesaurusName)
            .then(topics => cache[thesaurusName] = topics);
      }
      return _$q.when(cache[thesaurusName]);
   }

   saveThesaurusTopic(thesaurusName, entity) {
      if (entity.id !== undefined) {
         var additionalUrl = thesaurusName + '/' + entity.id;
         return _HttpService.put(additionalUrl, entity);
      }
      else {
         return _HttpService.post(thesaurusName + '/', entity)
            .then(_entity => cache[thesaurusName].push(_entity));
      }
   }

   deleteThesaurusTopic(thesaurusName, entity) {
      var additionalUrl = thesaurusName + '/' + entity.id;
      return  _HttpService.remove(additionalUrl, entity)
         .then(() => remove(cache[thesaurusName], entity));
   }

   getThesaurusNames() {
      return THESAURUS_STRUCTURES.keys;
   }

   getThesaurusStructure(thesaurusName) {
      return THESAURUS_STRUCTURES[thesaurusName];
   }
}
