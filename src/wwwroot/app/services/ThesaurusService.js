import {
   filter,
   map,
   forEach,
   includes,
   curry,
   isArray
} from 'lodash';

import { find } from 'lodash/fp';

import utils from '../utils.js';

const curryLength = 3;

import THESAURUS_STRUCTURES from './ThesaurusStructuresStore.js';

let _HttpService, _$q, _$translate, _HttpCacheService;
let _curryAction = curry(_actionOfAdditionFieldsForTopic, curryLength);

export default class ThesaurusService {
   constructor(HttpService, $q, $translate, HttpCacheService) {
      'ngInject';
      _HttpService = HttpService;
      _$q = $q;
      _$translate = $translate;
      _HttpCacheService = HttpCacheService;
   }

   getThesaurusTopics(thesaurusName) {
      let thesaurusesToLoad = _getLoadedThesaurusesList(thesaurusName);
      let mapThesaurusPromises = utils.array2map(thesaurusesToLoad, _HttpCacheService.get);
      return _$q.all(mapThesaurusPromises).then(thesauruses => {
         forEach(thesauruses, (thesaurus, name) => {
            _actionOfAdditionFieldsForTopics(thesaurus, name, _addRefTextFieldFunction);
         });
         return thesauruses[thesaurusName];
      });
   }

   getThesaurusTopic(thesaurusName, id) {
      return this.getThesaurusTopics(thesaurusName).then(find({id}));
   }

   getThesaurusTopicsByIds(thesaurusName, arrIds) {
      let isArrayIds = isArray(arrIds);
      return this.getThesaurusTopics(thesaurusName).then((topics) => {
         return filter(topics, (topic) => {
            if (isArrayIds) {
               return includes(arrIds, topic.id);
            } else {
               return arrIds === topic.id;
            }
         });
      });
   }

   getThesaurusTopicsGroup(thesaurusNames) {
      let mapThesaurusPromises = utils.array2map(thesaurusNames, this.getThesaurusTopics);
      return _$q.all(mapThesaurusPromises);
   }

   saveThesaurusTopic(thesaurusName, entity) {
      if (includes(this.getThesaurusNames(), thesaurusName)) {
         let orderField = find({type : 'number'}, THESAURUS_STRUCTURES[thesaurusName].fields);
         if (orderField && !entity.id) {
            return _saveThesaurusTopic(thesaurusName, entity)
               .then(savedEntity => {
                  return this.getThesaurusTopics(thesaurusName)
                     .then(topics => filter(topics, topic =>
                        topic[orderField.name] >= savedEntity[orderField.name] && topic.id !== savedEntity.id))
                     .then(topics => _incrementOrder(topics, orderField))
                     .then(editedTopics => this.saveThesaurusTopics(thesaurusName, editedTopics)
                        .then(() => savedEntity));
               });
         } else {
            return _saveThesaurusTopic(thesaurusName, entity);
         }
      } else {
         return _$q.reject(_$translate.instant('ERRORS.thesaurusErrors.incorrectNameMsg'));
      }
   }

   saveThesaurusTopics(thesaurusName, entities) {
      let mapThesaurusPromises = map(entities, entity => _saveThesaurusTopic(thesaurusName, entity));
      return _$q.all(mapThesaurusPromises);
   }

   deleteThesaurusTopic(thesaurusName, entity) {
      if (includes(this.getThesaurusNames(), thesaurusName)) {
         let additionalUrl = `${thesaurusName}/${entity.id}`;
         _actionOfAdditionFieldsForTopic(entity, thesaurusName, _deleteRefTextFieldFunction);
         return _HttpService.remove(additionalUrl, entity)
            .then(() => {
               _HttpCacheService.clearCache(thesaurusName);
               let orderField = find({type : 'number'}, THESAURUS_STRUCTURES[thesaurusName].fields);
               if (orderField) {
                  this.getThesaurusTopics(thesaurusName)
                     .then(topics => filter(topics, topic => topic[orderField.name] >= entity[orderField.name]))
                     .then(topics => _decrementOrder(topics, orderField))
                     .then(editedTopics => {
                        this.saveThesaurusTopics(thesaurusName, editedTopics)
                           .then(topics => find(topics, topic => topic[orderField.name] === entity[orderField.name]));
                     });
               }
            });
      } else {
         return _$q.reject(_$translate.instant('ERRORS.thesaurusErrors.incorrectNameMsg'));
      }
   }

   getThesaurusNames() {
      return Object.keys(THESAURUS_STRUCTURES);
   }

   getThesaurusStructure(thesaurusName) {
      return THESAURUS_STRUCTURES[thesaurusName];
   }
}

function _saveThesaurusTopic(thesaurusName, entity) {
   let _action = _curryAction(thesaurusName);
   _action(_deleteRefTextFieldFunction, entity);

   let promise;

   if (entity.id) {
      promise = _HttpService.put(`${thesaurusName}/${entity.id}`, entity);
   } else {
      promise = _HttpService.post(`${thesaurusName}/`, entity);
      promise = promise.then(_entity => {
         _HttpCacheService.clearCache(thesaurusName);
         return _entity;
      });
   }
   return promise.then((_entity) => {
      _action(_addRefTextFieldFunction, _entity);
      return _entity;
   });
}

function _getReferenceFields(thesaurusName) {
   let structure = THESAURUS_STRUCTURES[thesaurusName];
   if (structure) {
      return filter(structure.fields, {type: 'select'});
   } else {
      return [];
   }
}

function _getLoadedThesaurusesList(mainThesaurusName) {
   let array = map(_getReferenceFields(mainThesaurusName), 'refTo');
   array.unshift(mainThesaurusName);
   return array;
}

function _addRefTextFieldFunction(field, topic) {
   _HttpCacheService.get(field.refTo)
      .then(find({id: topic[field.name]}))
      .then(referencedTopic => {
         if (referencedTopic) {
            topic[field.refObject] = referencedTopic;
         }
      });
}

function _deleteRefTextFieldFunction(field, topic) {
   delete topic[field.refObject];
}

function _actionOfAdditionFieldsForTopic(thesaurusName, action, entity) {
   let additionFields = _getReferenceFields(thesaurusName);
   forEach(additionFields, field => action(field, entity));
   return entity;
}

function _actionOfAdditionFieldsForTopics(topics, thesaurusName, action) {
   forEach(topics, _curryAction(thesaurusName, action));
}

function _incrementOrder(topics, orderField) {
   forEach(topics, topic => topic[orderField.name]++);
   return topics;
}

function _decrementOrder(topics, orderField) {
   forEach(topics, topic => topic[orderField.name]--);
   return topics;
}
