module.exports = function _test() {
   return {
      devtool: 'inline-source-map',
      module: {
         // preLoaders: [
         //    {
         //       test: /\.js$/,
         //       exclude: [/node_modules/, /\.test\.js$/, /config/],
         //       loader: 'isparta-instrumenter?presets[]=es2015'
         //    }
         // ]
      }
   };
};
