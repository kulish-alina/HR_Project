module.exports = function _production(webpack) {
   return {
      debug: false,
      devtool: 'source-map ',
      module: {

      },
      plugins: [
         new webpack.optimize.UglifyJsPlugin(),
         new webpack.optimize.OccurenceOrderPlugin(),
         new webpack.optimize.DedupePlugin()
      ]
   };
};

