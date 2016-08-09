const LIST_OF_THESAURUS = ['stage', 'eventtype'];
import {
   set,
   cloneDeep,
   remove,
   clone,
   find,
   forEach,
   map,
   maxBy
} from 'lodash';

export default function CandidateProfileController( // eslint-disable-line max-statements
   $scope,
   $q,
   $translate,
   $state,
   $window,
   FileService,
   UserDialogService,
   ThesaurusService,
   CandidateService,
   VacancyService,
   LoggerService,
   EventsService,
   UserService
   ) {
   'ngInject';

   const vm                  = $scope;
   vm.uploader               = _createNewUploader();
   vm.addFilesForRemove      = addFilesForRemove;
   vm.queueFileIdsForRemove  = [];
   vm.saveChanges            = saveChanges;
   vm.isChanged              = false;
   vm.candidate              = $state.params._data ? $state.params._data : {};
   vm.editCandidate          = editCandidate;
   vm.back                   = back;
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
   vm.composedBy              = [];

   function _init() {
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then(topics => set(vm, 'thesaurus', topics));
      _initCurrentCandidate()
         .then(addVacanciesToCandidateIfNeeded)
         .then(_recompose)
         .then(_fillWithCandidates)
         .then(_fillWithVacancies)
         .then((vacancyStagesObject) => {
            vm.composedBy = vacancyStagesObject;
            vm.$watch('composedBy', () => {
               vm.isChanged = true;
            });
         }).catch(LoggerService.error);
   }
   _initDataForEvents();
   _init();

   function addVacanciesToCandidateIfNeeded() {
      let deffered = $q.defer();
      if ($state.params.vacancies && $state.params.vacancies.length) {
         forEach($state.params.vacancies, (v) => {
            let newVSI = {
               vacancyId: v.id,
               candidateId: vm.candidate.id,
               comment: null,
               isPassed: false,
               stageId: find(v.stageFlow, { order: 1 }).id,
               createdOn: (new Date()).toISOString()
            };
            vm.candidate.vacanciesProgress.push(newVSI);
         });
      }
      deffered.resolve();
      return deffered.promise;
   }

   function _recomposeBack(composedBy) {
      let newVacanciesProgress = [];
      forEach(composedBy, (stageObject) => {
         forEach(stageObject.stages, (vsi) => {
            newVacanciesProgress.push(vsi);
         });
      });
      return newVacanciesProgress;
   }

   function _initCurrentCandidate() {
      let deffered = $q.defer();
      if ($state.previous.params._data || $state.params.candidateId) {
         CandidateService.getCandidate($state.params.candidateId).then(candidate => {
            set(vm, 'candidate', candidate);
            vm.comments = cloneDeep(vm.candidate.comments);
            _getCandidateEvents(vm.candidate.id);
            deffered.resolve();
         });
      } else {
         vm.candidate = $state.params._data;
         _getCandidateEvents(vm.candidate.id);
         deffered.resolve();
      }
      return deffered.promise;
   }

   function _recompose() {
      let vacancyStageInfos = vm.candidate.vacanciesProgress;
      let deffered = $q.defer();
      let composedBy = [];
      forEach(vacancyStageInfos, (vsi) => {
         let composedEntity = find(composedBy, { vacancyId: vsi.vacancyId });
         if (composedEntity) {
            composedEntity.stages.push(vsi);
         } else {
            composedBy.push({
               candidateId: vsi.candidateId,
               vacancyId: vsi.vacancyId,
               stages: [ vsi ]
            }
            );
         }
      });

      let composedWithStages = map(composedBy, (obj) => {
         let currentStage = find(obj.stages, { isPassed: false });

         if (currentStage) {
            obj.currentStageId = currentStage.stageId;
            obj.selectedStageId = currentStage.stageId;
            obj.selectedStageIsPassed = currentStage.isPassed;
         } else if (obj.stages.length > 1) {
            let foundLastStage = maxBy(obj.stages, 'stageId');
            obj.currentStageId = foundLastStage.stageId;
            obj.selectedStageId = foundLastStage.stageId;
            obj.selectedStageIsPassed = foundLastStage.isPassed;
         } else {
            obj.currentStageId = 1;
            obj.selectedStageId = 1;
            obj.selectedStageIsPassed = false;
         }
         return obj;
      });
      deffered.resolve(composedWithStages);
      return deffered.promise;
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


   function _fillWithCandidates(recomposed) {
      let deffered = $q.defer();
      deffered.resolve(map(recomposed, _loadCandidates));
      return deffered.promise;
   }

   function _fillWithVacancies(recomposed) {
      let deffered = $q.defer();
      deffered.resolve(map(recomposed, _loadVacancies));
      return deffered.promise;
   }

   function _loadCandidates(candidateStagesObject) {
      let stagesObjectWithCandidate = candidateStagesObject;
      CandidateService.getCandidate(candidateStagesObject.candidateId).then(value => {
         stagesObjectWithCandidate.candidate = value;
      });
      return stagesObjectWithCandidate;
   }

   function _loadVacancies(vacanciesStagesObject) {
      let stagesObjectWithCandidate = vacanciesStagesObject;
      VacancyService.getVacancy(vacanciesStagesObject.vacancyId).then(value => {
         stagesObjectWithCandidate.vacancy = value;
         stagesObjectWithCandidate.stageFlow = value.stageFlow;
      });
      return stagesObjectWithCandidate;
   }

   vm.goToVacancies = () => {
      $state.go('vacancies', { _data: null, candidateIdToGoBack: vm.candidate.id });
   };

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

   function back() {
      $window.history.back();
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
      vm.candidate.vacanciesProgress = _recomposeBack(vm.composedBy);
      CandidateService.saveCandidate(vm.candidate).then(candidate => {
         vm.candidate = candidate;
         vm.comments = cloneDeep(vm.candidate.comments);
         UserDialogService.notification($translate.instant('DIALOG_SERVICE.SUCCESSFUL_CANDIDATE_SAVING'), 'success');
         vm.isChanged = false;
         vm.uploader.clearQueue();
         $state.go($state.previous.name, $state.previous.params);
      }).catch((error) => {
         vm.candidate.comments = memo;
         UserDialogService.notification($translate.instant('DIALOG_SERVICE.ERROR_CANDIDATE_SAVING'), 'error');
         LoggerService.error(error);
      });
   }

   function saveComment(comment) {
      vm.isChanged = true;
      let currentUser = UserService.getCurrentUser();
      comment.authorId = currentUser.id;
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
