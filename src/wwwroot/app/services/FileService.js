const FILES_URL = 'files/';
let _HttpService;
let _FileUploader;

import context from './../context';
import { noop } from 'lodash';

const _url = `${context.serverUrl + context.apiSuffix}files`;

export default class FileService {
   constructor(HttpService, FileUploader) {
      'ngInject';
      _HttpService = HttpService;
      _FileUploader = FileUploader;
   }

   getFileUploader({ onCompleteAllCallBack = noop, maxSize = 0 }) {
      let newUploader = new _FileUploader({
         url: _url,
         onCompleteAll: onCompleteAllCallBack
      });
      newUploader.filters.push({
         name: 'sizeFilter',
         fn: function sizeFilter(item) {
            if (item.size <= maxSize) {
               return true;
            }
         }
      });
      return newUploader;
   }

   remove(file) {
      const additionalUrl = FILES_URL + file.id;
      return _HttpService.remove(additionalUrl, file);
   }
}
