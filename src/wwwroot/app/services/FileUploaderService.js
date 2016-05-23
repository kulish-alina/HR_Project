const MAX_SIZE_OF_FILE = 51200;

let _FileUploader;

import context from './../context';
const _url = `${context.serverUrl + context.apiSuffix}files`;

export default class FileUploaderService {
   constructor(FileUploader) {
      'ngInject';
      _FileUploader = FileUploader;
   }

   getFileUploader(onCompleteAllCallBack) {
      let newUploader = onCompleteAllCallBack  ? new _FileUploader({
         url: _url,
         onCompleteAll: onCompleteAllCallBack
      }) : new _FileUploader({
         url: _url
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
