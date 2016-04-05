// Karma configuration
// Generated on Thu Mar 31 2016 17:19:02 GMT+0300 (FLE Daylight Time)

var  webpackConfig = require('./webpack.config.js');

module.exports = function(config) {
   config.set({
      basePath: '',

      plugins:[
         require('karma-webpack'),
         require('karma-jasmine'),
         require('karma-phantomjs-launcher')
      ],

      frameworks: [ 'jasmine' ],

      files: [
         'tests/*.js',
         'app/**/*.test.js'
      ],

      preprocessors: {
         'tests/*.js' : [ 'webpack' ],
         'app/**/*.test.js': [ 'webpack' ]
      },

      reporters: [ 'progress' ],

      port: 9876,

      autoWatch: true,

      browsers: [ 'PhantomJS' ],

      // Continuous Integration mode
      // if true, Karma captures browsers, runs the tests and exits
      singleRun: false,

      // Concurrency level
      // how many browser should be started simultaneous
      concurrency: Infinity,

      webpack: webpackConfig,

      webpackServer:{
         noInfo: true
      },
   })
}
