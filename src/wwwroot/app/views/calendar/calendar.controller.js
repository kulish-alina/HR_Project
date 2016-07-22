import {
   set,
   isEqual,
   remove
} from 'lodash';
export default function CandidateProfileController(
   $scope,
   $translate,
   EventsService,
   UserService,
   UserDialogService,
   VacancyService,
   CandidateService,
   ThesaurusService
   ) {
   'ngInject';

   const vm                   = $scope;
   vm.vacancies               = [];
   vm.candidates              = [];
   vm.eventTypes              = [];
   vm.getEventsForMonth       = getEventsForMonth;
   vm.users                   = [];
   vm.addUserToChekedUsers    = addUserToChekedUsers;
   vm.chekedUsersIds          = [];
   vm.removeEvent             = removeEvent;
   vm.saveEvent               = saveEvent;
   vm.getEventsForDate        = getEventsForDate;
   vm.vacancy                 = {};
   vm.vacancy.current         = 0;
   vm.vacancy.size            = 20;
   vm.candidate               = {};
   vm.candidate.current       = 0;
   vm.candidate.size          = 20;

   (function init() {
      UserService.getUsers().then(users => set(vm, 'users', users));
      VacancyService.search(vm.vacancy).then(data  => set(vm, 'vacancies',  data.vacancies));
      CandidateService.search(vm.candidate).then(data  => set(vm, 'candidates', data.candidate));
      ThesaurusService.getThesaurusTopics('eventtype')
         .then(eventTypes  => set(vm, 'eventTypes', eventTypes));
   }());

   function getEventsForMonth(date, usersIds) {
      if (!isEqual(usersIds, vm.chekedUsersIds)) {
         usersIds = vm.chekedUsersIds;
      }
      vm.eventCondidtion.startDate = new Date(date.getFullYear(), date.getMonth(), 1);
      vm.eventCondidtion.endDate = null;
      vm.eventCondidtion.userIds = usersIds;
      return EventsService.getEventsForPeriod(vm.eventCondidtion);
   }

   function getEventsForDate(date) {
      vm.eventCondidtion.startDate = date;
      vm.eventCondidtion.endDate = vm.eventCondidtion.startDate;
      return EventsService.getEventsForPeriod(vm.eventCondidtion);
   }

   function addUserToChekedUsers(user) {
      if (user.selected) {
         vm.chekedUsersIds.push(user.id);
      } else {
         remove(vm.chekedUsersIds, (currentUserId) =>  currentUserId === user.id);
      }
      vm.$broadcast('checkUser', {chekedUsers: vm.chekedUsers});
   }

   function removeEvent(event) {
      return UserDialogService.confirm($translate.instant('DIALOG_SERVICE.EVENT_REMOVING_DIALOG')).then(() => {
         return EventsService.remove(event).then(() => {
            return UserDialogService.notification($translate.instant('DIALOG_SERVICE.SUCCESSFUL_REMOVING_EVENT'),
                                                  'success');
         });
      });
   }

   function saveEvent(event) {
      return EventsService.save(event).then((responseEvent) => {
         return responseEvent;
      });
   }
}
