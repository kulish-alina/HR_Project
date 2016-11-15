const LIST_OF_THESAURUS = ['industry', 'level', 'language',
    'department', 'tag', 'skill', 'typeOfEmployment', 'languageLevel', 'stage', 'currency'];
import _utils from './../../utils';
import {
   remove,
   set,
   each,
   find,
   cloneDeep,
   split,
   map
} from 'lodash';

const DEFAULT_REDIRECT_VIEW_NAME = 'vacancyView';

export default function VacancyController( //eslint-disable-line max-statements
   $scope,
   $translate,
   $state,
   $q,
   $window,
   VacancyService,
   ValidationService,
   ThesaurusService,
   UserService,
   UserDialogService,
   FileService,
   LoggerService,
   SearchService,
   TransitionsService) {
   'ngInject';

   const vm = $scope;

   /* --- api --- */
   vm.back                         = back;
   vm.saveVacancy                  = saveVacancy;
   vm.vacancy                      = {};
   vm.vacancy.comments             = [];
   vm.vacancy.files                = [];
   vm.thesaurus                    = [];
   vm.uploader                     = createNewUploader();
   vm.vacancy.requiredSkills       = [];
   vm.vacancy.tags                 = [];
   vm.addFilesForRemove            = addFilesForRemove;
   vm.queueFilesForRemove          = [];
   vm.isFilesUploaded              = false;
   vm.saveComment                  = _saveComment;
   vm.removeComment                = _removeComment;
   vm.editComment                  = _editComment;
   vm.comments                     = cloneDeep(vm.vacancy.comments);
   vm.removeChildVacancy           = removeChildVacancy;
   vm.searchResponsible            = UserService.autocomplete;
   vm.getFullName                  = UserService.getFullName;
   vm.utils                        = _utils;
   vm.getStateTitle                = _getStateTitle;
   vm.locationsSort                = _utils.locationsSort;

   /* === impl === */
   (function init() {
      if ($state.params.vacancyId) {
         _initCurrentVacancy($state.params.vacancyId);
      }
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS)
         .then(topics => set(vm, 'thesaurus', topics));

      ThesaurusService.getOfficeLocations()
         .then(locations => set(vm, 'locations', locations));
   }());

   function _initCurrentVacancy(id) {
      VacancyService.getVacancy(id).then(vacancy => {
         _setCurrentVacancy(vacancy);
      });
   }

   function _setCurrentVacancy (vacancy) {
      set(vm, 'vacancy', vacancy);
      vm.comments = cloneDeep(vm.vacancy.comments);
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

   function back() {
      TransitionsService.back();
   }

   function removeChildVacancy(vacancy) {
      UserDialogService.confirm($translate.instant('VACANCY.VACANCY_REMOVE_MESSAGE')).then(() => {
         VacancyService.remove(vacancy).then((responseVacancy) => {
            vm.vacancy = responseVacancy;
            UserDialogService.notification($translate.instant('DIALOG_SERVICE.SUCCESSFUL_REMOVING'), 'success');
         });
      });
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
   }

   function invertDate(date) {
      let splitedDate = split(date, '-');
      return `${splitedDate[2]}-${splitedDate[1]}-${splitedDate[0]}`;
   }

   function _saveComment(comment) {
      let currentUser = UserService.getCurrentUser();
      comment.authorId = currentUser.id;
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
      VacancyService.save(vm.vacancy)
         .then(vacancy => {
            vm.vacancy = vacancy;
            vm.comments = cloneDeep(vm.vacancy.comments);
            UserDialogService.notification($translate.instant('DIALOG_SERVICE.SUCCESSFUL_SAVING'), 'success');
            SearchService.invalidateVacancies();
            TransitionsService.back(DEFAULT_REDIRECT_VIEW_NAME, {vacancyId : vacancy.id});
         })
         .catch((error) => {
            vm.vacancy.comments = memo;
            UserDialogService.notification($translate.instant('DIALOG_SERVICE.ERROR_SAVING'), 'error');
            LoggerService.error(error);
         });
   }

   function _getStateTitle(key) {
      return $translate.instant(key);
   }
}
