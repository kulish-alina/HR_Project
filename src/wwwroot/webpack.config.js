var path = require('path');
var pkg = require('./package.json');
var webpack = require('webpack');
var merge = require('webpack-merge');

const target = process.env.NODE_ENV;

const paths = {
   appPath: path.join(__dirname, 'app'),
   buildPath: path.join(__dirname, 'dist')
};
const wbpk = {
   global: require('./config/config.webpack/webpack.global.js'),
   output: require('./config/config.webpack/webpack.output.js'),
   development: require('./config/config.webpack/webpack.development.js'),
   production: require('./config/config.webpack/webpack.production.js'),
   test: require('./config/config.webpack/webpack.test.js')
};

//Module config
function _load() {
   if (target === 'test') {
      return merge.smart(wbpk.global(), wbpk.test());
   } else if (target === 'production') {
      return merge.smart(wbpk.global(),
         wbpk.output(path, paths.appPath, paths.buildPath, pkg),
         wbpk.production(webpack, paths.appPath, paths.buildPath));
   }
   // default state - development (with devserver configs)
   return merge.smart(wbpk.global(),
      wbpk.output(path, paths.appPath, paths.buildPath, pkg),
      wbpk.development(webpack));
}
module.exports = _load();
