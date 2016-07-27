import { find, gt } from 'lodash';

let currentLevel;
let _$log;

const offLevel   = 0;
const errorLevel = 1;
const warnLevel  = 2;
const logLevel   = 3;
const debugLevel = 4;

const OFF_LEVEL      = new Level(offLevel, 'OFF');
const ERROR_LEVEL    = new Level(errorLevel, 'ERROR');
const WARNING_LEVEL  = new Level(warnLevel, 'WARN');
const LOG_LEVEL      = new Level(logLevel, 'LOG');
const DEBUG_LEVEL    = new Level(debugLevel, 'DEBUG');

const LEVELS = [OFF_LEVEL, DEBUG_LEVEL, LOG_LEVEL, WARNING_LEVEL, ERROR_LEVEL];

export default class LoggerProvider {
   constructor() {
      this.changeLogLevel('LOG');
   }

   changeLogLevel(level) {
      currentLevel = find(LEVELS, {name: level}) || LOG_LEVEL;
   }

   $get($log) {
      'ngInject';
      _$log = $log;
      return new LoggerService();
   }
}

class LoggerService {
   debug(...args) {
      _logger(_$log.debug, DEBUG_LEVEL, args);
   }

   log(...args) {
      _logger(_$log.log, LOG_LEVEL, args);
   }

   warn(...args) {
      _logger(_$log.warn, WARNING_LEVEL, args);
   }

   error(...args) {
      _logger(_$log.error, ERROR_LEVEL, args);
   }
}

function _logger(method, level, args) {
   if (gt(level.priority, currentLevel.priority)) {
      return;
   }
   args.unshift(new Date());
   method.apply(_$log, args);
}

function Level(priority, name) {
   this.name = name;
   this.priority = priority;
}
