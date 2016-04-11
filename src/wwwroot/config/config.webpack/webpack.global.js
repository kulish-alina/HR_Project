var webpack = require('webpack');
var HtmlWebpackPlugin = require('html-webpack-plugin');
var path = require('path');

module.exports = function(appPath, buildPath, pkg) {
   return {
      entry: {
         entry: path.join(appPath, 'main.js'),
      },
      output: {
         path: buildPath,
         filename: 'bundle.js'
      },
      module: {
         preLoaders: [
            {
               test: /\.js$/,
               exclude: ['node_modules', 'build'],
               loader: 'eslint-loader'
            }
         ],
         loaders: [
            {
               test: /\.html/,
               loader: 'html'
            },
            {
               test: /\.js$/,
               exclude: /node_modules/,
               loader: 'ng-annotate?add=true!babel?presets[]=es2015'
            },
            {
               test: /\.css$/,
               loader: 'style-loader!css-loader!postcss-loader'
            },
            {
               test: /\.scss$/,
               loader: 'style-loader!css-loader!sass-loader!postcss-loader'
            },
            {
               test: /\.json$/,
               loader : 'json-loader'
            }]
      },
      eslint: {
         configFile: '.eslintrc',
         emitError: true,
         emitWarning: true,
         failOnError: true
      },
      plugins: [
         new HtmlWebpackPlugin({
            filename: 'index.html',
            pkg: pkg,
            template: path.join(appPath, 'index.html')
         }),
         new webpack.optimize.OccurenceOrderPlugin(),
         new webpack.optimize.DedupePlugin(),
      ]
   }
}
