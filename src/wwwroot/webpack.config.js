var path = require('path'),
   webpack = require("webpack"),
   rootPath = __dirname,
   appPath = path.join(rootPath, 'app'),
   buildPath = path.join(rootPath, 'build'),
   pkg = require('./package.json'),
   HtmlWebpackPlugin = require('html-webpack-plugin');

module.exports = {
   entry: path.join(appPath, 'main.js'),
   output: {
      path: path.join(buildPath),
      filename: 'bundle.js'
   },
   module: {
      preLoaders: [
         {
            test: /\.js$/,
            exclude: /node_modules/,
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
            exclude: ["node_modules"],
            loader: 'ng-annotate?add=true!babel?presets[]=es2015'
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
      new webpack.optimize.DedupePlugin()
   ],
   devServer: {
      historyApiFallback: true
   }
};
