import { set, forEach, remove, chunk, isEmpty, cloneDeep, clone, find } from 'lodash';

import './candidate.edit.scss';

const LIST_OF_THESAURUS = ['industry', 'level', 'city', 'language', 'languageLevel', 'source',
    'department', 'typeOfEmployment', 'tag', 'skill', 'stage', 'country', 'currency', 'socialNetwork'];

export default function CandidateController(//eslint-disable-line max-statements
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
   EventsService
   ) {
   'ngInject';

   const vm          = $scope;
   vm.keys           = Object.keys;
   vm.candidate      = vm.candidate || {};
   vm.thesaurus      = {};
   vm.languages      = [];
   vm.locations      = [];
   vm.saveCandidate        = saveCandidate;
   vm.clearUploaderQueue   = clearUploaderQueue;
   vm.addFilesForRemove    = addFilesForRemove;
   vm.candidate.comments     = $state.params._data ? $state.params._data.comments : vm.candidate.comments;
   vm.comments               = cloneDeep(vm.candidate.comments);
   vm.saveComment            = saveComment;
   vm.removeComment          = removeComment;
   vm.editComment             = editComment;
   vm.candidateEvents         = [];
   vm.cloneCandidateEvents    = [];
   vm.saveEvent               = saveEvent;
   vm.removeEvent             = removeEvent;
   vm.clear                   = clear;

   (function _init() {
      _initThesauruses();
      _initUploaders();
      _initCandidate();
   }());

   function clearUploaderQueue(uploader, name) {
      uploader.clearQueue();
      $element[0].querySelector(name).value = null;
   }

   function _onError(message) {
      LoggerService.error(message);
   }

   function _initThesauruses() {
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS)
         .then(data => vm.thesaurus = data)
         .then(_initLanguages)
         .then(_initLocations)
         .then(() => _addEmptySocials());
   }

   function _initCandidate() {
      if ($state.params._data) {
         vm.candidate = $state.params._data;
         vm.candidate.skills = $state.params._data.skills;
         vm.candidate.tags = $state.params._data.tags;
         vm.candidate.files = $state.params._data.files;
         vm.candidate.languageSkills = $state.params._data.languageSkills;
         vm.candidate.convertedSocials = $state.params._data.convertedSocials || [];
         _initPhones();
      } else if ($state.params.candidateId) {
         CandidateService.getCandidate($state.params.candidateId).then(candidate => {
            set(vm, 'candidate', candidate);
            vm.candidate.skills     = vm.candidate.skills || [];
            vm.candidate.tags       = vm.candidate.tags || [];
            vm.candidate.files      = vm.candidate.files || [];
            vm.candidate.languageSkills = vm.candidate.languageSkills || [];
            vm.candidate.convertedSocials = vm.candidate.convertedSocials || [];
            _initPhones();
         });
      } else {
         vm.candidate = {};
         vm.candidate.skills     = [];
         vm.candidate.tags       = [];
         vm.candidate.files      = [];
         vm.candidate.languageSkills = [];
         vm.candidate.convertedSocials = [];
         _initPhones();
      }
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

   function _deleteEmptyPhoneNumber() {
      remove(vm.candidate.phoneNumbers, phone => isEmpty(phone.number));
   }

   function saveCandidate(form) {
      if (ValidationService.validate(form)) {
         if (isEmpty(vm.filesUploader.getNotUploadedItems())) {
            _saveCandidate();
         } else {
            vm.filesUploader.uploadAll();
         }
      }
   }

   function addFilesForRemove(file) {
      FileService.remove(file).then(() => remove(vm.candidate.files, file));
   }

   function _saveCandidate() {
      let memo = vm.candidate.comments;
      vm.candidate.comments = vm.comments;
      _deleteEmptyPhoneNumber();
      _removeEmptySocials();
      CandidateService.saveCandidate(vm.candidate)
         .then(entity => {
            set(vm, 'candidate', entity);
            _addEmptySocials();
            _initPhones();
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

   function _initLanguages(thesauruses) {
      forEach(thesauruses.language, language => {
         vm.languages.push({language});
         forEach(thesauruses.languageLevel, languageLevel => {
            vm.languages.push({language, languageLevel});
         });
      });
      return thesauruses;
   }

   function _initLocations(thesauruses) {
      forEach(thesauruses.country, country => {
         vm.locations.push({country});
      });
      forEach(thesauruses.city, city => {
         vm.locations.push({country : city.countryObject, city});
      });
      return thesauruses;
   }

   function _addEmptySocials() {
      forEach(vm.thesaurus.socialNetwork, social => {
         let candidateSocial = find(vm.candidate.socialNetworks,
            _candidateSocial => _candidateSocial.socialNetworkId === social.id);
         if (!candidateSocial) {
            vm.candidate.convertedSocials.push({socialNetwork : social});
         }
      });
      _groupSocials(3);
   }

   function _removeEmptySocials() {
      remove(vm.candidate.convertedSocials, social => !social.path);
   }

   function _groupSocials(chunkSize) {
      vm.socialGroups = chunk(vm.candidate.convertedSocials, chunkSize);
   }

   function _initPhones() {
      vm.candidate.phoneNumbers = vm.candidate.phoneNumbers || [];
      vm.candidate.phoneNumbers.push({});
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

   function clear() {
      $state.go('candidate', {_data: null, candidateId: null});
   }
}
