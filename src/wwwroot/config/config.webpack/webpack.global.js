module.exports = function() {
   return {
      external: {
         'iconic': 'IconicJS'
      },
      resolve: {
         modulesDirectories: ['web_modules', 'node_modules', 'bower_components']
      },
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
               loader: 'style!css!sass?outputStyle=expanded&includePaths[]=./bower_components/foundation-apps/scss/'
            },
            {
               test: /\.json$/,
               loader : 'json-loader'
            },
            {
               test   : /\.(ttf|png|eot|svg|woff(2)?)(\?[a-z0-9]+)?$/,
               loader : 'file-loader'
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
