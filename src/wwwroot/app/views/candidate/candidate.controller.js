import { set, forEach, remove } from 'lodash';

const LIST_OF_THESAURUS = ['industry', 'level', 'city', 'language', 'languageLevel',
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

   vm.candidate.skills     = vm.candidate.skills || [];
   vm.candidate.tags       = vm.candidate.tags || [];
   vm.candidate.files      = vm.candidate.files || [];
   vm.candidate.phoneNumbers   = vm.candidate.phoneNumbers || [ {} ];;
   vm.candidate.languageSkills = vm.candidate.languageSkills || [];
   vm.candidate.convertedSocials = vm.candidate.convertedSocials || [];

   vm.saveCandidate        = saveCandidate;
   vm.clearUploaderQueue   = clearUploaderQueue;

   (function _init() {
      _initThesauruses();
      _createUploaders();
   }());

   //example of name is '#upload' ;
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

   function _createUploaders() {
      vm.filesUploader = FileService.getFileUploader({maxSize: 1024000});
      vm.imageUploader = FileService.getFileUploader({maxSize: 1024000});
      forEach([vm.filesUploader, vm.imageUploader], uploader => {
         uploader.onSuccessItem = (item, response, status, headers) => {
            LoggerService.log('onSuccessItem', item, response, status, headers);
         };
         uploader.onErrorItem = (fileItem, response, status, headers) => {
            LoggerService.error('onErrorItem', fileItem, response, status, headers);
         };
         uploader.onWhenAddingFileFailed = () => {
            UserDialogService.notification($translate.instant('COMMON.FILE_UPLOADER_ERROR_MESSAGE'), 'error');
         };
      });
   }

   function _deleteEmptyPhoneNumber() {
      remove(vm.candidate.phoneNumbers, {});
   }

   function saveCandidate(form) {
      if (ValidationService.validate(form)) {
         _deleteEmptyPhoneNumber();
         _removeEmptySocials();
         CandidateService.saveCandidate(vm.candidate)
            .then(entity => {
               set(vm, 'candidate', entity);
               vm.candidate.phoneNumbers = entity.phoneNumbers || [];
               vm.candidate.phoneNumbers.push({});
               _addEmptySocials();
               return entity;
            })
            .catch(_onError);
      }
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
         forEach(thesauruses.city, city => {
            if (city.countryId === country.id) {
               vm.locations.push({country, city});
            }
         });
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
   }

   function _removeEmptySocials() {
      remove(vm.candidate.convertedSocials, social => !social.path);
   }
}
