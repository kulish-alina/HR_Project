import { has, find, concat, remove, filter, map, forEach, some } from 'lodash';

import THESAURUS_STRUCTURES from './ThesaurusStructuresStore.js';

let _HttpService, _$q;

const cache = {};

export default class ThesaurusService {
   constructor(HttpService, $q) {
      'ngInject';
      _HttpService = HttpService;
      _$q = $q;
   }

   getThesaurusTopics(thesaurusName) {
      if (has(cache, thesaurusName)) {
         return _$q.when(cache[thesaurusName]);
      } else {
         let thesaurusesToLoad = _getLoadedThesaurusesList(thesaurusName);
         return _$q.all(map(thesaurusesToLoad, thesaurus => _HttpService.get(thesaurus)))
                .then(thesauruses => {
                   for (var i = 0; i < thesauruses.length; i++) {
                      cache[thesaurusesToLoad[i]] = thesauruses[i];
                      _actionOfAdditionFieldsForTopics(cache[thesaurusesToLoad[i]],
                                                       thesaurusesToLoad[i],
                                                       _addRefTextFieldFunction);
                   }
                   return cache[thesaurusName];
                });
      }
   }

   getThesaurusTopic(thesaurusName, id) {
      if (has(cache, thesaurusName)) {
         return _$q.when(find(cache[thesaurusName], {id: id}));
      } else {
         var url = thesaurusName + '/' + id;
         return this.getThesaurusTopics(thesaurusName)
            .then(() => find(cache[thesaurusName], {id: id}));
      }
   }

   saveThesaurusTopic(thesaurusName, entity) {
      if (has(this.getThesaurusNames(), thesaurusName)) {
         _actionOfAdditionFieldsForTopic(entity, thesaurusName, _deleteRefTextFieldFunction);
         if (entity.id !== undefined) {
            let additionalUrl = thesaurusName + '/' + entity.id;
            return _HttpService.put(additionalUrl, entity)
               .then(_entity => {
                  _actionOfAdditionFieldsForTopic(_entity, thesaurusName,
                                                  _addRefTextFieldFunction);
                  return _entity;
               });
         } else {
            return _HttpService.post(thesaurusName + '/', entity)
               .then(_entity => {
                  _actionOfAdditionFieldsForTopic(_entity, thesaurusName,
                                                  _addRefTextFieldFunction);
                  cache[thesaurusName].push(_entity);
               });
         }
      } else {
         return _$q.reject('Thesaurus name is incorrect.');
      }
   }

   deleteThesaurusTopic(thesaurusName, entity) {
      if (has(this.getThesaurusNames(), thesaurusName)) {
         let additionalUrl = thesaurusName + '/' + entity.id;
         _actionOfAdditionFieldsForTopic(entity, thesaurusName, _deleteRefTextFieldFunction);
         return  _HttpService.remove(additionalUrl, entity)
                     .then(() => remove(cache[thesaurusName], entity));
      } else {
         return _$q.reject('Thesaurus name is incorrect.');
      }
   }

   getThesaurusNames() {
      return Object.keys(THESAURUS_STRUCTURES);
   }

   getThesaurusStructure(thesaurusName) {
      return THESAURUS_STRUCTURES[thesaurusName];
   }
}

function _getReferenceFields(thesaurusName) {
   let structure = THESAURUS_STRUCTURES[thesaurusName];
   if (structure !== undefined) {
      return filter(structure.fields, {type: 'select'});
   } else {
      return [];
   }
}

function _getLoadedThesaurusesList(mainThesaurusName) {
   var list = [];
   list.push(mainThesaurusName);
   forEach(map(_getReferenceFields(mainThesaurusName), field => field.refTo),
           thesaurus => list.push(thesaurus));
   return list;
}

function _addRefTextFieldFunction(field, topic) {
   if (some(cache[field.refTo], {id: topic[field.name]})) {
      let referencedTopic = find(cache[field.refTo], {id: topic[field.name]});
      topic[field.additionFieldForText] = referencedTopic[field.labelRefFieldName];
   }
}

function _deleteRefTextFieldFunction(field, topic) {
   delete topic[field.additionFieldForText];
}

function _actionOfAdditionFieldsForTopic(topic, thesaurusName, action) {
   var additionFields = _getReferenceFields(thesaurusName);
   forEach(_getReferenceFields(thesaurusName), field => action(field, topic));
}

function _actionOfAdditionFieldsForTopics(topics, thesaurusName, action) {
   forEach(topics, topic => _actionOfAdditionFieldsForTopic(topic, thesaurusName, action));
}
