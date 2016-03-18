var path = require('path');
var pkg = require('./package.json');
var webpack = require('webpack');
var HtmlWebpackPlugin = require('html-webpack-plugin');
var merge = require('webpack-merge');

const TARGET = process.env.API_ENV;
const paths = {
   appPath: path.join(__dirname, 'app'),
   buildPath: path.join(__dirname, 'build')
}


const common = {

   // Entry accepts a path or an object of entries. We'll be using the
   // latter form given it's convenient with more complex configurations.
   entry: {
      entry: path.join(paths.appPath, 'main.js'),
   },
   output: {
      path: paths.buildPath,
      filename: 'bundle.js'
   }
};
function _load() {
   if (TARGET === 'production') {
      return merge(common, {})
      // /module.exports = merge(common, {});
   }
   
   return merge(common, {})
   //module.exports = merge(common, {});
}

module.exports = _load();
// module.exports = {
//    entry: path.join(appPath, 'main.js'),
//    output: {
//       path: path.join(buildPath),
//       filename: 'bundle.js'
//    },
//    module: {
//       preLoaders: [
//          {
//             test: /\.js$/,
//             exclude: ['node_modules', 'build'],
//             loader: 'eslint-loader'
//          }
//       ],
//       loaders: [
//          {
//             test: /\.html/,
//             loader: 'html'
//          },
//          {
//             test: /\.js$/,
//             exclude: /node_modules/,
//             loader: 'ng-annotate?add=true!babel?presets[]=es2015'
//          },
//          {
//             test: /\.css$/,
//             loader: 'style-loader!css-loader!autoprefixer-loader'
//          },
//          {
//             test: /\.scss$/,
//             loader: 'style-loader!css-loader!sass-loader!autoprefixer-loader'
//          }]
//    },
//    eslint: {
//       configFile: '.eslintrc',
//       emitError: true,
//       emitWarning: true,
//       failOnError: true
//    },
//    plugins: [
//       new HtmlWebpackPlugin({
//          filename: 'index.html',
//          pkg: pkg,
//          template: path.join(appPath, 'index.html')
//       }),
//       new webpack.optimize.OccurenceOrderPlugin(),
//       new webpack.optimize.DedupePlugin(),
//       new webpack.DefinePlugin({
//          'process.env.NODE_ENV': '"development"'
//       }),
//    ],
//    devServer: {
//       historyApiFallback: true
//    }
// };
