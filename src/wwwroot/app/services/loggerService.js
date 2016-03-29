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
      this._logger(_$log.debug, DEBUG_COEFFICIENT, args);
   }

   information(...args) {
      this._logger(_$log.log, LOG_COEFFICIENT, args);
   }

   warning(...args) {
      this._logger(_$log.warn, WARNING_COEFFICIENT, args);
   }

   error(...args) {
      this._logger(_$log.error, ERROR_COEFFICIENT, args);
   }

   _logger(method, coefficient, args) {
      if (logLevel <= coefficient) {
         args.unshift(new Date());
         method.apply(_$log, args);
      }
   }
}
