export default function convertToNumberCreater() {
   return function convertToNumberDirective() {
      return {
         require: 'ngModel',
         link: (scope, element, attrs, ngModel) => {
            ngModel.$parsers.push(val => parseInt(val, 10));
            ngModel.$formatters.push(val => `${val}`);
         }
      };
   };
}
