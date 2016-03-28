const OFF = 0;
const DEBUG_COEFFICIENT = 1;
const LOG_COEFFICIENT = 2;
const WARNING_COEFFICIENT = 3;
const ERROR_COEFFICIENT = 4;
var logLevel = DEBUG_COEFFICIENT;
let _$log;

export default class LoggerService {
   constructor($log) {
      'ngInject';
      _$log = $log;
   }

   debug(...args) {
      this._logger(_$log.debug, DEBUG_COEFFICIENT, arguments);
   }
   information() {
      this._logger(_$log.log, LOG_COEFFICIENT, arguments);
   }
   warning() {
      this._logger(_$log.warn, WARNING_COEFFICIENT, arguments);
   }
   error() {
      this._logger(_$log.error, ERROR_COEFFICIENT, arguments);
   }
   _logger(method, coefficient, arg) {
      if (logLevel <= coefficient) {
         method.apply(_$log, arg);
      }
   }
}
