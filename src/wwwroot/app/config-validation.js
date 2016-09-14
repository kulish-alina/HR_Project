import en from './translations/translations-en.json';
import ru from './translations/translations-ru.json';
import context from './context';

import utils from './utils';

import {
   partial,
   isFunction,
   isNaN
} from 'lodash';

const methods = ['minlength', 'maxlength', 'email', 'number', 'url'];
const { array2map } = utils;

export default function _configValidation($validationProvider, ValidationServiceProvider) {
   'ngInject';

   const _it = partial(_converter, $validationProvider);
   const validationExpression = array2map(methods, _it);

   validationExpression.title    = _titleValidation;
   validationExpression.date     = _dateValidation;
   validationExpression.letters  = _lettersValidation;
   validationExpression.counter  = _counterValidation;
   validationExpression.required = _requiredValidation;

   let lang = context.defaultLang || 'en';
   let msg = {en, ru}[lang];

   $validationProvider
      .setDefaultMsg(msg.VALIDATION)
      .setExpression(validationExpression)
      .setValidMethod('blur');

   $validationProvider.showSuccessMessage = false;
   $validationProvider.resetCallback = ValidationServiceProvider.resetCallback;
}

function _converter($validationProvider, nameOfMethod) {
   let orig = _wrap($validationProvider.getExpression(nameOfMethod));
   return function _wrapped(value, scope, element, attrs, param) {
      return value ? orig(value, scope, element, attrs, param) : true;
   };
}

function _wrap(validation) {
   return isFunction(validation) ? validation : (str) => validation.test(str);
}

function _titleValidation(value) {
   const minTitleLength = 3;
   const maxTitleLength = 50;
   return value ? value.length <= maxTitleLength && value.length >= minTitleLength : true;
}

function _dateValidation(value) {
   return value ? !isNaN(Date.parse(utils.formatDateToServer(value))) : true;
}

function _lettersValidation(value) {
   return value ? /^[а-яА-ЯёЁa-zA-Z]+$/.test(value) : true;
}

function _counterValidation(value, scope, element, attrs, param) {
   if (value) {
      let v = parseInt(value, 10);
      return isNaN(v) ? false : v <= parseInt(param || '30', 10);
   } else {
      return true;
   }
}

function _requiredValidation (value, scope, element, attrs) {
   let cond = attrs.isNeedRequired || 'true';
   if (cond === 'true') {
      return !!value;
   } else {
      return true;
   }
}
