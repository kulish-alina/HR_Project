import {
   set
} from 'lodash';

export default function RecruitingFunnelController(
   $scope,
   $state,
   ThesaurusService
) {
   'ngInject';

   const vm                   = $scope;
   vm.viewVacancy             = viewVacancy;

   (function init() {
      ThesaurusService.getThesaurusTopics('stage').then(topic => set(vm, 'stages', topic));
   }());

   function viewVacancy() {
      $state.go('vacancyView', {_data: vm.selectedVacancy, vacancyId: vm.selectedVacancy.id});
   }
}
