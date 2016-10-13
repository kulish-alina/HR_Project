export default function() {
   return function _fieldName(field) {
      switch (field) {
         case 'FirstName': {
            return 'First name';
         }
         case 'LastName': {
            return 'Last name';
         }
         case 'PositionDesired': {
            return 'Desired position';
         }
         case 'EndDate': {
            return 'End date';
         }
         default: {
            return field;
         }
      }
   };
}
