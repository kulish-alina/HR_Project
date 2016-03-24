const OFF = 0;
const DEBUG_COEFFICIENT = 1;
const LOG_COEFFICIENT = 2;
const WARNING_COEFFICIENT = 3;
const ERROR_COEFFICIENT = 4;
var logLevel = WARNING_COEFFICIENT;

export default class LoggerService {
    constructor($log) {
        'ngInject';
        this.log = $log;
    }

    debug() {
        this.logger(this.log.debug, DEBUG_COEFFICIENT, arguments);
    }

    logg() {
        this.logger(this.log.log, LOG_COEFFICIENT, arguments);
    }

    warning() {
        this.logger(this.log.warn, WARNING_COEFFICIENT, arguments);
    }

    error() {
        this.logger(this.log.error, ERROR_COEFFICIENT, arguments);
    }

    logger(method, coefficient, arg) {
        if (logLevel <= coefficient) {
            method.apply(this.log, arg);
        }
    }
}
