import { set, forEach, remove, chunk, isEmpty } from 'lodash';

import './candidate.edit.scss';

const LIST_OF_THESAURUS = ['industry', 'level', 'city', 'language', 'languageLevel', 'source',
    'department', 'typeOfEmployment', 'tag', 'skill', 'stage', 'country', 'currency', 'socialNetwork'];

export default function CandidateController(
   $element,
   $scope,
   $translate,
   CandidateService,
   ValidationService,
   FileService,
   ThesaurusService,
   UserDialogService,
   LoggerService
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

   (function _init() {
      _initThesauruses();
      _initUploaders();
      _initCandidate();
   }());

   function clearUploaderQueue(uploader, name) {
      uploader.clearQueue();
      $element[0].querySelector(name).value = null;
   }

   function _onError() {
      UserDialogService.notification('Some error was occurred!', 'error');
   }

   function _initThesauruses() {
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS)
         .then(data => vm.thesaurus = data)
         .then(_initLanguages)
         .then(_initLocations)
         .then(() => _addEmptySocials());
   }

   function _initCandidate() {
      vm.candidate.skills     = vm.candidate.skills || [];
      vm.candidate.tags       = vm.candidate.tags || [];
      vm.candidate.files      = vm.candidate.files || [];
      vm.candidate.languageSkills = vm.candidate.languageSkills || [];
      vm.candidate.convertedSocials = vm.candidate.convertedSocials || [];
      _initPhones();
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
      remove(vm.candidate.phoneNumbers, {});
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
      _deleteEmptyPhoneNumber();
      _removeEmptySocials();
      CandidateService.saveCandidate(vm.candidate)
         .then(entity => {
            set(vm, 'candidate', entity);
            _addEmptySocials();
            _initPhones();
            return entity;
         })
         .catch(_onError);
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
}
