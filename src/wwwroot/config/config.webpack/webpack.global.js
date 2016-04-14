module.exports = function() {
   return {
      module: {
         entry: {
         },
         output: {
         },
         preLoaders: [
            {
               test: /\.js$/,
               exclude: [/node_modules/, /dist/, /\.test\.js$/],
               loader: 'eslint-loader'
            }
         ],
         loaders: [
            {
               test: /\.html/,
               loader: 'html'
            },
            {
               test: /\.js$/,
               exclude: [/node_modules/, /dist/],
               loader: 'ng-annotate?add=true!babel?presets[]=es2015'
            },
            {
               test: /\.css$/,
               loader: 'style-loader!css-loader!postcss-loader'
            },
            {
               test: /\.scss$/,
               loader: 'style-loader!css-loader!sass-loader!postcss-loader'
            },
            {
               test: /\.json$/,
               loader: 'json-loader'
            }]
      },
      devServer: {
         // historyApiFallback: true,
         // hot: true,
         // inline: true,
         // progress: true,
         //contentBase: './src/public',
         stats: 'minimal'
      },
   }
}
