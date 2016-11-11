import utils  from '../utils.js';
import {
   each
} from 'lodash';
const DATE_TYPE = ['startDate', 'endDate'];
const REPORT_URL = 'report';
let _HttpService, _ReportsService;

export default class ReportsService {
   constructor(HttpService) {
      'ngInject';
      _ReportsService = this;
      _HttpService   = HttpService;
   }

   getDataForUserReport(searchParameters) {
      searchParameters = _ReportsService._convertToServerDates(searchParameters);
      let userReportUrl = 'usersReport';
      return _HttpService.get(`${REPORT_URL}/${userReportUrl}`, searchParameters)
         .then(this._convertFromServerDates);
   }

   getDataForVacancyReport(searchParameters) {
      searchParameters = _ReportsService._convertToServerDates(searchParameters);
      let vacancyReportUrl = 'vacanciesReport';
      return _HttpService.get(`${REPORT_URL}/${vacancyReportUrl}`, searchParameters)
         .then(this._convertFromServerDates);
   }

   getDataForCandidatesReport(searchParameters) {
      searchParameters = _ReportsService._convertToServerDates(searchParameters);
      let candidatesReportUrl = 'candidateProgressReport';
      return _HttpService.get(`${REPORT_URL}/${candidatesReportUrl}`, searchParameters);
   }

   _convertToServerDates(searchParameters) {
      each(DATE_TYPE, (type) => {
         if (searchParameters[type]) {
            searchParameters[type] = utils.formatDateToServer(searchParameters[type]);
         };
      });
      return searchParameters;
   }

   _convertFromServerDates(report) {
      each(DATE_TYPE, (type) => {
         if (report[type]) {
            report[type] = utils.formatDateFromServer(report[type]);
         };
      });
      return report;
   }
}
