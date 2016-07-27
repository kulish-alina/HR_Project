module.exports = function _global() {
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
               loader: 'style!css!sass'
            },
            {
               test: /\.json$/,
               loader : 'json-loader'
            },
            {
               test: /\.png$/,
               loader : 'file-loader'
            },
            {
               test   : /\.(ttf|eot|svg|woff(2)?)(\?[a-z0-9]+)?$/,
               loader : 'base64-font-loader'
            }]
      },
      devServer: {
         // historyApiFallback: true,
         // hot: true,
         // inline: true,
         // progress: true,
         //contentBase: './src/public',
         stats: 'minimal'
      }
   };
};
