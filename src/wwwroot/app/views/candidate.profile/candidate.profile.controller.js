const LIST_OF_THESAURUS = ['stage', 'eventtype'];
import {
   set,
   cloneDeep,
   remove,
   clone,
   find,
   forEach,
   map,
   filter,
   maxBy
} from 'lodash';

export default function CandidateProfileController( // eslint-disable-line max-statements
   $scope,
   $q,
   $translate,
   $state,
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
   vm.isCandidateLoaded       = false;

   debugger;

   function _init() {
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then(topics => set(vm, 'thesaurus', topics));
      _initCurrentCandidate()
         .then(addVacanciesToCandidateIfNeeded)
         .then(_recompose)
         .then(_fillWithVacancies)
         .then(calculateLatestStages)
         .then(_fillWithCandidates)
         .then((vacancyStagesObject) => {
            vm.composedBy = vacancyStagesObject;
            vm.$watchCollection('composedBy', () => {
               vm.isChanged = true;
            });
            vm.isCandidateLoaded = true;
         }).catch(LoggerService.error);
   }
   _initDataForEvents();
   _init();

   function addVacanciesToCandidateIfNeeded() {
      let deffered = $q.defer();
      debugger;
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
      if ($state.params._data) {
         vm.candidate = $state.params._data;
         _getCandidateEvents(vm.candidate.id);
         deffered.resolve();
      } else {
         CandidateService.getCandidate($state.params.candidateId).then(candidate => {
            set(vm, 'candidate', candidate);
            vm.comments = cloneDeep(vm.candidate.comments);
            _getCandidateEvents(vm.candidate.id);
            debugger;
            deffered.resolve();
         });
      }
      return deffered.promise;
   }

   function _recompose() {
      debugger;
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
      deffered.resolve(composedBy);
      return deffered.promise;
   }

   function calculateLatestStages(composedBy) {
      let composedWithStages = map(composedBy, (vacObject) => {
         let latestStageIdByOrder = 1;
         if (vacObject.stages.length  > 1) {
            let passedStages = filter(vacObject.stages, (vacancyStageInfo) => {
               return vacancyStageInfo.isPassed;
            });
            let avalaiblePassedStages = map(passedStages, (vacancyStageInfo) => {
               return {
                  stage: filter(vm.vacancy.stageFlow, (extStage) => {
                     return extStage.stage.id === vacancyStageInfo.stageId;
                  })[0],
                  vacancyStageInfo
               };
            });
            if (avalaiblePassedStages.length) {
               latestStageIdByOrder = maxBy(avalaiblePassedStages, 'stage.order').stage.id;
            }

         }
         vacObject.latestStageId = latestStageIdByOrder;
         return vacObject;
      });
      console.log(composedWithStages);
      return composedWithStages;
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
      return $q.all(map(recomposed, _loadCandidate));
   }

   function _fillWithVacancies(recomposed) {
      return $q.all(map(recomposed, _loadVacancy));
   }

   function _loadCandidate(candidateStagesObject) {
      let deffered = $q.defer();
      let stagesObjectWithCandidate = candidateStagesObject;
      CandidateService.getCandidate(candidateStagesObject.candidateId).then(value => {
         stagesObjectWithCandidate.candidate = value;
         deffered.resolve(stagesObjectWithCandidate);
      });
      return deffered.promise;
   }

   function _loadVacancy(vacanciesStagesObject) {
      let deffered = $q.defer();
      let stagesObjectWithVacancy = vacanciesStagesObject;
      VacancyService.getVacancy(vacanciesStagesObject.vacancyId).then(value => {
         stagesObjectWithVacancy.vacancy = value;
         stagesObjectWithVacancy.stageFlow = value.stageFlow;
         deffered.resolve(stagesObjectWithVacancy);
      });
      return deffered.promise;
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
