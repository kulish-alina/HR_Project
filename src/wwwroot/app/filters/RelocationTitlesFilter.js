import { map } from 'lodash';

export default function relocationTitlesFilterCreator() {
   return function _getRelocationsTitles(places) {
      return map(places, place => {
         return place.city ? place.city.title : place.country.title;
      });
   };
}
