const LIST_OF_THESAURUS = ['stage', 'eventtype'];
const STATE_FOR_REMOVE = 1;
import {
   set,
   cloneDeep,
   remove,
   clone,
   find,
   forEach,
   map,
   filter,
   each
} from 'lodash';

export default function CandidateProfileController( // eslint-disable-line max-statements, max-params
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
   UserService,
   SearchService,
   UserHistoryService,
   TransitionsService
   ) {
   'ngInject';

   const vm                  = $scope;
   vm.uploader               = _createNewUploader();
   vm.addFilesForRemove      = addFilesForRemove;
   vm.queueFileIdsForRemove  = [];
   vm.saveAndBack            = saveAndBack;
   vm.isChanged              = false;
   vm.candidate              = {};
   vm.editCandidate          = editCandidate;
   vm.back                   = back;
   vm.candidate.comments     = [];
   vm.candidate.files        = [];
   vm.comments               = cloneDeep(vm.candidate.comments);
   vm.saveComment            = saveComment;
   vm.removeComment          = removeComment;
   vm.candidateEvents         = [];
   vm.cloneCandidateEvents    = [];
   vm.saveEvent               = saveEvent;
   vm.removeEvent             = removeEvent;
   vm.vacancyStageInfosComposedByCandidateIdVacancyId = [];
   vm.isCandidateLoaded       = false;
   vm.currentUser             = UserService.getCurrentUser();

   (function _init() {
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then(topics => set(vm, 'thesaurus', topics));
      _initCurrentCandidate()
         .then(addVacanciesToCandidateIfNeeded)
         .then(recompose)
         .then(fillWithVacancies)
         .then(fillWithCandidates)
         .then((vacancyStagesObject) => {
            each(vacancyStagesObject, stageObject => {
               each(stageObject.vacancyStageInfos, vsi => {
                  if (!vsi.stage) {
                     vsi.stage = find(stageObject.vacancy.stageFlow, ['stage.id', vsi.stageId]);
                  }
               });
            });
            vm.vacancyStageInfosComposedByCandidateIdVacancyId = vacancyStagesObject;
            vm.$watch('vacancyStageInfosComposedByCandidateIdVacancyId', () => {
               vm.isChanged = true;
            }, true);
         })
         .then(() => {
            UserHistoryService.toReadableFormat(vm.candidate.history, vm).then((converted) => {
               vm.convertedHistory = converted;
               vm.isCandidateLoaded = true;
            });
         }).catch(LoggerService.error);

   }());

   function addVacanciesToCandidateIfNeeded() {
      if ($state.params.vacancies && $state.params.vacancies.length) {
         forEach($state.params.vacancies, (v) => {
            let newVSI = {
               vacancyId: v.id,
               candidateId: vm.candidate.id,
               comment: null,
               isPassed: false,
               stageState: 1,
               stage: find(v.stageFlow, { order: 1 }).stage,
               stageId: find(v.stageFlow, { order: 1 }).stage.id,
               createdOn: (new Date()).toISOString()
            };
            vm.candidate.vacanciesProgress.push(newVSI);
         });
      }
      return $q.when();
   }

   function recomposeBackAndSaveChangedVacancies(vacancyStageInfosComposedByCandidateIdVacancyId) {
      let newVacanciesProgress = [];
      forEach(vacancyStageInfosComposedByCandidateIdVacancyId, (stageObject) => {
         forEach(stageObject.vacancyStageInfos, (vsi) => {
            newVacanciesProgress.push(vsi);
         });
      });
      return newVacanciesProgress;
   }

   function _initCurrentCandidate() {
      return CandidateService.getCandidate($state.params.candidateId).then(candidate => {
         set(vm, 'candidate', candidate);
         vm.comments = cloneDeep(vm.candidate.comments);
         _getCandidateEvents(vm.candidate.id);
      });
   }

   function recompose() {
      let vacancyStageInfos = vm.candidate.vacanciesProgress;
      let vacancyStageInfosComposedByCandidateIdVacancyId = [];
      forEach(vacancyStageInfos, (vsi) => {
         let composedEntity = find(vacancyStageInfosComposedByCandidateIdVacancyId, { vacancyId: vsi.vacancyId });
         if (composedEntity) {
            composedEntity.vacancyStageInfos.push(vsi);
         } else {
            vacancyStageInfosComposedByCandidateIdVacancyId.push({
               candidateId: vsi.candidateId,
               vacancyId: vsi.vacancyId,
               vacancyStageInfos: [ vsi ]
            }
            );
         }
      });
      let composedWithCurrentStage = map(vacancyStageInfosComposedByCandidateIdVacancyId, (vacancyObject) => {
         let currentStageId = filter(vacancyObject.vacancyStageInfos, (vsi) => {
            return vsi.stageState === 1;
         })[0].stageId;
         return Object.assign(vacancyObject, {
            currentStageId
         });
      });
      return $q.when(composedWithCurrentStage);
   }

   function fillWithCandidates(recomposed) {
      return $q.all(map(recomposed, _loadCandidate));
   }

   function fillWithVacancies(recomposed) {
      return $q.all(map(recomposed, _loadVacancy));
   }

   function _loadCandidate(candidateStagesObject) {
      let stagesObjectWithCandidate = candidateStagesObject;
      if (candidateStagesObject.candidateId === vm.candidate.id) {
         stagesObjectWithCandidate.candidate = vm.candidate;
         return $q.when(stagesObjectWithCandidate);
      }
      return CandidateService.getCandidate(candidateStagesObject.candidateId).then(value => {
         stagesObjectWithCandidate.candidate = value;
         return stagesObjectWithCandidate;
      });
   }

   function _loadVacancy(vacanciesStagesObject) {
      let deffered = $q.defer();
      let stagesObjectWithVacancy = vacanciesStagesObject;
      VacancyService.getVacancy(vacanciesStagesObject.vacancyId, true).then(value => {
         stagesObjectWithVacancy.vacancy = value;
         stagesObjectWithVacancy.stageFlow = value.stageFlow;
         deffered.resolve(stagesObjectWithVacancy);
      });
      return deffered.promise;
   }

   vm.goToVacancies = () => {
      let vacanciesIds = map(vm.vacancyStageInfosComposedByCandidateIdVacancyId, 'vacancyId');
      _saveChanges();
      TransitionsService.go('vacancies.search', { vacanciesIds, candidateIdToGoBack: vm.candidate.id });
   };

   function _createNewUploader() {
      let newUploader = FileService.getFileUploader({ onCompleteAllCallBack : _saveChanges, maxSize : 2048000 });
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
      TransitionsService.go('candidate', {candidateId: vm.candidate.id});
   }

   function back() {
      TransitionsService.back();
   }

   function saveAndBack() {
      _saveChanges().then(back);
   }

   function _saveChanges() {
      if (vm.uploader.getNotUploadedItems().length) {
         return $q.when(vm.uploader.uploadAll());
      } else if (vm.queueFileIdsForRemove.length) {
         return FileService.removeGroup(vm.queueFileIdsForRemove).then(() => {
            vm.queueFileIdsForRemove = [];
            return  _candidateSave();
         });
      } else {
         return _candidateSave();
      }
   }

   function _candidateSave() {
      let memo = vm.candidate.comments;
      vm.candidate.comments = vm.comments;
      vm.candidate.vacanciesProgress = recomposeBackAndSaveChangedVacancies(
            vm.vacancyStageInfosComposedByCandidateIdVacancyId);
      return CandidateService.saveCandidate(vm.candidate).then(candidate => {
         vm.candidate = candidate;
         vm.comments = cloneDeep(vm.candidate.comments);
         UserDialogService.notification($translate.instant('DIALOG_SERVICE.SUCCESSFUL_CANDIDATE_SAVING'), 'success');
         vm.isChanged = false;
         vm.uploader.clearQueue();
      }).catch((error) => {
         vm.candidate.comments = memo;
         UserDialogService.notification($translate.instant('DIALOG_SERVICE.ERROR_CANDIDATE_SAVING'), 'error');
         LoggerService.error(error);
         SearchService.invalidateCandidates();
      });
   }

   function saveComment(comment) {
      vm.isChanged = true;
      if (comment.id) {
         set(find(vm.comments, comment), 'message', comment.message);
      } else {
         set(comment, 'authorId', UserService.getCurrentUser().id);
         vm.comments.push(comment);
      }
      return $q.when(comment);
   }

   function removeComment(comment) {
      vm.isChanged = true;
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
         vm.cloneCandidateEvents  = clone(vm.candidateEvents);
      });
   }
   function saveEvent(event) {
      return EventsService.save(event).then((responseEvent) => {
         _getCandidateEvents(vm.candidate.id);
         vm.cloneCandidateEvents  = clone(vm.candidateEvents);
         return responseEvent;
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
