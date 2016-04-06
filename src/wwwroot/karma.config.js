// Karma configuration
// Generated on Thu Mar 31 2016 17:19:02 GMT+0300 (FLE Daylight Time)

var webpackConfig = require('./webpack.config.js');

module.exports = function(config) {
   config.set({
      basePath: '',

      plugins: [
         require('karma-webpack'),
         require('karma-jasmine'),
         require('karma-phantomjs-launcher')
      ],

      frameworks: [ 'jasmine' ],

      files: [
         'node_modules/angular/angular.js',
         'node_modules/angular-mocks/angular-mocks.js',
         'tests/*.js'
      ],

      preprocessors: {
         'tests/*.js': [ 'webpack' ]
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
      // webpack: {
      //    module: {
      //       loaders: [
      //          { test: /\.js$/, exclude: /node_modules/, loader: 'babel?presets[]=es2015' }
      //       ]
      //    },
      //    watch: true
      // },
      webpackServer: {
         noInfo: true
      },
   })
}
