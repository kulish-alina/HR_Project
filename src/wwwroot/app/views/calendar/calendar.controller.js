import './calendar.scss';
import {
   set,
   remove,
   each,
   assign,
   map,
   zipObject,
   flatten,
   round,
   isEqual
} from 'lodash';
let HUE_SATURATION_LIGHTNESS_LIMIT = 360;
let HUE_SATURATION_LIGHTNESS_OFFSET = 20;
export default function CandidateProfileController(
   $scope,
   $translate,
   EventsService,
   UserService,
   UserDialogService,
   VacancyService,
   CandidateService,
   ThesaurusService,
   LocalStorageService
   ) {
   'ngInject';

   const vm                   = $scope;
   vm.vacancies               = [];
   vm.candidates              = [];
   vm.eventTypes              = [];
   vm.getEventsForMonth       = getEventsForMonth;
   vm.users                   = [];
   vm.addUserToChekedUsers    = addUserToChekedUsers;
   vm.chekedUsersIds          = LocalStorageService.get('chekedUsersIds') || [];
   vm.removeEvent             = removeEvent;
   vm.saveEvent               = saveEvent;
   vm.getEventsForDate        = getEventsForDate;
   vm.vacancy                 = {};
   vm.vacancy.current         = 0;
   vm.vacancy.size            = 20;
   vm.candidate               = {};
   vm.candidate.current       = 0;
   vm.candidate.size          = 20;
   vm.userColorObject         = {};

   (function init() {
      UserService.getUsers().then(users => {
         set(vm, 'users', users);
         generateColorsForUsers();
         each(vm.users, (user) => {
            if (vm.chekedUsersIds.length && vm.chekedUsersIds.includes(user.id)) {
               assign(user, {selected: true});
            } else {
               assign(user, {selected: false});
            }
         });
      });
      VacancyService.search(vm.vacancy).then(data  => set(vm, 'vacancies',  data.vacancies));
      CandidateService.search(vm.candidate).then(data  => set(vm, 'candidates', data.candidate));
      ThesaurusService.getThesaurusTopics('eventtype')
         .then(eventTypes  => set(vm, 'eventTypes', eventTypes));
   }());

   function generateColorsForUsers() {
      let userIdsArr = map(vm.responsibles, (user) => {
         return user.id;
      });
      let count = 0;
      let colorIndexArray = [];
      while (count <= HUE_SATURATION_LIGHTNESS_LIMIT) {
         colorIndexArray.push(count);
         count += HUE_SATURATION_LIGHTNESS_OFFSET;
      }
      let userColors = map(colorIndexArray, (num) => {
         let pastel =  `hsl(${num}, 100%, 95%)`;
         return pastel;
      });
      let fulluserColors = flatten(Array(round(userIdsArr.length / userColors.length) + 1).fill(userColors));
      vm.userColorObject = zipObject(userIdsArr, fulluserColors);
   }

   function getEventsForMonth(startDate, endDate, usersIds) {
      vm.eventCondidtion.startDate = startDate;
      vm.eventCondidtion.endDate = endDate;
      if (usersIds.length !== 1 && !isEqual(usersIds, vm.chekedUsersIds)) {
         usersIds = vm.chekedUsersIds;
      }
      vm.eventCondidtion.userIds = usersIds;
      return EventsService.getEventsForPeriod(vm.eventCondidtion);
   }

   function getEventsForDate(date) {
      vm.eventCondidtion.startDate = date;
      vm.eventCondidtion.endDate = vm.eventCondidtion.startDate;
      return EventsService.getEventsForPeriod(vm.eventCondidtion);
   }

   function addUserToChekedUsers(user) {
      if (user.selected && !(vm.chekedUsersIds.includes(user.id))) {
         vm.chekedUsersIds.push(user.id);
      } else if (!user.selected) {
         remove(vm.chekedUsersIds, (currentUserId) =>  currentUserId === user.id);
      }
      vm.$broadcast('checkUser', {checkedUsersIds: vm.chekedUsersIds});
      LocalStorageService.set('chekedUsersIds', vm.chekedUsersIds);
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
