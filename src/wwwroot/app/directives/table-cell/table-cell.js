import {
   each,
   sumBy,
   set
} from 'lodash';

const REPORT_VACANCY_STATES = [
   'vacanciesPendingInCurrentPeriodCount',
   'vacanciesOpenedInCurrentPeriodCount',
   'vacanciesInProgressInCurrentPeriodCount',
   'vacanciesClosedInCurrentPeriodCount',
   'vacanciesCanceledInCurrentPeriodCount',
   'pendingVacanciesCount',
   'openVacanciesCount',
   'inProgressVacanciesCount'
];

import template from './table-cell.directive.html';
import './table-cell.scss';

export default class TableCellDirective {
   constructor() {
      this.restrict   = 'E';
      this.template   = template;
      this.scope      = {
         type: '@',
         report: '=',
         vacancyState: '@',
         users: '='
      };
      this.controller = TableCellController;
   }
   static createInstance() {
      'ngInject';
      TableCellDirective.instance = new TableCellDirective();
      return TableCellDirective.instance;
   }
}

function TableCellController($scope) {
   'ngInject';
   const vm               = $scope;
   vm.locationResult      = {};
   vm.userResult          = {};

   vm.$watch('report', function obs() {
      groupByStages();
   });

   function groupByStages() {
      if (vm.type === 'only-locations' || vm.type === 'locations-users') {
         each(REPORT_VACANCY_STATES, (stat) => {
            set(vm.locationResult, stat, sumBy(vm.report, stat));
         });
      } else if (vm.type === 'only-users') {
         each(REPORT_VACANCY_STATES, (stat) => {
            set(vm.userResult, stat, sumBy(vm.report, stat));
         });
      }

   }
}
