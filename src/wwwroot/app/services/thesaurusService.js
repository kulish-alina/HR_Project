import {
   has,
   find,
   remove,
   filter,
   map,
   forEach,
   includes,
   curry
} from 'lodash';

import utils from '../utils.js';

const curryLength = 3;
const activeStateId = 1;

import THESAURUS_STRUCTURES from './ThesaurusStructuresStore.js';

let _HttpService, _$q, _$translate;
let _curryAction = curry(_actionOfAdditionFieldsForTopic, curryLength);

const cache = {};

export default class ThesaurusService {
   constructor(HttpService, $q, $translate) {
      'ngInject';
      _HttpService = HttpService;
      _$q = $q;
      _$translate = $translate;
   }

   getThesaurusTopics(thesaurusName) {
      if (has(cache, thesaurusName)) {
         return _$q.when(cache[thesaurusName]);
      } else {
         let thesaurusesToLoad = _getLoadedThesaurusesList(thesaurusName);
         let mapThesaurusPromises = utils.array2map(thesaurusesToLoad, name => _HttpService.get(name));
         return _$q.all(mapThesaurusPromises).then(thesauruses => {
            forEach(thesauruses, (thesaurus, name) => {
               cache[name] = filter(thesaurus, {state : activeStateId});
               _actionOfAdditionFieldsForTopics(thesaurus, name, _addRefTextFieldFunction);
            });
            return cache[thesaurusName];
         });
      }
   }

   getThesaurusTopic(thesaurusName, id) {
      if (has(cache, thesaurusName)) {
         return _$q.when(find(cache[thesaurusName], {id}));
      } else {
         return this.getThesaurusTopics(thesaurusName).then(() => find(cache[thesaurusName], {id}));
      }
   }

   getThesaurusTopicsGroup(thesaurusNames) {
      let mapThesaurusPromises = utils.array2map(thesaurusNames, this.getThesaurusTopics);
      return _$q.all(mapThesaurusPromises);
   }

   saveThesaurusTopic(thesaurusName, entity) {
      if (includes(this.getThesaurusNames(), thesaurusName)) {
         let _action = _curryAction(thesaurusName);
         _action(_deleteRefTextFieldFunction, entity);

         let promise;

         if (entity.id) {
            promise = _HttpService.put(`${thesaurusName}/${entity.id}`, entity);
         } else {
            promise = _HttpService.post(`${thesaurusName}/`, entity);
            promise = promise.then(_entity => {
               debugger;
               cache[thesaurusName].push(_entity);
               return _entity;
            });
         }

         return promise.then(_action(_addRefTextFieldFunction));
      } else {
         return _$q.reject(_$translate.instant('ERRORS.thesaurusErrors.incorrectNameMsg'));
      }
   }

   saveThesaurusTopics(thesaurusName, entities) {
      let mapThesaurusPromises = utils.array2map(entities, entity => this.saveThesaurusTopic(thesaurusName, entity));
      return _$q.all(mapThesaurusPromises);
   }

   deleteThesaurusTopic(thesaurusName, entity) {
      if (includes(this.getThesaurusNames(), thesaurusName)) {
         let additionalUrl = `${thesaurusName}/${entity.id}`;
         _actionOfAdditionFieldsForTopic(entity, thesaurusName, _deleteRefTextFieldFunction);
         return _HttpService.remove(additionalUrl, entity).then(() => remove(cache[thesaurusName], entity));
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
   let referencedTopic = find(cache[field.refTo], {id: topic[field.name]});
   if (referencedTopic) {
      topic[field.refObject] = referencedTopic;
   }
}

function _deleteRefTextFieldFunction(field, topic) {
   delete topic[field.refObject];
}

function _actionOfAdditionFieldsForTopic(thesaurusName, action, entity) {
   let additionFields = _getReferenceFields(thesaurusName);
   forEach(additionFields, field => action(field, entity));
}

function _actionOfAdditionFieldsForTopics(topics, thesaurusName, action) {
   forEach(topics, _curryAction(thesaurusName, action));
}
