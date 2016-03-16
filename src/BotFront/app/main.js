import angular from 'angular';
import router  from 'angular-ui-router';

import config  from './botConfig';

var dependencies = [router];

angular
   .module('bot', dependencies)
   .config(config);
