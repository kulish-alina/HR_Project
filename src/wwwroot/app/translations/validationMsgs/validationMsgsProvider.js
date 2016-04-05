import en from './validationMsgsEn.json';
export default function ValidationMsgsProvider(localLenguage) {
   var result = en;
   //if (this[localLenguage]) {
   //   result = this[localLenguage];
   //}
   return result;
}
