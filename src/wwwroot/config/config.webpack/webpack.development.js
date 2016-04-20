module.exports = function(webpack) {
   return {
      debug: true,
      devtool: 'eval-source-map ',
      plugins: [
         new webpack.HotModuleReplacementPlugin()
      ]
   };
};
