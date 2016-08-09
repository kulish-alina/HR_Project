import {
   forEach,
   now,
   filter,
   map,
   reduce,
   remove,
   union,
   curry,
   curryRight,
   set,
   unionWith,
   differenceWith,
   isNull,
   get,
   merge,
   head
} from 'lodash';
import utils  from '../utils';

const CANDIDATE_URL           = 'candidate/';
const DATE_TYPE_TO_CONVERT    = ['birthDate', 'createdOn'];
const DELETED_STATE           = 1;
const ZERO_YEAR               = 1970;


let _set     = curry(set, 3);
let _forEach = curryRight(forEach, 2);
let convertThesaurusToIds              = curry(_convertThesaurusToIds, 2);
let _addSavedThesaurusesTopicsCurried  = curry(_addSavedThesaurusesTopics, 3);
let addReferencedThesaurusObjects      = curry(_addReferencedThesaurusObjects, 2);

const THESAURUSES = [
   new ThesaurusHelper('tag',   'tagIds',   'tags',     true,  true),
   new ThesaurusHelper('skill', 'skillIds', 'skills',   true,  true),

   new ThesaurusHelper('industry',          'industryId',        'industry',     false,    false),
   new ThesaurusHelper('city',              'cityId',            'city',         false,    false),
   new ThesaurusHelper('level',             'levelId',           'level',        false,    false),
   new ThesaurusHelper('currency',          'currencyId',        'currency',     false,    false),
   new ThesaurusHelper('socialNetwork',     'socialNetworks',    'socials',      false,    false),
   new ThesaurusHelper('source',            'mainSourceId',      'mainSource',   false,    false),
   new ThesaurusHelper('typeOfEmployment',  'typeOfEmployment',  'typeOfEmploymentObject', false,  false)
];

const CONVERTORS_TO_SERVER = [
   _convertToServerDates,
   _convertThesaurusesToIds,
   _setStartExperience,
   _convertRelocationPlacesToBackend,
   _convertLanguageSkillsToBackend,
   _convertSocialToBackend,
   _deleteReferencedThesaurusObjects
];

const CONVERTORS_TO_CLIENT = [
   _convertFromServerDates,
   _addReferencedThesaurusesObjects,
   _setMonthYearExperience,
   _convertRelocationPlacesToClient,
   _convertLanguageSkillsToClient,
   _convertSocialToClient
];

let _HttpService, _ThesaurusService, _$q;

export default class CandidateService {
   constructor(HttpService, ThesaurusService, $q) {
      'ngInject';
      _HttpService = HttpService;
      _ThesaurusService = ThesaurusService;
      _$q = $q;
   }

   getCandidates() {
      return _HttpService.get(CANDIDATE_URL).then(_forEach(_convertToClientFormat));
   }

   getCandidate(id) {
      return _HttpService.get(CANDIDATE_URL + id).then(_convertToClientFormat);
   }

   saveCandidate(entity) {
      return _saveNewThesaurusTopics(entity)
         .then(() => _convertToServerFormat(entity))
         .then(candidate => {
            if (candidate.id) {
               return _HttpService.put(CANDIDATE_URL + candidate.id, candidate);
            } else {
               return _HttpService.post(CANDIDATE_URL, candidate);
            }
         })
         .then(_convertToClientFormat)
         .catch(error => {
            _convertToClientFormat(entity);
            return _$q.reject(error);
         });
   }

   deleteCandidate(entity) {
      return _HttpService.remove(CANDIDATE_URL + entity.id, entity);
   }

   search(condition) {
      _convertLanguageSkillsToBackend(condition);
      remove(condition.languageSkills, {state : DELETED_STATE});
      return _HttpService.post(`${CANDIDATE_URL}search`, condition)
         .then(response => {
            return _$q.all(map(response.candidate, _convertToClientFormat))
               .then(candidates => {
                  _convertLanguageSkillsToClient(condition);
                  return  candidates;
               })
               .then(_set(response, 'candidate'))
               .then(response);
         });
   }
}

function _setDeletedState(item) {
   item.state = DELETED_STATE;
}

function _addSavedThesaurusesTopics(candidate, thesaurusName, topics) {
   candidate[thesaurusName] = union(candidate[thesaurusName] || [],  topics);
   return candidate;
}

function _saveNewThesaurusTopics(candidate) {
   let promises = reduce(THESAURUSES, (memo, helper) => {
      let newTopicPattern = {id : undefined};
      let topicsToSave = filter(candidate[helper.clientField], newTopicPattern);
      if (topicsToSave.length) {
         remove(candidate[helper.clientField], newTopicPattern);
         memo[helper.clientField] = _ThesaurusService.saveThesaurusTopics(helper.thesaurusName, topicsToSave)
            .then(_addSavedThesaurusesTopicsCurried(candidate, helper.clientField));
      }
      return memo;
   }, {});
   return _$q.all(promises);
}


function _convertToClientFormat(candidate) {
   return _$q.all(map(CONVERTORS_TO_CLIENT, convertor => convertor(candidate))).then(() => candidate);
}

function _convertToServerFormat(candidate) {
   return _$q.all(map(CONVERTORS_TO_SERVER, convertor => convertor(candidate))).then(() => candidate);
}

function  _convertThesaurusesToIds(candidate) {
   return _$q.all(map(THESAURUSES, convertThesaurusToIds(candidate))).then(() => candidate);
}

function _convertThesaurusToIds(candidate, helper) {
   if (helper.needConvertForServer) {
      candidate[helper.serverField] = map(candidate[helper.clientField], 'id');
   }
   return _$q.when(candidate);
}

function _addReferencedThesaurusesObjects(candidate) {
   return _$q.all(map(THESAURUSES, addReferencedThesaurusObjects(candidate))).then(() => candidate);
}

function _addReferencedThesaurusObjects(candidate, helper) {
   return _ThesaurusService.getThesaurusTopicsByIds(helper.thesaurusName, candidate[helper.serverField])
      .then(topics => {
         return helper.isArray ? topics : head(topics);
      })
      .then(_set(candidate, helper.clientField));
}

function _deleteReferencedThesaurusObjects(candidate) {
   return _$q.all(map(THESAURUSES, refThesaurus => {
      delete candidate[refThesaurus.clientField];
   })).then(candidate);
}

function _convertArrayFieldToClient(candidate, serverFieldName, convertedFieldName, itemConverter) {
   return _$q.all(map(candidate[serverFieldName], itemConverter))
      .then(_set(candidate, convertedFieldName))
      .then(() => candidate);
}

function _convertArrayFieldToServer(candidate, serverFieldName, convertedFieldName, equalFunction, itemConverter) {
   let changedItems = unionWith(
      candidate[serverFieldName] || [],
      map(candidate[convertedFieldName], itemConverter),
      equalFunction);

   let deletedItems = differenceWith(
      changedItems,
      map(candidate[convertedFieldName], itemConverter),
      equalFunction);

   return _$q.all(map(deletedItems, _setDeletedState))
      .then(() => set(candidate, serverFieldName, changedItems))
      .then(() => delete candidate[convertedFieldName])
      .then(candidate);
}

function _convertSocialToClient(candidate) {
   return  _convertArrayFieldToClient(candidate, 'socialNetworks', 'convertedSocials', _getCandidateSocialObject);
}

function _convertSocialToBackend(candidate) {
   return _convertArrayFieldToServer(
      candidate,
      'socialNetworks',
      'convertedSocials',
      isEqualSocials,
      _convertSocialFieldsToBackend
   );
}

function _convertLanguageSkillsToClient(candidate) {
   return  _convertArrayFieldToClient(candidate, 'languageSkills', 'convertedLanguageSkills',_getLanguageSkillObgect);
}

function _convertLanguageSkillsToBackend(candidate) {
   return _convertArrayFieldToServer(
      candidate,
      'languageSkills',
      'convertedLanguageSkills',
      isEqualLanguageSkills,
      _convertLanguageSkillFieldsToBackend
   );
}

function _convertRelocationPlacesToClient(candidate) {
   return  _convertArrayFieldToClient(
      candidate,
      'relocationPlaces',
      'convertedRelocationPlaces',
      _getRelocationPlaceObgect
   );
}

function _convertRelocationPlacesToBackend(candidate) {
   return _convertArrayFieldToServer(
      candidate,
      'relocationPlaces',
      'convertedRelocationPlaces',
      isEqualRelocationPlaces,
      _convertRelocationsFieldsToBackend
   );
}

function _convertFromServerDates(candidate) {
   return _$q.all(map(DATE_TYPE_TO_CONVERT, type => {
      candidate[type] = utils.formatDateFromServer(candidate[type]);
   })).then(candidate);
}

function _convertToServerDates(candidate) {
   return _$q.all(map(DATE_TYPE_TO_CONVERT, type => {
      if (candidate[type]) {
         candidate[type] = utils.formatDateToServer(candidate[type]);
      };
   }))
   .then(deleteMonthYearExperience);
}

function _setStartExperience(candidate) {
   if (candidate.experienceMonths || candidate.experienceYears) {
      let nowDate                    = new Date(now());
      candidate.startExperience      = new Date(
         nowDate.getFullYear() - candidate.experienceYears,
         nowDate.getMonth()    - candidate.experienceMonths
      );
   }
   return _$q.when(candidate);
}

function _setMonthYearExperience(candidate) {
   if (candidate.startExperience) {
      let nowDate                    = new Date(now());
      let startExperienceDate        = new Date(candidate.startExperience);
      let differenceDate             = new Date(nowDate - startExperienceDate);
      candidate.experienceYears      = differenceDate.getFullYear() - ZERO_YEAR;
      candidate.experienceMonths     = differenceDate.getMonth();
   }
   return _$q.when(candidate);
}

function deleteMonthYearExperience(candidate) {
   delete candidate.experienceYears;
   delete candidate.experienceMonths;
   return _$q.when(candidate);
}

function isEqualLanguageSkills(languageSkill1, languageSkill2) {
   return languageSkill1.languageId === languageSkill2.languageId &&
      languageSkill1.languageLevel === languageSkill2.languageLevel;
}

function isEqualRelocationPlaces(relocationPlace1, relocationPlace2) {
   return relocationPlace1.countryId === relocationPlace2.countryId &&
      relocationPlace1.cityId === relocationPlace2.cityId;
}

function isEqualSocials(social1, social2) {
   return social1.socialNetworkId === social2.socialNetworkId;
}


function _convertSocialFieldsToBackend(social) {
   return {
      socialNetworkId  : social.socialNetwork.id,
      path             : social.path
   };
}

function _convertLanguageSkillFieldsToBackend(skill) {
   return {
      languageId     : skill.language.id,
      languageLevel  : get(skill, 'languageLevel.id')
   };
}

function _convertRelocationsFieldsToBackend(place) {
   return {
      countryId   : place.country.id,
      cityId      : get(place, 'city.id')
   };
}

function _getLanguageSkillObgect(sourceLanguageSkill) {
   let convertedLanguageSkill = {};
   _ThesaurusService.getThesaurusTopic('language', sourceLanguageSkill.languageId)
      .then(_set(convertedLanguageSkill, 'language'));
   _ThesaurusService.getThesaurusTopic('languageLevel', sourceLanguageSkill.languageLevel)
      .then(level => {
         convertedLanguageSkill.languageLevel = isNull(level) || level === undefined ? {} : level;
      });
   return convertedLanguageSkill;
}

function _getRelocationPlaceObgect(sourceRelocationPlace) {
   let convertedRelocationPlace = {};
   _ThesaurusService.getThesaurusTopic('country', sourceRelocationPlace.countryId)
      .then(_set(convertedRelocationPlace, 'country'));
   _ThesaurusService.getThesaurusTopic('city', sourceRelocationPlace.cityId)
      .then(city => {
         convertedRelocationPlace.city = isNull(city) ? undefined : city;
      });
   return convertedRelocationPlace;
}

function _getCandidateSocialObject(socialSource) {
   let convertedSocial = {path: socialSource.path};
   _ThesaurusService.getThesaurusTopic('socialNetwork', socialSource.socialNetworkId)
      .then(social => {
         convertedSocial.socialNetwork = merge({}, socialSource.socialNetwork, social);
      });
   return convertedSocial;
}

function ThesaurusHelper(thesaurusName, serverField, clientField, needConvertForServer, isArray) {
   this.thesaurusName            = thesaurusName;
   this.serverField              = serverField;
   this.clientField              = clientField;
   this.needConvertForServer     = needConvertForServer;
   this.isArray                  = isArray;
}
