import translationsEn from './translations/translationsEn.json';
import translationsRu from './translations/translationsRu.json';
import context from './context';
import {
   reduce,
   isFunction,
   constant
} from 'lodash';

export default function _configValidation($validationProvider) {

   const _true = constant(true);
   const methods = ['minlength', 'maxlength', 'email', 'number', 'url'];
   const _origin = reduce(methods, (memo, nameOfMethod) => {
      memo[nameOfMethod] = _wrap($validationProvider.getExpression(nameOfMethod));
      return memo;
   }, {});

   function _wrap(validation) {
      return isFunction(validation) ? validation : (str) => validation.test(str);
   }
   const validationExpression = reduce(methods, (memo, nameOfMethod) => {
      memo[nameOfMethod] = (value, scope, element, attrs, param) => {
         let orig = _origin[nameOfMethod];
         return value ? orig(value, scope, element, attrs, param) : _true;
      };
      return memo;
   }, {});
   validationExpression.title = function _titleValidation(value) {
      const minTitleLength = 3;
      const maxTitleLength = 50;
      return value ? value.length <= maxTitleLength && value.length >= minTitleLength : true;
   };
   $validationProvider.showSuccessMessage = false;
   $validationProvider
      .setDefaultMsg({ en: translationsEn.VALIDATION,
                       ru: translationsRu.VALIDATION}[context.defaultLang || 'en'])
      .setExpression(validationExpression)
      .setValidMethod('blur');
}
