import { set, forEach, remove, chunk, isEmpty, cloneDeep, clone, find, curry } from 'lodash';

import './candidate.edit.scss';

const LIST_OF_THESAURUS = ['industry', 'level', 'city', 'language', 'languageLevel', 'source',
    'department', 'typeOfEmployment', 'tag', 'skill', 'stage', 'country', 'currency', 'socialNetwork', 'eventtype'];

let curriedSet = curry(set, 3);

export default function CandidateController( //eslint-disable-line max-statements
   $q,
   $element,
   $scope,
   $state,
   $translate,
   CandidateService,
   ValidationService,
   FileService,
   ThesaurusService,
   UserDialogService,
   LoggerService,
   EventsService,
   UserService,
   VacancyService
   ) {
   'ngInject';

   const vm                = $scope;
   vm.keys                 = Object.keys;
   vm.candidate            = vm.candidate || {};
   vm.thesaurus            = {};
   vm.locations            = [];
   vm.saveCandidate        = saveCandidate;
   vm.clearUploaderQueue   = clearUploaderQueue;
   vm.addFilesForRemove    = addFilesForRemove;
   vm.candidate.comments     = $state.params._data ? $state.params._data.comments : [];
   vm.comments             = cloneDeep(vm.candidate.comments);
   vm.saveComment          = saveComment;
   vm.removeComment        = removeComment;
   vm.editComment          = editComment;
   vm.candidateEvents      = [];
   vm.cloneCandidateEvents = [];
   vm.saveEvent            = saveEvent;
   vm.removeEvent          = removeEvent;
   vm.clear                = clear;
   vm.vacancyIdToGoBack       = $state.params.vacancyIdToGoBack;

   (function _init() {
      _initDataForEvents();
      _initThesauruses()
         .then(_initCandidate)
         .then(_initLocations);
      _initUploaders();
   }());

   function clearUploaderQueue(uploader, name) {
      uploader.clearQueue();
      $element[0].querySelector(name).value = null;
   }

   function _onError(message) {
      LoggerService.error(message);
      UserDialogService.notification(message);
   }

   function _initThesauruses() {
      return ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then(curriedSet(vm, 'thesaurus'));
   }

   function _initDataForEvents() {
      vm.vacancies                  = [];
      vm.candidates                 = [];
      vm.responsibles               = [];
      vm.vacancyPredicat            = {};
      vm.vacancyPredicat.current    = 0;
      vm.vacancyPredicat.size       = 30;
      vm.candidatePredicat          = {};
      vm.candidatePredicat.current  = 0;
      vm.candidatePredicat.size     = 20;
      CandidateService.search(vm.candidatePredicat).then(data  => set(vm, 'candidates', data.candidate));
      UserService.getUsers().then(users => set(vm, 'responsibles', users));
      VacancyService.search(vm.vacancyPredicat).then(response => vm.vacancies = response.vacancies);
   }

   function _getCandidate() {
      if ($state.params._data) {
         return $q.when($state.params._data);
      } else if ($state.params.candidateId) {
         return CandidateService.getCandidate($state.params.candidateId);
      } else {
         return $q.when({});
      }
   }

   function _setCandidateProperties(candidate) {
      candidate.skills           = candidate.skills || [];
      candidate.tags             = candidate.tags || [];
      candidate.files            = candidate.files || [];
      candidate.languageSkills   = candidate.languageSkills || [];
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
   }

   function _initImageUploader() {
      let uploader = FileService.getFileUploader({maxSize: 1024000});
      uploader.onSuccessItem = (item, response, status, headers) => {
         LoggerService.log('onSuccessItem', item, response, status, headers);
         UserDialogService.notification($translate.instant('Success'), 'success');
         let parsedResponse = JSON.parse(item._xhr.response);
         if (vm.candidate.photo) {
            FileService.remove(vm.candidate.photo);
         }
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
      let uploader = FileService.getFileUploader({maxSize: 1024000});
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

   function _deleteEmptyPhoneNumber(candidate) {
      remove(candidate.phoneNumbers, phone => isEmpty(phone.number));
      return candidate;
   }

   vm.saveAndGoBack = (form) => {
      saveCandidate(form).then(() => {
         $state.go('vacancyView', { vacancyId: vm.vacancyIdToGoBack, 'candidatesIds': [ vm.candidate.id ] });
      });
   };

   function saveCandidate(form) {
      return ValidationService.validate(form).then(() => {
         if (isEmpty(vm.filesUploader.getNotUploadedItems())) {
            return _saveCandidate();
         } else {
            vm.filesUploader.uploadAll();
         }
      });
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
         .catch(() => {
            _onError();
            vm.candidate.comments = memo;
            UserDialogService.notification($translate.instant('DIALOG_SERVICE.ERROR_CANDIDATE_SAVING'), 'error');
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
         vm.locations.push({country});
      });
      forEach(vm.thesaurus.city, city => {
         vm.locations.push({country : city.countryObject, city});
      });
   }

   function _addEmptySocials(candidate) {
      candidate.convertedSocials = candidate.convertedSocials || [];
      forEach(vm.thesaurus.socialNetwork, social => {
         let candidateSocial = find(vm.candidate.socialNetworks,
            _candidateSocial => _candidateSocial.socialNetworkId === social.id);
         if (!candidateSocial) {
            candidate.convertedSocials.push({socialNetwork : social});
         }
      });
      return candidate;
   }

   function _deleteEmptySocials(candidate) {
      remove(candidate.convertedSocials, social => !social.path);
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
      return $q.when(vm.comments.push(comment));
   }

   function removeComment(comment) {
      vm.isChanged = true;
      let commentForRemove = find(vm.comments, comment);
      if (comment.id) {
         commentForRemove.state = 1;
         remove(vm.comments, comment);
         return $q.when(vm.comments.push(commentForRemove));
      } else {
         return $q.when(remove(vm.comments, comment));
      }
   }

   function editComment(comment) {
      vm.isChanged = true;
      return $q.when(remove(vm.comments, comment));
   }

   function _getCandidateEvents(candidateId) {
      EventsService.getEventsByCandidate(candidateId).then(events => {
         set(vm, 'candidateEvents', events);
         vm.cloneCandidateEvents  = clone(vm.candidateEvents);
      });
   }
   function saveEvent(event) {
      EventsService.save(event).then(() => {
         _getCandidateEvents(vm.candidate.id);
         vm.cloneCandidateEvents  = clone(vm.candidateEvents);
      });
   }

   function removeEvent(event) {
      EventsService.remove(event).then(() => {
         remove(vm.candidateEvents, {id: event.id});
         vm.cloneCandidateEvents  = clone(vm.candidateEvents);
         UserDialogService.notification($translate.instant('DIALOG_SERVICE.SUCCESSFUL_REMOVING_EVENT'), 'success');
      });
   }

   vm.isNeedToGoBack = () => {
      if (vm.vacancyIdToGoBack) {
         return true;
      }
      return false;
   };

   function clear() {
      $state.go('candidate', {_data: null, candidateId: null});
   }
}
