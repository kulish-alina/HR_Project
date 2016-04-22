require('angular');
require('angular-mocks/angular-mocks');

var testsContext = require.context('../app/', true, /\.test\.js$/);
testsContext.keys().forEach(testsContext);
