import {
   assign,
   set,
   forEach,
   remove,
   chunk,
   isEmpty,
   cloneDeep,
   clone,
   find,
   curry
 } from 'lodash';
import './candidate.edit.scss';

import utils from './../../utils.js';

const LIST_OF_THESAURUS = ['industry', 'level', 'city', 'language', 'languageLevel', 'source',
   'department', 'typeOfEmployment', 'tag', 'skill', 'stage', 'country', 'currency', 'socialNetwork', 'eventtype'];

const IMAGE_UPLOADER_MODAL_NAME     = 'basicModal';
const CLOSE_MODAL_EVENT_NAME        = 'close';
const CANDIDATE_PROFILE_VIEW_NAME   = 'candidateProfile';
const STATE_FOR_REMOVE              = 1;
const IMAGE_FILE_UPLOADER_SELECTOR  = '#photoUpload';
const FOUNDATION_CLOSE_EVENT        = 'close';

let curriedSet = curry(set, 3);

export default function CandidateController( // eslint-disable-line max-params, max-statements
   $q,
   $element,
   $scope,
   $state,
   $translate,
   $window,
   CandidateService,
   ValidationService,
   FileService,
   ThesaurusService,
   UserDialogService,
   LoggerService,
   EventsService,
   UserService,
   VacancyService,
   CVParserService,
   FoundationApi,
   TransitionsService
) {
   'ngInject';

   const vm = $scope;

   vm.keys        = Object.keys;
   vm.candidate   = {};
   vm.thesaurus   = {};
   vm.locations   = [];
   vm.duplicates  = $state.params.duplicates || [];
   vm.candidateCVLoaded    = false;

   vm.vacancyIdToGoBack    = $state.params.vacancyIdToGoBack;
   vm.candidate.cvText     = vm.candidate.cvText || [];
   vm.candidate.comments   = $state.params._data ? $state.params._data.comments : [];
   vm.candidateEvents      = $state.params._data ? $state.params._data.events : [];
   vm.comments             = cloneDeep(vm.candidate.comments);
   vm.cloneCandidateEvents = clone(vm.candidateEvents);

   vm.forceSaveCandidate   = forceSaveCandidate;
   vm.saveWithVerify       = saveWithVerify;
   vm.clearUploaderQueue   = clearUploaderQueue;
   vm.addFilesForRemove    = addFilesForRemove;
   vm.saveComment          = saveComment;
   vm.removeComment        = removeComment;
   vm.saveEvent            = saveEvent;
   vm.removeEvent          = removeEvent;
   vm.back                 = back;
   vm.viewCandidate        = viewCandidate;
   vm.submit               = submit;
   vm.locationsSort        = utils.locationsSort;

   (function _init() {
      _initThesauruses()
         .then(_initCandidate)
         .then(_initLocations);
      _initUploaders();
   } ());

   function clearUploaderQueue(uploader, name) {
      uploader.clearQueue();
      $element[0].querySelector(name).value = null;
   }

   function _onError(messageToShow, messageToLog) {
      LoggerService.error(messageToLog || messageToShow);
      UserDialogService.notification(messageToShow, 'error');
   }

   function _initThesauruses() {
      return ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then(curriedSet(vm, 'thesaurus'));
   }

   function _getCandidate() {
      if ($state.params.candidateId) {
         return CandidateService.getCandidate($state.params.candidateId);
      } else if ($state.params.notSavedCandidate) {
         return $q.when($state.params.notSavedCandidate);
      } else {
         return $q.when({});
      }
   }

   function _setCandidateProperties(candidate) {
      candidate.skills = candidate.skills || [];
      candidate.tags = candidate.tags || [];
      candidate.files = candidate.files || [];
      candidate.languageSkills = candidate.languageSkills || [];
      candidate.convertedSocials = candidate.convertedSocials || [];
      _addAdditionProperties(candidate);
      return candidate;
   }

   function _initCandidate() {
      _getCandidate()
         .then(curriedSet(vm, 'candidate'))
         .then(() => _setCandidateProperties(vm.candidate));
   }

   function _initUploaders() {
      _initImageUploader();
      _initFilesUploader();
      _initCVUploader();
      FoundationApi.subscribe(IMAGE_UPLOADER_MODAL_NAME, imageUploadModalCallback);
   }


   function _initImageUploader() {
      let uploader = FileService.getFileUploader({ maxSize: 1024000 });
      uploader.onSuccessItem = (item, response, status, headers) => {
         LoggerService.log('onSuccessItem', item, response, status, headers);
         UserDialogService.notification($translate.instant('Success'), 'success');
         let parsedResponse = JSON.parse(item._xhr.response);
         if (vm.candidate.photo) {
            FileService.remove(vm.candidate.photo);
         }
         FoundationApi.publish(IMAGE_UPLOADER_MODAL_NAME, CLOSE_MODAL_EVENT_NAME);
         vm.candidate.photo = parsedResponse;
      };
      uploader.onErrorItem = (fileItem, response, status, headers) => {
         LoggerService.error('onErrorItem', fileItem, response, status, headers);
         UserDialogService.notification($translate.instant('COMMON.FILE_UPLOADER_ERROR_MESSAGE'), 'error');
      };
      uploader.onWhenAddingFileFailed = () => {
         UserDialogService.notification($translate.instant('COMMON.FILE_UPLOADER_ERROR_MESSAGE'), 'error');
      };
      vm.imageUploader = uploader;
   }

   function _initFilesUploader() {
      let uploader = FileService.getFileUploader({ maxSize: 1024000 });
      uploader.onSuccessItem = (item, response, status, headers) => {
         LoggerService.log('onSuccessItem', item, response, status, headers);
         UserDialogService.notification($translate.instant('Success'), 'success');
         let parsedResponse = JSON.parse(item._xhr.response);
         vm.candidate.files.push(parsedResponse);
      };
      uploader.onCompleteAll = () => {
         _saveCandidate();
         clearUploaderQueue(vm.filesUploader, '#filesUploader');
      };
      uploader.onErrorItem = (fileItem, response, status, headers) => {
         LoggerService.error('onErrorItem', fileItem, response, status, headers);
         UserDialogService.notification($translate.instant('COMMON.FILE_UPLOADER_ERROR_MESSAGE'), 'error');
      };
      uploader.onWhenAddingFileFailed = () => {
         UserDialogService.notification($translate.instant('COMMON.FILE_UPLOADER_ERROR_MESSAGE'), 'error');
      };
      vm.filesUploader = uploader;
   }

   vm.uploadCV = () => {
      let nativeUploadButton = document.querySelector('#cvUploader');
      nativeUploadButton.click();
   };

   vm.getWordsArray = (line) => {
      return CVParserService.getWordsArray(line);
   };

   vm.resolveWordClass = (word) => {
      return CVParserService.resolveWordClass(word, vm.candidate);
   };

   function _initCVUploader() {
      let uploader = FileService.getFileUploader({ maxSize: 1024000 });
      uploader.onSuccessItem = (item, response, status, headers) => {
         let parsedResponse = JSON.parse(item._xhr.response);
         vm.candidate.files.push(parsedResponse);
         CVParserService.parseCandidateCV(parsedResponse.filePath).then((candidate) => {
            LoggerService.log('CV Parsed', item, response, status, headers);
            UserDialogService.notification($translate.instant('CV was sucessfuly parsed'), 'success');
            if (candidate.experienceYears) {
               candidate.experienceYears = parseInt(candidate.experienceYears);
            }
            vm.candidate = assign(vm.candidate, candidate);
            vm.candidate.description = candidate.text.join('<br>');
         });
      };
      uploader.onCompleteAll = () => {
         clearUploaderQueue(vm.cvUploader, '#cvUploader');
      };

      uploader.onAfterAddingFile = (item) => {
         uploader.uploadItem(item);
      };

      uploader.onErrorItem = (fileItem, response, status, headers) => {
         LoggerService.error('onErrorItem', fileItem, response, status, headers);
         UserDialogService.notification($translate.instant('COMMON.FILE_UPLOADER_ERROR_MESSAGE'), 'error');
      };
      uploader.onWhenAddingFileFailed = () => {
         UserDialogService.notification($translate.instant('COMMON.FILE_UPLOADER_ERROR_MESSAGE'), 'error');
      };
      vm.cvUploader = uploader;
   }

   function _deleteEmptyPhoneNumber(candidate) {
      remove(candidate.phoneNumbers, phone => isEmpty(phone.number));
      return candidate;
   }

   function saveAndGoBack() {
      saveCandidate().then((candidate) => {
         if (vm.vacancyIdToGoBack) {
            TransitionsService.back('vacancyView',
               { vacancyId: vm.vacancyIdToGoBack, candidatesIds: [ candidate.id ] });
         } else {
            TransitionsService.back(CANDIDATE_PROFILE_VIEW_NAME,
                                 { candidateId : candidate.id });
         }
         return candidate;
      });
   };

   function forceSaveCandidate() {
      saveAndGoBack();
   }

   function saveWithVerify() {
      let candidateToCompare = cloneDeep(vm.candidate);
      _deleteAdditionProperties(candidateToCompare);
      CandidateService.getDuplicates(candidateToCompare)
         .then(duplicates => {
            if (vm.candidate.id) {
               remove(duplicates, {id: vm.candidate.id});
            }
            vm.duplicates = duplicates;
            if (duplicates.length) {
               return $q.reject($translate.instant('DIALOG_SERVICE.ERROR_SIMILAR_CANDIDATES'));
            }
            return saveAndGoBack();
         })
         .catch(errorMes => {
            _onError(errorMes);
         });
   }

   function saveCandidate() {
      if (isEmpty(vm.filesUploader.getNotUploadedItems())) {
         return _saveCandidate();
      } else {
         vm.filesUploader.uploadAll();
      }
   }

   function submit(form) {
      ValidationService.validate(form).then(saveWithVerify);
   }

   function addFilesForRemove(file) {
      FileService.remove(file).then(() => remove(vm.candidate.files, file));
   }

   function _saveCandidate() {
      let memo = vm.candidate.comments;
      vm.candidate.comments = vm.comments;
      _deleteAdditionProperties(vm.candidate);
      return CandidateService.saveCandidate(vm.candidate)
         .then(candidate => {
            set(vm, 'candidate', candidate);
            return candidate;
         })
         .then(() => _addAdditionProperties(vm.candidate))
         .then(entity => {
            vm.comments = cloneDeep(vm.candidate.comments);
            UserDialogService.notification($translate.instant('DIALOG_SERVICE.SUCCESSFUL_CANDIDATE_SAVING'), 'success');
            return entity;
         })
         .catch((reason) => {
            _onError($translate.instant('DIALOG_SERVICE.ERROR_CANDIDATE_SAVING'), reason);
            vm.candidate.comments = memo;
         });
   }

   function _addAdditionProperties(candidate) {
      _addEmptySocials(candidate);
      _groupSocials(3);
      _initPhones(candidate);
      return candidate;
   }

   function _deleteAdditionProperties(candidate) {
      _deleteEmptyPhoneNumber(candidate);
      _deleteEmptySocials(candidate);
   }

   function _initLocations() {
      forEach(vm.thesaurus.country, country => {
         vm.locations.push({ country });
      });
      forEach(vm.thesaurus.city, city => {
         vm.locations.push({ country: city.countryObject, city });
      });
   }

   function _addEmptySocials(candidate) {
      candidate.convertedSocials = candidate.convertedSocials || [];
      forEach(vm.thesaurus.socialNetwork, social => {
         let candidateSocial = find(vm.candidate.socialNetworks,
            _candidateSocial => _candidateSocial.socialNetworkId === social.id);
         if (!candidateSocial) {
            candidate.convertedSocials.push({ socialNetwork: social });
         }
      });
      return candidate;
   }

   function _deleteEmptySocials(candidate) {
      remove(candidate.convertedSocials, social => !social.path);
      remove(candidate.socialNetworks, social => !social.id);
      return candidate;
   }

   function _groupSocials(chunkSize) {
      vm.socialGroups = chunk(vm.candidate.convertedSocials, chunkSize);
   }

   function _initPhones(candidate) {
      candidate.phoneNumbers = candidate.phoneNumbers || [];
      candidate.phoneNumbers.push({});
      return candidate;
   }

   function saveComment(comment) {
      if (comment.id) {
         set(find(vm.comments, comment), 'message', comment.message);
      } else {
         set(comment, 'authorId', UserService.getCurrentUser().id);
         vm.comments.push(comment);
      }
      return $q.when(comment);
   }

   function removeComment(comment) {
      if (comment.id) {
         set(find(vm.comments, comment), 'state', STATE_FOR_REMOVE);
         return $q.when(comment);
      } else {
         remove(vm.comments, comment);
         return $q.when(comment);
      }
   }

   function _getCandidateEvents(candidateId) {
      EventsService.getEventsByCandidate(candidateId).then(events => {
         set(vm, 'candidateEvents', events);
         vm.cloneCandidateEvents = clone(vm.candidateEvents);
      });
   }
   function saveEvent(event) {
      EventsService.save(event).then(() => {
         _getCandidateEvents(vm.candidate.id);
         vm.cloneCandidateEvents = clone(vm.candidateEvents);
      });
   }

   function removeEvent(event) {
      EventsService.remove(event).then(() => {
         remove(vm.candidateEvents, { id: event.id });
         vm.cloneCandidateEvents = clone(vm.candidateEvents);
         UserDialogService.notification($translate.instant('DIALOG_SERVICE.SUCCESSFUL_REMOVING_EVENT'), 'success');
      });
   }

   vm.isNeedToGoBack = () => {
      return vm.vacancyIdToGoBack;
   };

   function back() {
      TransitionsService.back();
   }

   function viewCandidate(candidate) {
      TransitionsService.go(
         CANDIDATE_PROFILE_VIEW_NAME,
         {
            candidateId: candidate.id,
            candidatePredicate: vm.candidatePredicate
         }, { duplicates : vm.duplicates,
              notSavedCandidate : vm.candidate});
   }

   function imageUploadModalCallback(message) {
      if (message === FOUNDATION_CLOSE_EVENT && !vm.imageUploader.isUploading) {
         clearUploaderQueue(vm.imageUploader, IMAGE_FILE_UPLOADER_SELECTOR);
      }
   }
}
