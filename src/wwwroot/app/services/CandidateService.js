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
   merge
} from 'lodash';
import utils  from '../utils';

const CANDIDATE_URL           = 'candidate/';
const DATE_TYPE_TO_CONVERT    = [ 'birthDate' ];
const DELETED_STATE           = 1;

let _forEach   = curryRight(forEach, 2);
let curriedSet = curry(set, 3);
let _addSavedThesaurusesTopicsCurried = curry(_addSavedThesaurusesTopics, 3);

const THESAURUSES = [
   new ThesaurusHelper('tag',   'tagIds',   'tags',     true),
   new ThesaurusHelper('skill', 'skillIds', 'skills',   true),

   new ThesaurusHelper('industry',          'industryId',        'industry'),
   new ThesaurusHelper('city',              'cityId',            'city'),
   new ThesaurusHelper('typeOfEmployment',  'typeOfEmployment',  'typeOfEmployment'),
   new ThesaurusHelper('level',             'levelId',           'level'),
   new ThesaurusHelper('currency',          'currencyId',        'currency'),
   new ThesaurusHelper('socialNetwork',     'socialNetworks',    'socials')
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
         .catch(() => _convertToClientFormat(entity));
   }

   deleteCandidate(entity) {
      return _HttpService.remove(CANDIDATE_URL + entity.id, entity);
   }

   search(condition) {
      return _HttpService.post(`${CANDIDATE_URL}search`, condition).then(response => {
         return _$q.all(map(response.candidate, _convertToClientFormat)).then((candidate) => {
            response.candidate = candidate;
            return response;
         });
      });
   }
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
            .then(_addSavedThesaurusesTopicsCurried(candidate, helper.thesaurusName));
      }
      return memo;
   }, {});
   return _$q.all(promises);
}

function  _convertThesaurusToIds(candidate) {
   forEach(THESAURUSES, helper => {
      if (helper.needConvertForServer) {
         candidate[helper.serverField] = map(candidate[helper.clientField], 'id');
      }
   });
   return candidate;
}

function _convertToClientFormat(candidate) {
   _convertFromServerDates(candidate);
   _addReferencedThesaurusObjects(candidate);
   _setMonthYearExperience(candidate);
   _convertRelocationPlacesToClient(candidate);
   _convertLanguageSkillsToClient(candidate);
   _convertSocialToClient(candidate);
   return candidate;
}

function _convertToServerFormat(candidate) {
   _convertToServerDates(candidate);
   _convertThesaurusToIds(candidate);
   _deleteReferencedThesaurusObjects(candidate);
   _setStartExperience(candidate);
   _convertRelocationPlacesToBackend(candidate);
   _convertLanguageSkillsToBackend(candidate);
   _convertSocialToBackend(candidate);
   delete candidate.experienceYears;
   delete candidate.experienceMonthes;
   return candidate;
}

function _convertSocialToClient(candidate) {
   let convertedSocials = map(candidate.socialNetworks, social => _getCandidateSocialObject(social));
   candidate.convertedSocials = convertedSocials;
   return candidate;
}

function _convertSocialToBackend(candidate) {
   let changedSocials = unionWith(
      candidate.socialNetworks || [],
      map(candidate.convertedSocials, _convertSocialFieldsToBackend),
      isEqualSocials);

   let deletedSocials = differenceWith(
      changedSocials,
      candidate.convertedSocials,
      isEqualSocials);

   forEach(deletedSocials, social => social.state = DELETED_STATE);
   candidate.socialNetworks = changedSocials;
   delete candidate.convertedSocials;
   return candidate;
}

function _convertLanguageSkillsToBackend(candidate) {
   let changedLanguageSkills = unionWith(
      candidate.languageSkills,
      map(candidate.convertedLanguageSkills, _convertLanguageSkillFieldsToBackend),
      isEqualLanguageSkills);

   let deletedLanguageSkills = differenceWith(
      changedLanguageSkills,
      candidate.convertedLanguageSkills,
      isEqualLanguageSkills);

   forEach(deletedLanguageSkills, skill => skill.state = DELETED_STATE);
   candidate.languageSkills = changedLanguageSkills;
   delete candidate.convertedLanguageSkills;
   return candidate;
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

function _getLanguageSkillObgect(sourceLanguageSkill) {
   let convertedLanguageSkill = {};
   _ThesaurusService.getThesaurusTopic('language', sourceLanguageSkill.languageId)
      .then(curriedSet(convertedLanguageSkill, 'language'));
   _ThesaurusService.getThesaurusTopic('languageLevel', sourceLanguageSkill.languageLevel)
      .then(level => {
         convertedLanguageSkill.languageLevel = isNull(level) ? undefined : level;
      });
   return convertedLanguageSkill;
}

function _getRelocationPlaceObgect(sourceRelocationPlace) {
   let convertedRelocationPlace = {};
   _ThesaurusService.getThesaurusTopic('country', sourceRelocationPlace.countryId)
      .then(curriedSet(convertedRelocationPlace, 'country'));
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

function _convertLanguageSkillsToClient(candidate) {
   let _languageSkills = map(candidate.languageSkills, _getLanguageSkillObgect);
   candidate.convertedLanguageSkills = _languageSkills;
   return candidate;
}

function _convertRelocationPlacesToBackend(candidate) {
   let changedRelocationPlaces = unionWith(
      candidate.relocationPlaces,
      map(candidate.convertedRelocationPlaces, _convertRelocationsFieldsToBackend),
      isEqualRelocationPlaces);

   let deletedRelocationPlaces = differenceWith(
      changedRelocationPlaces,
      candidate.convertedRelocationPlaces,
      isEqualRelocationPlaces);

   forEach(deletedRelocationPlaces, place => place.state = DELETED_STATE);
   candidate.relocationPlaces = changedRelocationPlaces;
   delete candidate.convertedRelocationPlaces;
   return candidate;
}

function _convertRelocationPlacesToClient(candidate) {
   let convertedRelocationPlaces = map(candidate.relocationPlaces, _getRelocationPlaceObgect);
   candidate.convertedRelocationPlaces = convertedRelocationPlaces;
   return candidate;
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

function _addReferencedThesaurusObjects(candidate) {
   forEach(THESAURUSES, ({thesaurusName, clientField, serverField, needConvertForServer}) => {
      //TODO: resolve problem with convertation ids to objects
      if (!needConvertForServer) {
         _ThesaurusService.getThesaurusTopicsByIds(thesaurusName, candidate[serverField])
            .then(curriedSet(candidate, clientField));
      }
   });
}

function _deleteReferencedThesaurusObjects(candidate) {
   forEach(THESAURUSES, refThesaurus => {
      if (refThesaurus.needConvertForServer) {
         delete candidate[refThesaurus.clientField];
      }
   });
}

function _convertFromServerDates(candidate) {
   forEach(DATE_TYPE_TO_CONVERT, type => {
      candidate[type] = utils.formatDateFromServer(candidate[type]);
   });
   return candidate;
}

function _convertToServerDates(candidate) {
   forEach(DATE_TYPE_TO_CONVERT, type => {
      if (candidate[type]) {
         candidate[type] = utils.formatDateToServer(candidate[type]);
      };
   });
   return candidate;
}

function _setStartExperience(candidate) {
   if (candidate.experienceMonthes || candidate.experienceYears) {
      let nowDate                    = new Date(now());
      candidate.startExperience      = new Date(
         nowDate.getFullYear() - candidate.experienceYears,
         nowDate.getMonth()    - candidate.experienceMonthes
      );
   }
}

function _setMonthYearExperience(candidate) {
   if (candidate.startExperience) {
      let nowDate                    = new Date(now());
      let startExperienceDate        = new Date(candidate.startExperience);
      candidate.experienceYears      = nowDate.getFullYear()   - startExperienceDate.getFullYear();
      candidate.experienceMonthes    = nowDate.getMonth()      - startExperienceDate.getMonth();
   }
}

function ThesaurusHelper(thesaurusName, serverField, clientField, needConvertForServer) {
   this.thesaurusName            = thesaurusName;
   this.serverField              = serverField;
   this.clientField              = clientField;
   this.needConvertForServer     = needConvertForServer;
}
