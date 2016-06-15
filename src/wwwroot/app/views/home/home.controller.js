const LIST_OF_THESAURUS = ['industry', 'level', 'location',
    'typeOfEmployment'];
import {
   set,
   cloneDeep,
   remove
} from 'lodash';

export default function HomeController(
   $scope,
   $q,
   $state,
   $translate,
   VacancyService,
   ThesaurusService,
   UserService,
   LoggerService,
   UserDialogService,
   NoteService
   ) {
   'ngInject';

   const vm           = $scope;
   vm.thesaurus       = [];
   vm.responsibles    = [];
   vm.vacancy         = {};
   vm.vacancies       = [];
   vm.viewVacancy     = viewVacancy;
   vm.total           = 0;
   vm.vacancy.current = 0;
   vm.vacancy.size    = 20;
   vm.pagination      = { current: 0 };
   vm.pageChanged     = pageChanged;
   vm.userNotes       = [];
   vm.notes           = cloneDeep(vm.userNotes);
   vm.saveNote        = saveNote;
   vm.removeNote      = removeNote;
   vm.editNote        = editNote;

   function pageChanged(newPage) {
      vm.vacancy.current = newPage;
      VacancyService.search(vm.vacancy).then(response => {
         vm.total = response.total;
         vm.vacancies = response.vacancies;
      }).catch(_onError);
   };

   function _init() {
      UserService.getUsers().then(users => set(vm, 'responsibles', users));
      ThesaurusService.getThesaurusTopicsGroup(LIST_OF_THESAURUS).then(topics => set(vm, 'thesaurus', topics));
      NoteService.getNotesByUser().then((notes) => {
         vm.userNotes = notes;
         vm.notes  = cloneDeep(vm.userNotes);
      });
      VacancyService.search(vm.vacancy).then(response => {
         vm.total = response.total;
         vm.vacancies = response.vacancies;
      }).catch(_onError);
   }
   _init();

   function viewVacancy(vacancy) {
      $state.go('vacancyView', {_data: vacancy, vacancyId: vacancy.id});
   }

   function saveNote(note) {
      return NoteService.save(note).then((res) => {
         vm.userNotes.push(res);
         return vm.notes  = cloneDeep(vm.userNotes);
      });
   }

   function removeNote(note) {
      return NoteService.remove(note).then(() => {
         remove(vm.userNotes, {id: note.id});
         return vm.notes  = cloneDeep(vm.userNotes);
      });
   }

   function editNote(note) {
      return $q.when(remove(vm.userNotes, {id: note.id}));
   }

   function _onError(error) {
      UserDialogService.notification($translate.instant('DIALOG_SERVICE.ERROR_VACANCIES_SEARCH'), 'error');
      LoggerService.error(error);
   }
}
