module.exports = function _development(webpack) {
   return {
      debug: true,
      devtool: 'eval-source-map ',
      plugins: [
         new webpack.HotModuleReplacementPlugin()
      ]
   };
};
