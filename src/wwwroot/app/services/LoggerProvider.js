import { find } from 'lodash';

let currentLevel;
let _$log;

const OFF_LEVEL      = new Level(0, 'OFF');
const ERROR_LEVEL    = new Level(1, 'ERROR');
const WARNING_LEVEL  = new Level(2, 'WARNING');
const LOG_LEVEL      = new Level(3, 'LOG');
const DEBUG_LEVEL    = new Level(4, 'DEBUG');

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

   warning(...args) {
      _logger(_$log.warn, WARNING_LEVEL, args);
   }

   error(...args) {
      _logger(_$log.error, ERROR_LEVEL, args);
   }
}

function _logger(method, level, args) {
   if (level.compareTo(currentLevel) === 1) {
      return;
   }
   args.unshift(new Date());
   method.apply(_$log, args);
}

function Level(priority, name) {
   this.name = name;
   this.priority = priority;
}

Level.prototype.compareTo = function compareTo(another) {
   return this.priority < another.priority ? -1 : this.priority > another.priority ? 1 : 0;
}
