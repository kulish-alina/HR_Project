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
         new webpack.HotModuleReplacementPlugin()
      ]
   };
};
