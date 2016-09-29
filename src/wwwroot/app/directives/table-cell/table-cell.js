import {
   find
} from 'lodash';
import template from './table-cell.directive.html';
import './table-cell.scss';
export default class TableCellDirective {
   constructor() {
      this.restrict   = 'E';
      this.template   = template;
      this.scope      = {
         type: '@',
         report: '=',
         locationId: '='
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
   vm.reportFilteredByLocation = {};
   vm.filtered = [];
   console.log(vm);

   function filterByLocation() {
      console.log(vm.report.startDateReport);
      vm.filtered = find(vm.report.startDateReport.startDateReport, {locationId: vm.locationId});
      console.log(vm.filtered);
   }

   vm.$watch('report.startDateReport', function watcher() {
      if (vm.report.startDateReport.startDateReport) {
         filterByLocation();
      }
   });
}
