const LIST_OF_THESAURUS = ['industry', 'level', 'city', 'language',
    'department', 'tag', 'skill', 'typeOfEmployment', 'languageLevel', 'stage'];
import {
   remove,
   set,
   each,
   find,
   cloneDeep,
   split,
   map
} from 'lodash';

export default function VacancyController(
   $scope,
   $translate,
   $state,
   $q,
   VacancyService,
   ValidationService,
   ThesaurusService,
   UserService,
   UserDialogService,
   FileService,
   LoggerService
) {
   'ngInject';

   const vm = $scope;

   /* --- api --- */
   vm.clear                        = clear;
   vm.saveVacancy                  = saveVacancy;
   vm.vacancy                      = {};
   vm.vacancy.comments             = $state.params._data ? $state.params._data.comments : [];
   vm.vacancy.files                = $state.params._data ? $state.params._data.files : [];
   vm.thesaurus                    = [];
   vm.responsibles                 = [];
   vm.uploader                     = createNewUploader();
   vm.vacancy.requiredSkills       = vm.vacancy.requiredSkills || [];
   vm.vacancy.tags                 = vm.vacancy.tags || [];
   vm.addFilesForRemove            = addFilesForRemove;
   vm.queueFilesForRemove          = [];
   vm.isFilesUploaded              = false;
   vm.saveComment                  = _saveComment;
   vm.removeComment                = _removeComment;
   vm.editComment                  = _editComment;
   vm.comments                     = cloneDeep(vm.vacancy.comments);
   vm.goToChildVacancy             = goToChildVacancy;
   vm.goToParentVacancy            = goToParentVacancy;
   vm.removeChildVacancy           = removeChildVacancy;

   /* === impl === */

   (function init() {
      _initCurrentVacancy();
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then(topics => set(vm, 'thesaurus', topics));
      UserService.getUsers().then(users => set(vm, 'responsibles', users));
   }());

   function _initCurrentVacancy() {
      if ($state.params._data) {
         vm.vacancy = $state.params._data;
      } else if ($state.params.vacancyId) {
         VacancyService.getVacancy($state.params.vacancyId).then(vacancy => {
            set(vm, 'vacancy', vacancy);
            vm.comments = cloneDeep(vm.vacancy.comments);
         });
      } else {
         vm.vacancy = {};
         vm.vacancy.comments = [];
         vm.vacancy.files = [];
      }
   }

   function createNewUploader() {
      let newUploader = FileService.getFileUploader({ onCompleteAllCallBack : _vs, maxSize : 2048000 });
      newUploader.onSuccessItem = function onSuccessUpload(item) {
         let response = JSON.parse(item._xhr.response);
         vm.vacancy.files.push(response);
         vm.isFilesUploaded = true;
      };
      newUploader.onWhenAddingFileFailed = function onAddingFileFailed() {
         UserDialogService.notification($translate.instant('COMMON.FILE_UPLOADER_ERROR_MESSAGE'), 'warning');
      };
      return newUploader;
   }

   function addFilesForRemove(file) {
      vm.queueFilesForRemove.push(file);
      remove(vm.vacancy.files, {id: file.id});
      vm.isChanged = true;
   }

   function clear() {
      $state.go('vacancyEdit', {_data: null, vacancyId: null});
   }

   function goToChildVacancy(vacancy) {
      $state.go('vacancyView', {_data: null, vacancyId: vacancy.id});
   }

   function removeChildVacancy(vacancy) {
      UserDialogService.confirm($translate.instant('VACANCY.VACANCY_REMOVE_MESSAGE')).then(() => {
         VacancyService.remove(vacancy).then((responseVacancy) => {
            vm.vacancy = responseVacancy;
            UserDialogService.notification($translate.instant('DIALOG_SERVICE.SUCCESSFUL_REMOVING'), 'success');
         });
      });
   }

   function goToParentVacancy() {
      $state.go('vacancyEdit', {_data: null, vacancyId: vm.vacancy.parentVacancyId});
   }

   function saveVacancy(ev, form) {
      ev.preventDefault();
      //TODO: remove this terrible method and use moment.js
      let dates = [vm.vacancy.startDate, vm.vacancy.deadlineDate, vm.vacancy.endDate];
      let convertedDates = map(dates, invertDate);
      let starDate = Date.parse(convertedDates[0]);
      let deadlineDate = Date.parse(convertedDates[1]);
      let endDate = Date.parse(convertedDates[2]);
      if (starDate > deadlineDate || starDate > endDate || deadlineDate > endDate) {
         UserDialogService.notification($translate.instant('DIALOG_SERVICE.INVALID_DATES'), 'error');
         return false;
      }
      ValidationService.validate(form).then(() => {
         if (vm.uploader.getNotUploadedItems().length) {
            vm.uploader.uploadAll();
         } else if (vm.queueFilesForRemove) {
            each(vm.queueFilesForRemove, (file) => FileService.remove(file));
            vm.queueFilesForRemove = [];
            _vs();
         } else {
            _vs();
         }
      });
      return false;
   }

   function invertDate(date) {
      let splitedDate = split(date, '-');
      return `${splitedDate[2]}-${splitedDate[1]}-${splitedDate[0]}`;
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

   function _vs() {
      let memo = vm.vacancy.comments;
      vm.vacancy.comments = vm.comments;
      VacancyService.save(vm.vacancy).then(vacancy => {
         vm.vacancy = vacancy;
         vm.comments = cloneDeep(vm.vacancy.comments);
         UserDialogService.notification($translate.instant('DIALOG_SERVICE.SUCCESSFUL_SAVING'), 'success');
      }).catch((error) => {
         vm.vacancy.comments = memo;
         UserDialogService.notification($translate.instant('DIALOG_SERVICE.ERROR_SAVING'), 'error');
         LoggerService.error(error);
      });
   }
}
