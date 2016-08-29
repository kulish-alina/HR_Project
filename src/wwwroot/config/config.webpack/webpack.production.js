var CopyWebpackPlugin = require('copy-webpack-plugin');

module.exports = function _production(webpack, appPath) {
   return {
      context: appPath,
      debug: false,
      module: {

      },
      plugins: [
         new CopyWebpackPlugin([{ from: 'images/', to: 'app/images/' }]),
         new webpack.optimize.UglifyJsPlugin(),
         new webpack.optimize.OccurenceOrderPlugin(),
         new webpack.optimize.DedupePlugin()
      ]
   };
};

