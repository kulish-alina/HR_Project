var path = require('path'),
    webpack = require("webpack"),
    srcPath = path.join(__dirname, 'app'),
    buildPath = path.join(__dirname, 'build'),
    pkg = require('./package.json'),
    HtmlWebpackPlugin = require('html-webpack-plugin');

module.exports = {
    entry: path.join(srcPath, 'main.js'),
    output: {
        path: path.join(buildPath),
        filename: 'bundle.js'
    },
    module: {
        loaders: [{
            test: /index.html/,
            loader: 'file?name=templates/[name]-[hash:6].html'
        }, {
            test: /\.html/,
            loader: 'html'
        }, {
            test: /\.js$/,
            exclude: ["node_modules"],
            loader: "ng-annotate?add=true!babel?presets[]=es2015"
        }]
    },
    plugins: [
      new HtmlWebpackPlugin({
         filename: 'index.html',
         pkg: pkg,
         template: path.join(srcPath, 'index.html')
      }),
      new webpack.optimize.OccurenceOrderPlugin(),
      new webpack.optimize.DedupePlugin()
    ],
    devServer: {
      historyApiFallback: true
    }
};
