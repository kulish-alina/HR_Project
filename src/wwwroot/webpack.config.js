var path = require('path'),
   webpack = require("webpack"),
   rootPath = __dirname,
   srcPath = path.join(rootPath, 'app'),
   buildPath = path.join(rootPath, 'build'),
   pkg = require('./package.json'),
   HtmlWebpackPlugin = require('html-webpack-plugin');

module.exports = {
   entry: path.join(srcPath, 'main.js'),
   output: {
      path: path.join(buildPath),
      filename: 'bundle.js'
   },
   eslint: {
      configFile: path.join(rootPath, '.eslintrc'),      
      failOnError: true,
      emitWarning: true
   },
   module: {
      preLoadres: [
         {
            test: /\.js$/,
            loaders: 'eslint-loader?{rules:{"no-debugger": 2}}',
            include: srcPath
         }
      ],
      loaders: [
         {
            test: /index.html/,
            loader: 'file?name=templates/[name]-[hash:6].html'
         }, 
         {
            test: /\.html/,
            loader: 'html'
         }, 
         {
            test: /\.js$/,
            exclude: ["node_modules"],
            loader: "ng-annotate?add=true!babel?presets[]=es2015"
         }]
   },
   plugins: [
      new HtmlWebpackPlugin({
         filename: 'index.html',
         pkg: pkg,
         template: path.join(srcPath, 'index.html')
      }),
      new webpack.optimize.OccurenceOrderPlugin(),
      new webpack.optimize.DedupePlugin()
    ],
    devServer: {
      historyApiFallback: true
    }
};
