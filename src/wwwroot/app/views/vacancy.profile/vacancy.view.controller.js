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
   filter
} from 'lodash';

import {
   isMatch
} from 'lodash/fp';

import './vacancy.view.scss';

let moment = require('moment');

const MATCH_FIELDS = ['responsibleId', 'startDate', 'endDate', 'deadlineDate'];
const STATE_FOR_REMOVE = 1;
const ENTITY_STATES = {
   Inactive     : 1,
   Active       : 2,
   Verfied      : 3,
   Unverified   : 4,
   Pending      : 5,
   Open         : 6,
   Processing   : 7,
   Closed       : 8,
   Cancelled    : 9
};

export default function VacancyProfileController( // eslint-disable-line max-params, max-statements
   $scope,
   $q,
   $state,
   $translate,
   $element,
   $timeout,
   $window,
   ThesaurusService,
   UserService,
   VacancyService,
   UserDialogService,
   FileService,
   LoggerService,
   CandidateService,
   UserHistoryService,
   TransitionsService
   ) {
   'ngInject';

   const vm                = $scope;
   vm.thesaurus            = [];
   vm.responsibles         = [];
   vm.vacancy              = {};
   vm.edit                 = edit;
   vm.back                 = back;
   vm.uploader             = createNewUploader();
   vm.addFilesForRemove    = addFilesForRemove;
   vm.queueFilesForRemove  = [];
   vm.saveAndBack          = saveAndBack;
   vm.isChanged            = isChanged;
   vm.currentStage         = '';
   vm.saveComment          = saveComment;
   vm.removeComment        = removeComment;

   vm.clonedVacancyStageInfosComposedByCandidateIdVacancyId     = [];
   vm.vacancyStageInfosComposedByCandidateIdVacancyId           = [];
   vm.isVacancyLoaded      = false;
   vm.currentUser          = UserService.getCurrentUser();
   vm.searchResponsible    = _searchResponsible;

   (function _init() {
      _initCurrentVacancy()
        .then(addCandidatesToVacancyIfNeeded)
        .then(recompose)
        .then(fillWithCandidates)
        .then(fillWithVacancies)
        .then((vacancyStagesObject) => {
           vm.vacancyStageInfosComposedByCandidateIdVacancyId = vacancyStagesObject;
           vm.vacancyStageInfosComposedByCandidateIdVacancyId = cloneDeep(
             vm.vacancyStageInfosComposedByCandidateIdVacancyId);
        })
        .then(() => {
           UserHistoryService.toReadableFormat(vm.vacancy.history, vm).then((converted) => {
              vm.convertedHistory = converted;
              vm.isVacancyLoaded = true;
           });
        })
        .catch(LoggerService.error);
      UserService.getUsers().then(users => set(vm, 'responsibles', users));
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then(topics => set(vm, 'thesaurus', topics));
   }());

   vm.closeVacancyWith = (candidate) => {
      vm.vacancy.closingCandidateId = candidate.id;
      vm.vacancy.closingCandidate = candidate;
      vm.vacancy.state = ENTITY_STATES.Closed;
   };

   function _initCurrentVacancy() {
      return VacancyService.getVacancy($state.params.vacancyId).then(vacancy => {
         set(vm, 'vacancy', vacancy);
         vm.clonedVacancy = cloneDeep(vm.vacancy);
         vm.comments = cloneDeep(vm.vacancy.comments)  || [];
      });
   }

   vm.getPassDate = () => {
      if (vm.isVacancyLoaded) {
         let candidatesProgress = _recomposeBack(
           vm.vacancyStageInfosComposedByCandidateIdVacancyId) || vm.vacancy.candidatesProgress;
         let hireStage = filter(vm.vacancy.stageFlow, (extStage) => {
            return extStage.stage.title === 'Hired';
         })[0];
         return moment(find(candidatesProgress, {stageId: hireStage.stage.id}).createdOn).format('DD-MM-YYYY');
      }
   };

   function addCandidatesToVacancyIfNeeded() {
      if ($state.params.candidatesIds && $state.params.candidatesIds.length) {
         forEach($state.params.candidatesIds, (cId) => {
            let newVSI = {
               vacancyId: vm.vacancy.id,
               candidateId: cId,
               comment: null,
               stageState: 1,
               stageId: _getVacancyFirstStage().stage.id,
               createdOn: (new Date()).toISOString()
            };
            vm.vacancy.candidatesProgress.push(newVSI);
         });
      }
      return $q.when();
   }

   function _getVacancyFirstStage() {
      return find(vm.vacancy.stageFlow, { order: 1 });
   };

   vm.getHiredCandidateFullName = () => {
      if (vm.isVacancyLoaded) {
         let space = ' ';
         let fullName = vm.vacancy.closingCandidate.firstName;
         fullName = fullName + space;
         fullName = fullName + vm.vacancy.closingCandidate.lastName;
         return fullName;
      }
   };

   function recompose() {
      let vacancyStageInfos = vm.vacancy.candidatesProgress;
      let vacancyStageInfosComposedByCandidateIdVacancyId = [];
      vm.parentEntity = 'vacancy';
      forEach(vacancyStageInfos, (vsi) => {
         if (!vsi.stage) {
            vsi.stage = find(vm.vacancy.stageFlow, ['stage.id', vsi.stageId]);
         }
         let composedEntity = find(vacancyStageInfosComposedByCandidateIdVacancyId, { candidateId: vsi.candidateId });
         if (composedEntity) {
            composedEntity.vacancyStageInfos.push(vsi);
         } else {
            vacancyStageInfosComposedByCandidateIdVacancyId.push({
               candidateId: vsi.candidateId,
               vacancyId: vsi.vacancyId,
               vacancyStageInfos: [ vsi ]
            });
         }
      });
      let composedWithCurrentStage = map(vacancyStageInfosComposedByCandidateIdVacancyId, (candObject) => {
         let currentStageId = find(candObject.vacancyStageInfos, ['stageState', 1]).stageId;
         return Object.assign(candObject, {
            currentStageId
         });
      });
      return $q.when(composedWithCurrentStage);
   }

   vm.isVacancyClosed = () => {
      return !!vm.vacancy.closingCandidateId;
   };

   vm.goToHiredCandidate = () => {
      TransitionsService.go('candidateProfile', { candidateId: vm.vacancy.closingCandidateId });
   };

   function fillWithCandidates(recomposed) {
      return $q.all(map(recomposed, _loadCandidate));
   }

   function fillWithVacancies(recomposed) {
      return $q.all(map(recomposed, _loadVacancy));
   }

   function _loadCandidate(candidateStagesObject) {
      let stagesObjectWithCandidate = candidateStagesObject;
      return CandidateService.getCandidate(candidateStagesObject.candidateId).then(value => {
         stagesObjectWithCandidate.candidate = value;
         return stagesObjectWithCandidate;
      });
   }

   function _loadVacancy(vacanciesStagesObject) {
      let stagesObjectWithVacancy = vacanciesStagesObject;
      if (vm.vacancy.id === vacanciesStagesObject.vacancyId) {
         stagesObjectWithVacancy.vacancy = vm.vacancy;
         stagesObjectWithVacancy.stageFlow = vm.vacancy.stageFlow;
         return $q.when(stagesObjectWithVacancy);
      } else {
         return VacancyService.getVacancy(vacanciesStagesObject.vacancyId).then(value => {
            stagesObjectWithVacancy.vacancy = value;
            stagesObjectWithVacancy.stageFlow = value.stageFlow;
            return stagesObjectWithVacancy;
         });
      }
   }
   vm.goToCandidates = () => {
      let candidateIds = map(vm.vacancyStageInfosComposedByCandidateIdVacancyId, 'candidateId');
      _saveChanges()
         .then(() => TransitionsService.go('candidates.search', { candidateIds, vacancyIdToGoBack: vm.vacancy.id }));
   };

   vm.goToCandidate = () => {
      _saveChanges()
         .then(() => TransitionsService.go('candidate', { vacancyIdToGoBack: vm.vacancy.id }));
   };

   function edit() {
      TransitionsService.go('vacancyEdit', { vacancyId: vm.vacancy.id});
   }

   function isChanged() {
      if (!vm.vacancy) {
         return false;
      }
      let res = false;
      res = res || vm.uploader.queue.length !== 0;
      res = res || !isMatch(pick(vm.clonedVacancy, MATCH_FIELDS), vm.vacancy);
      res = res || !_isEqualComents();
      res = res || !isEqual(vm.clonedVacancyStageInfosComposedByCandidateIdVacancyId,
        vm.vacancyStageInfosComposedByCandidateIdVacancyId);
      return res;
   }

   function _isEqualComents() {
      if (vm.comments && vm.vacancy.comments) {
         if (vm.comments.length !== vm.vacancy.comments.length || vm.queueFilesForRemove.length) {
            return false;
         }
      }
      let fields = ['createdOn', 'id', 'message', 'state'];
      return every(vm.comments, (comment) => {
         comment = pick(comment, fields);
         return some(vm.vacancy.comments, isMatch(comment));
      });
   }

   function createNewUploader() {
      let newUploader = FileService.getFileUploader({ onCompleteAllCallBack : _saveChanges, maxSize : 2048000 });
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

   function saveAndBack() {
      _saveChanges().then(back);
   }

   function _saveChanges() {
      if (vm.uploader.getNotUploadedItems().length) {
         return $q.when(vm.uploader.uploadAll());
      } else if (vm.queueFilesForRemove) {
         each(vm.queueFilesForRemove, (file) => FileService.remove(file));
         vm.queueFilesForRemove = [];
         return _vs();
      } else {
         return _vs();
      }
   }

   function _searchResponsible() {
      return UserService.getUsers();
   };

   function back() {
      TransitionsService.back();
   }

   function saveComment(comment) {
      if (comment.id) {
         set(find(vm.comments, comment), 'message', comment.message);
      } else {
         set(comment, 'authorId', UserService.getCurrentUser().id);
         vm.comments.push(comment);
      }
      return $q.when(comment);
   }

   function removeComment(comment) {
      if (comment.id) {
         set(find(vm.comments, comment), 'state', STATE_FOR_REMOVE);
         return $q.when(comment);
      } else {
         remove(vm.comments, comment);
         return $q.when(comment);
      }
   }

   function _recomposeBack(vacancyStageInfosComposedByCandidateIdVacancyId) {
      let newCandidatesProgress = [];
      forEach(vacancyStageInfosComposedByCandidateIdVacancyId, (stageObject) => {
         forEach(stageObject.vacancyStageInfos, (vsi) => {
            newCandidatesProgress.push(vsi);
         });
      });
      return newCandidatesProgress;
   }

   function _vs() {
      let memo = vm.vacancy.comments;
      vm.vacancy.comments = vm.comments;
      vm.vacancy.candidatesProgress = _recomposeBack(vm.vacancyStageInfosComposedByCandidateIdVacancyId);
      return VacancyService.save(vm.vacancy).then(vacancy => {
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
