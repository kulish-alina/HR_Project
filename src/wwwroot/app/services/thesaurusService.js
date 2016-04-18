import { has, find, concat, remove, filter, map, forEach } from 'lodash';

let _HttpService, _$q;

const cache = { };

const THESAURUS_STRUCTURES = {
   'countries' :  {
      thesaurusName : 'THESAURUSES.COUNTRIES',
      fields : [ {name : 'title', label : 'name', type : 'text'} ]
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
            refTo : 'countries', labelRefFieldName : 'title' , additionFieldForText : 'countryName'}
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
         let thesaurusesToLoad = _getLoadedThesaurusesList(thesaurusName);
         return _$q.all(map(thesaurusesToLoad, thesaurus => _HttpService.get(thesaurus)))
                .then(thesauruses => {
                   for (var i = 0; i < thesauruses.length; i++) {
                      cache[thesaurusesToLoad[i]] = thesauruses[i];
                      _actionOfAdditionFieldsForTopics(cache[thesaurusesToLoad[i]],
                                                       thesaurusesToLoad[i],
                                                       cache,
                                                       _addRefTextFieldFunction);
                   }
                   return cache[thesaurusName];
                });
      }
      return _$q.when(cache[thesaurusName]);
   }

   getThesaurusTopic(thesaurusName, id) {
      if (!has(cache, thesaurusName)) {
         var url = thesaurusName + '/' + id;
         return this.getThesaurusTopics(thesaurusName)
            .then(() => {
               return find(cache[thesaurusName], s => s.id === id);
            });
      }
      return _$q.when(find(cache[thesaurusName], s => s.id === id));
   }

   saveThesaurusTopic(thesaurusName, entity) {
      _actionOfAdditionFieldsForTopic(entity, thesaurusName, cache, _deleteRefTextFieldFunction);
      if (entity.id !== undefined) {
         var additionalUrl = thesaurusName + '/' + entity.id;
         return _HttpService.put(additionalUrl, entity)
            .then(_entity => {
               _actionOfAdditionFieldsForTopic(_entity, thesaurusName,
                  cache, _addRefTextFieldFunction);
               return _entity;
            });
      }
      else {
         return _HttpService.post(thesaurusName + '/', entity)
            .then(_entity => {
               _actionOfAdditionFieldsForTopic(_entity, thesaurusName,
                  cache, _addRefTextFieldFunction);
               cache[thesaurusName].push(_entity);
            });
      }
   }

   deleteThesaurusTopic(thesaurusName, entity) {
      var additionalUrl = thesaurusName + '/' + entity.id;
      _actionOfAdditionFieldsForTopic(entity, thesaurusName, cache, _deleteRefTextFieldFunction);
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

function _getReferenceFields(thesaurusName) {
   return filter(THESAURUS_STRUCTURES[thesaurusName].fields, field => has(field, 'refTo'));
}

function _getLoadedThesaurusesList(mainThesaurusName) {
   var list = [];
   list.push(mainThesaurusName);
   forEach(map(_getReferenceFields(mainThesaurusName), field => field.refTo),
           thesaurus => list.push(thesaurus));
   return list;
}

function _addRefTextFieldFunction(field, topic, _cache) {
   var additThesaurus = find(_cache[field.refTo], s => s.id === topic[field.name]);
   if (additThesaurus !== undefined) {
      topic[field.additionFieldForText] =
         find(_cache[field.refTo], s => s.id === topic[field.name])[field.labelRefFieldName];
   }
}

function _deleteRefTextFieldFunction(field, topic, _cache) {
   delete topic[field.additionFieldForText];
}

function _actionOfAdditionFieldsForTopic(topic, thesaurusName, _cache, action) {
   var additionFields = _getReferenceFields(thesaurusName);
   forEach(_getReferenceFields(thesaurusName), field => action(field, topic, _cache));
}

function _actionOfAdditionFieldsForTopics(topics, thesaurusName, _cache, action) {
   forEach(topics, topic => _actionOfAdditionFieldsForTopic(topic, thesaurusName, _cache, action));
}
