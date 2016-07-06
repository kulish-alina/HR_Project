const FILE_URL = 'file';
let _HttpService;
let _FileUploader;

import context from './../context';
import { noop } from 'lodash';

const _url = `${context.serverUrl + context.apiSuffix}file`;

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
      return _HttpService.remove(`${FILE_URL}/${file.id}`, file);
   }

   removeGroup(fileIds) {
      return _HttpService.post(`${FILE_URL}/removeGroup`, fileIds);
   }
}
