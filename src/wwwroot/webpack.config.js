var path = require('path');
var pkg = require('./package.json');
var webpack = require('webpack');
var HtmlWebpackPlugin = require('html-webpack-plugin');
var merge = require('webpack-merge');

const target = process.env.NODE_ENV;
const paths = {
   appPath: path.join(__dirname, 'app'),
   buildPath: path.join(__dirname, 'build')
}
const wbpk = {
   global: require('./config/config.webpack/webpack.global.js'),
   development: require('./config/config.webpack/webpack.development.js'),
   production: require('./config/config.webpack/webpack.production.js')
}

//Module config
function _load() {
   if (target === 'production') {
      return merge.smart(wbpk.global(paths.appPath, paths.buildPath, pkg), wbpk.production());
   }
   //default state - development (with devserver configs)
   return merge.smart(wbpk.global(paths.appPath, paths.buildPath, pkg), wbpk.development());
}
module.exports = _load();