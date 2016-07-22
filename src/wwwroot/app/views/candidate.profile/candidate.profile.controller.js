const LIST_OF_THESAURUS = ['stage', 'eventtype'];
import {
   set,
   cloneDeep,
   remove,
   clone,
   find
} from 'lodash';

export default function CandidateProfileController(
   $scope,
   $q,
   $translate,
   $state,
   FileService,
   UserDialogService,
   ThesaurusService,
   CandidateService,
   LoggerService,
   EventsService,
   UserService,
   VacancyService
   ) {
   'ngInject';

   const vm                  = $scope;
   vm.uploader               = _createNewUploader();
   vm.addFilesForRemove      = addFilesForRemove;
   vm.queueFileIdsForRemove  = [];
   vm.saveChanges            = saveChanges;
   vm.isChanged              = false;
   vm.candidate              = {};
   vm.editCandidate          = editCandidate;
   vm.candidate.comments     = $state.params._data ? $state.params._data.comments : vm.candidate.comments;
   vm.candidate.files        = $state.params._data ? $state.params._data.files : vm.candidate.files;
   vm.comments               = cloneDeep(vm.candidate.comments);
   vm.saveComment            = saveComment;
   vm.removeComment          = removeComment;
   vm.editComment             = editComment;
   vm.candidateEvents         = [];
   vm.cloneCandidateEvents    = [];
   vm.saveEvent               = saveEvent;
   vm.removeEvent             = removeEvent;

   function _init() {
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then(topics => set(vm, 'thesaurus', topics));
      _initCurrentCandidate();
      _initDataForEvents();
   }
   _init();

   function _initCurrentCandidate() {
      if ($state.params._data) {
         vm.candidate = $state.params._data;
         _getCandidateEvents(vm.candidate.id);
      } else {
         CandidateService.getCandidate($state.params.candidateId).then(candidate => {
            set(vm, 'candidate', candidate);
            vm.comments = cloneDeep(vm.candidate.comments);
            _getCandidateEvents(vm.candidate.id);
         });
      }
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

   function _createNewUploader() {
      let newUploader = FileService.getFileUploader({ onCompleteAllCallBack : saveChanges, maxSize : 2048000 });
      newUploader.onSuccessItem = function onSuccessUpload(item) {
         let response = JSON.parse(item._xhr.response);
         vm.candidate.files.push(response);
         vm.isChanged = false;
      };
      newUploader.onWhenAddingFileFailed = function onAddingFileFailed() {
         UserDialogService.notification($translate.instant('COMMON.FILE_UPLOADER_ERROR_MESSAGE'), 'warning');
      };
      newUploader.onAfterAddingAll = function onAfterAddingAl() {
         vm.isChanged = true;
      };
      return newUploader;
   }

   function addFilesForRemove(file) {
      vm.queueFileIdsForRemove.push(file.id);
      remove(vm.candidate.files, {id: file.id});
      vm.isChanged = true;
   }

   function editCandidate() {
      $state.go('candidate', {_data: vm.candidate, candidateId: vm.candidate.id});
   }

   function saveChanges() {
      if (vm.uploader.getNotUploadedItems().length) {
         vm.uploader.uploadAll();
      } else if (vm.queueFileIdsForRemove.length) {
         FileService.removeGroup(vm.queueFileIdsForRemove).then(() => {
            vm.queueFileIdsForRemove = [];
            _candidateSave();
         });
      } else {
         _candidateSave();
      }
   }

   function _candidateSave() {
      let memo = vm.candidate.comments;
      vm.candidate.comments = vm.comments;
      CandidateService.saveCandidate(vm.candidate).then(candidate => {
         vm.candidate = candidate;
         vm.comments = cloneDeep(vm.candidate.comments);
         UserDialogService.notification($translate.instant('DIALOG_SERVICE.SUCCESSFUL_CANDIDATE_SAVING'), 'success');
         vm.isChanged = false;
         vm.uploader.clearQueue();
      }).catch((error) => {
         vm.candidate.comments = memo;
         UserDialogService.notification($translate.instant('DIALOG_SERVICE.ERROR_CANDIDATE_SAVING'), 'error');
         LoggerService.error(error);
      });
   }

   function saveComment(comment) {
      vm.isChanged = true;
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
}
