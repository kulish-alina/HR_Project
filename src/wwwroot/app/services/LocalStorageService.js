
let _storage;
let _logger;

export default class LocalStorageService {
   constructor($window, LoggerService) {
      'ngInject';
      _storage = $window.localStorage;
      _logger = LoggerService;
   }

   /**
    * Function that extracts specific data from session storage
    * @param {string} key Uniq identifier for the value to be extracted
    * @returns {object} The object that was located in session storage
    */
   get(key) {
      let data = _storage.getItem(key);
      try {
         return JSON.parse(data);
      } catch (err) {
         _logger.error('Something went wrong with parsing info', data, key, err);
      }
   }

   /**
    * Function that sets new value to the storage
    * @param {string} key Uniq value for storing incoming data
    * @param {object} value The object is needed to be saved for session life
    * @returns {undefined}
    */
   set(key, value) {
      _logger.log('Adding new session data', value, key);
      let data = JSON.stringify(value);
      try {
         _storage.setItem(key, data);
      } catch (err) {
         _logger.error('Could not save some data into sessionStorage', data, key, err);
      }
   }

   /**
    * Function that removes some data from session storage
    * @param {string} key The key for object that needs to be deleted
    * @returns {undefined}
    */
   remove(key) {
      _logger.log('Removing some session data', key);
      try {
         _storage.removeItem(key);
      } catch (err) {
         _logger.error('Could remove the data with specified key', key, err);
      }
   }

   /**
    * Function that removes all data from session storage
    * @returns {undefined}
    */
   clear() {
      _storage.clear();
   }
}
