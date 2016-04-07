var HtmlWebpackPlugin = require('html-webpack-plugin');

module.exports = function(path, appPath, buildPath, pkg) {
   return {
      entry: {
         app: path.join(appPath, 'main.js'),
      },
      output: {
         path: buildPath,
         filename: '[name].bundle.js'
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
         emitError: true,
         emitWarning: true,
         failOnError: true
      },
   }
}
