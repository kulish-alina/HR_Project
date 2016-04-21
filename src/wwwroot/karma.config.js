// Karma configuration
// Generated on Thu Mar 31 2016 17:19:02 GMT+0300 (FLE Daylight Time)

var webpackConfig = require('./webpack.config.js');

module.exports = function _karma(config) {
   config.set({

      frameworks: [ 'jasmine' ],

      files: [
         'config/tests.webpack.js'
      ],

      preprocessors: {
         'config/tests.webpack.js': [ 'webpack' ]
      },

      exclude: [],

      reporters: [ 'progress' ],

      port: 9876,

      browsers: [ 'PhantomJS' ],

      // Continuous Integration mode
      // if true, Karma captures browsers, runs the tests and exits
      singleRun: true,

      webpack: webpackConfig,

      plugins: [
         require('karma-webpack'),
         'karma-jasmine',
         'karma-phantomjs-launcher',
         'karma-chrome-launcher'
      ]
   });
};
