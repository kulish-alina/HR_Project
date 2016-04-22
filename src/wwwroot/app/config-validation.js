import en from './translations/translations-en.json';
import ru from './translations/translations-ru.json';
import context from './context';

import {
   reduce,
   isFunction,
   constant
} from 'lodash';

const methods = ['minlength', 'maxlength', 'email', 'number', 'url'];
const _true = constant(true);

export default function _configValidation($validationProvider) {
   const _origin = reduce(methods, (memo, nameOfMethod) => {
      memo[nameOfMethod] = _wrap($validationProvider.getExpression(nameOfMethod));
      return memo;
   }, {});

   const validationExpression = reduce(methods, (memo, nameOfMethod) => {
      memo[nameOfMethod] = (value, scope, element, attrs, param) => {
         let orig = _origin[nameOfMethod];
         return value ? orig(value, scope, element, attrs, param) : _true;
      };
      return memo;
   }, {});

   validationExpression.title = _titleValidation;

   let lang = context.defaultLang || 'en';
   let msg = {en, ru}[lang];

   $validationProvider
      .setDefaultMsg(msg.VALIDATION)
      .setExpression(validationExpression)
      .setValidMethod('blur');

   $validationProvider.showSuccessMessage = false;
}

function _wrap(validation) {
   return isFunction(validation) ? validation : (str) => validation.test(str);
}

function _titleValidation(value) {
   const minTitleLength = 3;
   const maxTitleLength = 50;
   return value ? value.length <= maxTitleLength && value.length >= minTitleLength : true;
}
