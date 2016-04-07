// Karma configuration
// Generated on Thu Mar 31 2016 17:19:02 GMT+0300 (FLE Daylight Time)

var webpackConfig = require('./webpack.config.js');

console.log(webpackConfig);

module.exports = function(config) {
   config.set({
      // plugins: [
      //    require('karma-webpack'),
      //    require('karma-jasmine'),
      //    require('karma-phantomjs-launcher'),
      //    require('karma-coverage')
      // ],

      frameworks: ['jasmine'],

      files: [
         'config/tests.webpack.js'
      ],

      preprocessors: {
         'config/tests.webpack.js': ['webpack', 'sourcemap']
      },

      reporters: ['progress', 'coverage'],

      port: 9876,

      browsers: ['PhantomJS'],

      // Continuous Integration mode
      // if true, Karma captures browsers, runs the tests and exits
      singleRun: true,

      coverageReporter: {
         dir: 'coverage/',
         reporters: [
            { type: 'text-summary' },
            { type: 'html' }
         ]
      },
      webpack: webpackConfig,
      webpackMiddleware: {
         noInfo: 'errors-only'
      }
   })
}
