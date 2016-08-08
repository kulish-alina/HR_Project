var CopyWebpackPlugin = require('copy-webpack-plugin');

module.exports = function _development(webpack) {
   return {
      debug: true,
      devtool: 'eval-source-map ',
      devServer: {
         historyApiFallback: true,
         hot: true,
         inline: true,
         progress: true
      },
      plugins: [
         new CopyWebpackPlugin([ { from: 'images/', to: 'app/images/' } ]),
         new webpack.HotModuleReplacementPlugin()
      ]
   };
};
