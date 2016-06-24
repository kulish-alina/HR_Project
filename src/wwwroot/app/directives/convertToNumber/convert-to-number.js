import { toNumber, toString } from 'lodash';

export default function convertToNumberDirective() {
   return {
      require: 'ngModel',
      link: (scope, element, attrs, ngModel) => {
         ngModel.$parsers.push(toNumber);
         ngModel.$formatters.push(toString);
      }
   };
}
