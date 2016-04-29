export default function () {
   return function _phoneFilter(tel) {
      if (!tel) {
         return '';
      }
      const value = tel.toString().trim().replace(/^\+/, '');
      // Phone Number length constants
      const nanpNumberLength            = 10; //nanp = North American Numbering Plan
      const euNumberLength              = 11; //eu = European
      const asNumberLength              = 12; //as = Asian
      // NANP number format constants
      const nanpCityBlockStartIndex     = 0;
      const nanpCityBlockEndIndex       = 3;
      // European number format constants
      const euContryBloackIndex         = 0;
      const euCityBlockStartIndex       = 1;
      const euCityBlockEndIndex         = 4;
      // Asian number format constants
      const asCountryBlockStartIndex    = 0;
      const asCountryBlockEndIndex      = 3;
      const asCityBlockStartIndex       = 3;
      const asCityBlockEndIndex         = 5;
      // Number splitter constants
      const numberFirstBlockStartIndex  = 0;
      const numberFirstBlockEndIndex    = 3;
      const numberSecondBlockStartIndex = 3;
      const numberSecondBlockEndIndex   = 5;

      let country, city, number;

      switch (value.length) {
         case nanpNumberLength: // +1PPP####### -> C (PPP) ###-##-##
            country = '';
            city = value.slice(nanpCityBlockStartIndex, nanpCityBlockEndIndex);
            number = value.slice(nanpCityBlockEndIndex);
            break;

         case euNumberLength: // +CPPP####### -> CCC (PP) ###-##-##
            country = value[euContryBloackIndex];
            city = value.slice(euCityBlockStartIndex, euCityBlockEndIndex);
            number = value.slice(euCityBlockEndIndex);
            break;

         case asNumberLength: // +CCCPP####### -> CCC (PP) ###-##-##
            country = `+${value.slice(asCountryBlockStartIndex, asCountryBlockEndIndex)}`;
            city = value.slice(asCityBlockStartIndex, asCityBlockEndIndex);
            number = value.slice(asCityBlockEndIndex);
            break;

         default:
            return tel;
      }

      number = `${
      number.slice(numberFirstBlockStartIndex, numberFirstBlockEndIndex)}-${
      number.slice(numberSecondBlockStartIndex, numberSecondBlockEndIndex)}-${
      number.slice(numberSecondBlockEndIndex)}`;

      return `${country}(${city}) ${number}`;
   };
}
