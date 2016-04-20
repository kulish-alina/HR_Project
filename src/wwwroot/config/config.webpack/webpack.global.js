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
      resolve: {
         modulesDirectories: ["web_modules", "node_modules", "bower_components"]
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
               loader: 'style!css!sass?outputStyle=expanded&includePaths[]=./bower_components/foundation-apps/scss/'
            },
            {
               test: /\.json$/,
               loader : 'json-loader'
            },
            {
               test   : /\.(ttf|png|eot|svg|woff(2)?)(\?[a-z0-9]+)?$/,
               loader : 'file-loader'
            }]
      },

      eslint: {
         configFile: '.eslintrc',
         emitError: true,
         emitWarning: true,
         failOnError: true
      },
      plugins: [
         new webpack.ResolverPlugin(
            new webpack.ResolverPlugin.DirectoryDescriptionFilePlugin(".bower.json", ["main"])
         ),
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
