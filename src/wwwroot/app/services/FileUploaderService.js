const MAX_SIZE_OF_FILE = 51200;

let _FileUploader;

import context from './../context';

export default class FileUploaderService {
   constructor(FileUploader) {
      'ngInject';
      _FileUploader = FileUploader;
   }

   getFileUploader(onCompleteAllCallBack) {
      let newUploader = onCompleteAllCallBack  ? new _FileUploader({
         url: `${context.serverUrl}files`,
         onCompleteAll: onCompleteAllCallBack
      }) : new _FileUploader({
         url: `${context.serverUrl}files`
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
