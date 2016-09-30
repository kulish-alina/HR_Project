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
   'vacanciesClosedInCanceledPeriodCount',
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
         vacancyState: '@'
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

   vm.$watch('report', function obs() {
      groupByStages();
   });

   function groupByStages() {
      each(REPORT_VACANCY_STATES, (stat) => {
         set(vm.locationResult, stat, sumBy(vm.report, stat));
      });
   }
}
