var HtmlWebpackPlugin = require('html-webpack-plugin');

module.exports = function _output(path, appPath, buildPath, pkg) {
   return {
      entry: {
         app: path.join(appPath, 'main.js')
      },
      output: {
         path: buildPath,
         filename: '[name].bundle.js'
      },
      module: {
         preLoaders: [
            {
               test: /\.js$/,
               exclude: [/node_modules/, /dist/],
               loader: 'eslint-loader'
            }
         ]
      },
      plugins: [
         new HtmlWebpackPlugin({
            filename: 'index.html',
            pkg: pkg,
            template: path.join(appPath, 'index.html')
         })
      ],
      eslint: {
         configFile: '.eslintrc',
         failOnError: true,
         quiet: true
      }
   };
};
