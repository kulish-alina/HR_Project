import { has, find, concat, remove, filter, map, forEach } from 'lodash';

let _HttpService, _$q;

const cache = { };

const THESAURUS_STRUCTURES = {
   'countries' :  {
      thesaurusName : 'THESAURUSES.COUNTRIES',
      fields : [ {name : 'title', label : 'name', type : 'text' } ]
   },
   'socials' : {
      thesaurusName : 'THESAURUSES.SOCIALS',
      fields : [
         {name : 'title', label : 'name', type : 'text' },
         {name : 'img', label : 'img', type : 'img' }
      ]
   },
   'languages' : {
      thesaurusName : 'THESAURUSES.LANGUAGES',
      fields : [ {name : 'title', label : 'title', type : 'text' } ]
   },
   'locations' : {
      thesaurusName : 'THESAURUSES.LOCATION',
      fields : [
         {name : 'title', label : 'name', type : 'text' },
         {name : 'country', label : 'country', type : 'select',
            refTo : {thesaurusName: 'countries', labelFieldName : 'title'} }
      ]
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
            .then(topics => {
               cache[thesaurusName] = topics;
               var refs = map(filter(THESAURUS_STRUCTURES[thesaurusName].fields,
                              field => has(field, 'refTo')),
                          field => field.refTo.thesaurusName);
               forEach(refs, function(ref) {
                  if (!has(cache, ref)) {
                     return _HttpService.get(ref)
                        .then(_topics => cache[ref] = _topics);
                  }
               });
               /*
               var refs = THESAURUS_STRUCTURES[thesaurusName].fields
                  .filter(field => has(field, 'refTo'))
                  .map(field => field.refTo)
                  .forEach(thesaurus => getThesaurusTopics(thesaurus));

               forEach(refs, function(ref) { return this.getThesaurusTopics(ref); });
                  */
               return topics;
            });
      }
      return _$q.when(cache[thesaurusName]);
   }

   getThesaurusTopic(thesaurusName, id) {
      if (!has(cache, thesaurusName)) {
         var url = thesaurusName + '/' + id;
         return this.getThesaurusTopics(thesaurusName)
            .then(topics => {
               return find(topics, s => s.id === id);
            });
      }
      return _$q.when(find(cache[thesaurusName], s => s.id === id));
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
