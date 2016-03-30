var webpack = require('webpack');

module.exports = function() {
   return {
      debug: true,
      devtool: 'source-map ',
      devServer: {
         historyApiFallback: true,
         hot: true,
         inline: true,
         progress: true,
      },
      plugins: [
         new webpack.HotModuleReplacementPlugin()
      ]
   }
}
