module.exports = function(webpack) {
   return {
      debug: true,
      devtool: 'eval-source-map ',
      plugins: [
         new webpack.HotModuleReplacementPlugin()
      ],
      loaders: [
         {
            test: /\.js$/,
            exclude: /\.test\.js$/
         },
      ]
   }
}
