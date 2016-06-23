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
   find,
   concat,
   unionWith,
   differenceWith
} from 'lodash';
import utils  from '../utils.js';

const CANDIDATE_URL           = 'candidate/';
const DATE_TYPE_TO_CONVERT    = [ 'birthDate' ];
const deletedState            = 1;

let _forEach   = curryRight(forEach, 2);
let curriedSet = curry(set, 3);
let _addSavedThesaurusesTopicsCurried = curry(_addSavedThesaurusesTopics, 3);

const THESAURUSES = [
   new ThesaurusHelper('tag',   'tagIds',   'tags',     true),
   new ThesaurusHelper('skill', 'skillIds', 'skills',   true),

   new ThesaurusHelper('industry',          'industryId',        'industry'),
   new ThesaurusHelper('city',              'cityId',            'city'),
   new ThesaurusHelper('typeOfEmployment',  'typeOfEmployment',  'typeOfEmployment'),
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
         .catch(() => {
            _convertToClientFormat(entity);
         });
   }

   deleteCandidate(entity) {
      _HttpService.remove(CANDIDATE_URL + entity.id, entity);
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
         candidate[helper.serverField] = map(candidate[helper.clientField], topic => topic.id);
      }
   });
   return candidate;
}

function _convertToClientFormat(candidate) {
   _convertFromServerDates(candidate);
   _addReferencedThesaurusObjects(candidate);
   _setMonthYearExperience(candidate);
   _partitionOfRelocationCities(candidate);
   _convertLanguageSkillsToClient(candidate);
   return candidate;
}

function _convertToServerFormat(candidate) {
   _convertToServerDates(candidate);
   _convertThesaurusToIds(candidate);
   _deleteReferencedThesaurusObjects(candidate);
   _setStartExperience(candidate);
   _groupRelocationPlaces(candidate);
   _convertLanguageSkillsToBackend(candidate);
   delete candidate.experienceYears;
   delete candidate.experienceMonthes;
   return candidate;
}

function _convertLanguageSkillsToBackend(candidate) {
   let changedLanguageSkills = unionWith(
      candidate.languageSkills,
      candidate.convertedLanguageSkills,
      isEquelLanguageSkills);

   let deletedLanguageSkills = differenceWith(
      changedLanguageSkills,
      candidate.convertedLanguageSkills,
      isEquelLanguageSkills);

   forEach(deletedLanguageSkills, skill => skill.state = deletedState);
   candidate.languageSkills = changedLanguageSkills;
   delete candidate.convertedLanguageSkills;
   return candidate;
}

function isEquelLanguageSkills(sourceLanguageSkill, changedLanguageSkill) {
   return sourceLanguageSkill.languageId === changedLanguageSkill.languageId &&
      sourceLanguageSkill.languageLevel === changedLanguageSkill.languageLevel;
}

function _convertLanguageSkillsToClient(candidate) {
   let _languageSkills = map(candidate.languageSkills, languageSkill => {
      return {
         languageId     : languageSkill.languageId,
         languageLevel  : languageSkill.languageLevel
      };
   });
   candidate.convertedLanguageSkills = _languageSkills;
   return candidate;
}

function _groupRelocationPlaces(candidate) {
   let relocationPlaces = reduce(candidate.convertedRelocationPlaces, (result, place) => {
      let countryPlace = find(result, _ => _.countryId === place.countryId);
      if (countryPlace) {
         countryPlace.cityIds = concat(countryPlace.cityIds, place.shortForm.cityIds);
      } else {
         let fullPlace        = place.id ? (find(candidate.relocationPlaces, {id: place.id}) || {}) : {};
         fullPlace.countryId  = place.shortForm.countryId;
         fullPlace.cityIds    = concat([], place.shortForm.cityIds);
         result.push(fullPlace);
      }
      return result;
   }, []);
   candidate.relocationPlaces = relocationPlaces;
   delete candidate.convertedRelocationPlaces;
   return candidate;
}

function _partitionOfRelocationCities(candidate) {
   let _relocationPlaces = [];
   forEach(candidate.relocationPlaces, place => forEach(place.cityIds, cityId =>
      _relocationPlaces.push({ countryId : place.countryId, cityIds : cityId })));
   set(candidate, 'convertedRelocationPlaces', _relocationPlaces);
   return candidate;
}

function _addReferencedThesaurusObjects(candidate) {
   forEach(THESAURUSES, ({thesaurusName, clientField, serverField, needConvertForServer}) => {
      if (needConvertForServer) {
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
