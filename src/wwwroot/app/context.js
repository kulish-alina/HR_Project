'use strict';

class Context {
   constructor(logLevel, logPattern, serverUrl) {
      this._logLevel = logLevel;
      this._logPattern = logPattern;
      this._serverUrl = serverUrl;
   }

   logLevel() {
      return this._logLevel;
   }

   logPattern() {
      return this._logPattern;
   }

   serverUrl() {
      return this._serverUrl;
   }
}

var context;
if (process.env.NODE_ENV === 'development') {
   context = new Context(3, '*' ,'http://localhost:53031/api/');
} else {
   // a cap while we don't know our URL
   //TODO: URL for website on production server
   context = new Context(4, '*', 'http://bot.com/api');
}

export { context };
// Object.defineProperty(Context, 'logPattern', {
//    configurable: false,
//    enumerable: true,
//    writable: false
// });

// Object.defineProperty(Context, 'logLevel', {
//    configurable : false,
//    enumerable: true,
//    writable: false
// });

// Object.defineProperty(Context, 'serverUrl', {
//    configurable:false,
//    enumerable: true,
//    writable: false
// });
