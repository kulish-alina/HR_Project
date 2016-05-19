const MAX_SIZE_OF_FILE = 5120;

let _FileUploader;

import context from './../context';

export default class FileUploaderService {
   constructor(FileUploader) {
      'ngInject';
      _FileUploader = FileUploader;
   }

   getFileUploader(onCompleteAllCallBack) {
      let newUploader = new _FileUploader({
         url: `${context.url}/files`,
         onCompleteAll: onCompleteAllCallBack
      });
      newUploader.filters.push({
         name: 'sizeFilter',
         fn: function sizeFilter(item) {
            if (item.size <= MAX_SIZE_OF_FILE) {
               return true;
            }
         }
      });
      return newUploader;
   }
}
