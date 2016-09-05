import utils  from '../utils.js';
import {
   each
} from 'lodash';
const DATE_TYPE = ['startDate', 'endDate'];
const REPORT_URL = 'report';
let _HttpService;

export default class ReportsService {
   constructor(HttpService) {
      'ngInject';
      _HttpService   = HttpService;
   }

   getDataForUserReport(searchParameters) {
      searchParameters = this._convertToServerDates(searchParameters);
      let userReportUrl = 'usersReport';
      return _HttpService.post(`${REPORT_URL}/${userReportUrl}`, searchParameters);
   }

   _convertToServerDates(searchParameters) {
      each(DATE_TYPE, (type) => {
         if (searchParameters[type]) {
            searchParameters[type] = utils.formatDateToServer(searchParameters[type]);
         };
      });
      return searchParameters;
   }
}
