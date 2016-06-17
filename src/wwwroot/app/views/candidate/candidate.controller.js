import { set, forEach } from 'lodash';

const LIST_OF_THESAURUS = ['industry', 'level', 'city', 'language', 'languageLevel',
    'department', 'typeOfEmployment', 'tag', 'skill', 'stage', 'country'/*, 'currency'*/];

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

   vm.saveCandidate        = saveCandidate;
   vm.clearUploaderQueue   = clearUploaderQueue;

   (function _init() {
      _initThesauruses();
      _createUploader();
   }());

   function clearUploaderQueue() {
      vm.uploader.clearQueue();
      $element[0].querySelector('#upload').value = null;
   }
   function _onError() {
      UserDialogService.notification('Some error was occurred!', 'error');
   }

   function _initThesauruses() {
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS)
         .then(data => vm.thesaurus = data)
         .then(_initLanguages).then(_initLocations);
   }

   function _createUploader() {
      vm.uploader = FileService.getFileUploader({maxSize: 1024000});
      vm.uploader.onSuccessItem = (item, response, status, headers) => {
         LoggerService.log('onSuccessItem', item, response, status, headers);
      };
      vm.uploader.onErrorItem = (fileItem, response, status, headers) => {
         LoggerService.error('onErrorItem', fileItem, response, status, headers);
      };
      vm.uploader.onWhenAddingFileFailed = () => {
         UserDialogService.notification($translate.instant('COMMON.FILE_UPLOADER_ERROR_MESSAGE'), 'error');
      };
   }

   function saveCandidate(form) {
      if (ValidationService.validate(form)) {
         CandidateService.saveCandidate(vm.candidate)
            .then(entity => set(vm, 'candidate', entity))
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
            vm.locations.push({country, city});
         });
      });
      return thesauruses;
   }
}
