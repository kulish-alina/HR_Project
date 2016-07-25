const LIST_OF_THESAURUS = ['stage', 'industry'];
import {
   remove,
   set,
   each,
   cloneDeep,
   find,
   pick,
   some,
   every,
   forEach,
   map,
   isEqual,
   maxBy
} from 'lodash';

import {
   isMatch
} from 'lodash/fp';


const MATCH_FIELDS = ['responsibleId', 'startDate', 'endDate', 'deadlineDate'];

export default function VacancyProfileController( // eslint-disable-line max-params, max-statements
   $scope,
   $q,
   $state,
   $translate,
   $element,
   $timeout,
   ThesaurusService,
   UserService,
   VacancyService,
   UserDialogService,
   FileService,
   LoggerService,
   CandidateService
   ) {
   'ngInject';

   const vm                = $scope;
   vm.thesaurus            = [];
   vm.responsibles         = [];
   vm.edit                 = edit;
   vm.uploader             = createNewUploader();
   vm.addFilesForRemove    = addFilesForRemove;
   vm.queueFilesForRemove  = [];
   vm.saveChanges          = saveChanges;
   vm.vacancy.comments     = $state.params._data ? $state.params._data.comments : [];
   vm.comments             = cloneDeep(vm.vacancy.comments);
   vm.isChanged            = isChanged;
   vm.selectStage          = selectStage;
   vm.currentStage         = '';
   vm.saveComment          = _saveComment;
   vm.removeComment        = _removeComment;
   vm.editComment          = _editComment;
   vm.goToParentVacancy    = goToParentVacancy;
   vm.goToChildVacancy     = goToChildVacancy;
   vm.clonedComposedBy     = [];
   vm.composedBy           = [];

   (function _init() {
      _initCurrentVacancy().then(() => {
         addCandidatesToVacancyIfNeeded().then(() => {
            return _recompose(vm.vacancy.candidatesProgress);
         }).then((recomposed) => {
            return _fillWithCandidates(recomposed);
         }).then((recomposedAndFilledWithCandidates) => {
            return _fillWithVacancies(recomposedAndFilledWithCandidates);
         }).then((vacancyStagesObject) => {
            vm.composedBy = vacancyStagesObject;
            vm.clonedComposedBy = cloneDeep(vm.composedBy);
         }).catch(LoggerService.error);
      });
      UserService.getUsers().then(users => set(vm, 'responsibles', users));
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then(topics => set(vm, 'thesaurus', topics));
   }());

   function _initCurrentVacancy() {
      let deffered = $q.defer();
      if ($state.params._data) {
         vm.vacancy = $state.params._data;
         vm.vacancy.comments = $state.params._data.comments;
         vm.vacancy.files = $state.params._data.files;
         vm.clonedVacancy = cloneDeep(vm.vacancy);
         vm.comments = cloneDeep(vm.vacancy.comments);
         deffered.resolve();
      } else {
         VacancyService.getVacancy($state.params.vacancyId).then(vacancy => {
            set(vm, 'vacancy', vacancy);
            vm.clonedVacancy = cloneDeep(vm.vacancy);
            vm.comments = cloneDeep(vm.vacancy.comments);
            deffered.resolve();
         });
      }
      return deffered.promise;
   }

   function addCandidatesToVacancyIfNeeded() {
      let deffered = $q.defer();
      if ($state.params.candidatesIds && $state.params.candidatesIds.length) {
         forEach($state.params.candidatesIds, (cId) => {
            let newVSI = {
               vacancyId: vm.vacancy.id,
               candidateId: cId,
               comment: null,
               isPassed: false,
               stageId: _getVacancyFirstStage().id,
               createdOn: (new Date()).toISOString()
            };
            vm.vacancy.candidatesProgress.push(newVSI);
         });
      }
      deffered.resolve();
      return deffered.promise;
   }

   function _getVacancyFirstStage() {
      return find(vm.vacancy.stageFlow, { order: 1 });
   };

   function _recompose(vacancyStageInfos) {
      let deffered = $q.defer();
      let composedBy = [];
      vm.parentEntity = 'vacancy';
      forEach(vacancyStageInfos, (vsi) => {
         let composedEntity = find(composedBy, { candidateId: vsi.candidateId });
         if (composedEntity) {
            composedEntity.stages.push(vsi);
         } else {
            composedBy.push({
               candidateId: vsi.candidateId,
               vacancyId: vsi.vacancyId,
               stages: [ vsi ]
            });
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


   function _fillWithCandidates(recomposed) {
      let deffered = $q.defer();
      deffered.resolve(map(recomposed, _loadCandidates));
      return deffered.promise;
   }

   function _fillWithVacancies(recomposed) {
      let deffered = $q.defer();
      if (vm.getvacancy) {
         deffered.resolve(map(recomposed, _loadVacancies));
      } else {
         deffered.resolve(recomposed);
      }
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

   vm.goToCandidates = () => {
      saveChanges();
      $state.go('candidates', { _data: null, vacancyIdToGoBack: vm.vacancy.id });
   };

   vm.goToCandidate = () => {
      saveChanges();
      $state.go('candidate', { _data: null, vacancyIdToGoBack: vm.vacancy.id });
   };

   function goToParentVacancy() {
      $state.go('vacancyEdit', {_data: null, vacancyId: vm.vacancy.parentVacancyId});
   }

   function goToChildVacancy(vacancy) {
      $state.go('vacancyEdit', {_data: null, vacancyId: vacancy.id});
   }

   function edit() {
      $state.go('vacancyEdit', {_data: vm.vacancy, vacancyId: vm.vacancy.id});
   }

   function isChanged() {
      if (!vm.vacancy) {
         return false;
      }
      let res = false;
      res = res || vm.uploader.queue.length !== 0;
      res = res || !isMatch(pick(vm.clonedVacancy, MATCH_FIELDS), vm.vacancy);
      res = res || !_isEqualComents();
      res = res || !isEqual(vm.clonedComposedBy, vm.composedBy);
      return res;
   }

   function _isEqualComents() {
      if (vm.comments.length !== vm.vacancy.comments.length || vm.queueFilesForRemove.length) {
         return false;
      }
      let fields = ['createdOn', 'id', 'message', 'state'];
      return every(vm.comments, (comment) => {
         comment = pick(comment, fields);
         return some(vm.vacancy.comments, isMatch(comment));
      });
   }

   function createNewUploader() {
      let newUploader = FileService.getFileUploader({ onCompleteAllCallBack : saveChanges, maxSize : 2048000 });
      newUploader.onSuccessItem = function onSuccessUpload(item) {
         let response = JSON.parse(item._xhr.response);
         vm.vacancy.files.push(response);
      };
      newUploader.onWhenAddingFileFailed = function onAddingFileFailed() {
         UserDialogService.notification($translate.instant('COMMON.FILE_UPLOADER_ERROR_MESSAGE'), 'warning');
      };
      newUploader.onAfterAddingAll = function onAfterAddingAl() {
      };
      return newUploader;
   }

   function addFilesForRemove(file) {
      vm.queueFilesForRemove.push(file);
      remove(vm.vacancy.files, {id: file.id});
   }

   function saveChanges() {
      if (vm.uploader.getNotUploadedItems().length) {
         vm.uploader.uploadAll();
      } else if (vm.queueFilesForRemove) {
         each(vm.queueFilesForRemove, (file) => FileService.remove(file));
         vm.queueFilesForRemove = [];
         _vs();
      } else {
         _vs();
      }
   }

   function selectStage(stageName) {
      vm.currentStage = stageName;
   }

   function _saveComment(comment) {
      return $q.when(vm.comments.push(comment));
   }

   function _removeComment(comment) {
      let commentForRemove = find(vm.comments, comment);
      if (comment.id) {
         commentForRemove.state = 1;
         remove(vm.comments, comment);
         return $q.when(vm.comments.push(commentForRemove));
      } else {
         return $q.when(remove(vm.comments, comment));
      }
   }

   function _editComment(comment) {
      return $q.when(remove(vm.comments, comment));
   }

   function _recomposeBack(composedBy) {
      let newCandidatesProgress = [];
      forEach(composedBy, (stageObject) => {
         forEach(stageObject.stages, (vsi) => {
            newCandidatesProgress.push(vsi);
         });
      });
      return newCandidatesProgress;
   }

   function _vs() {
      let memo = vm.vacancy.comments;
      vm.vacancy.comments = vm.comments;
      vm.vacancy.candidatesProgress = _recomposeBack(vm.composedBy);
      VacancyService.save(vm.vacancy).then(vacancy => {
         vm.vacancy = vacancy;
         vm.comments = cloneDeep(vm.vacancy.comments);
         vm.clonedVacancy = cloneDeep(vm.vacancy);
         vm.uploader.clearQueue();
         UserDialogService.notification($translate.instant('DIALOG_SERVICE.SUCCESSFUL_SAVING'), 'success');
      }).catch((error) => {
         vm.vacancy.comments = memo;
         UserDialogService.notification($translate.instant('DIALOG_SERVICE.ERROR_SAVING'), 'error');
         LoggerService.error(error);
      });
   }
}
