import _dialog from './dialog.view.html';
import './dialog.scss';

import {
   assign,
   invoke
} from 'lodash';

let _$q, _ModalFactory, _NotificationFactory, _$translate;

export default class UserDialogService {

   constructor($q, $translate, ModalFactory, NotificationFactory) {
      'ngInject';
      _$q                  = $q;
      _ModalFactory        = ModalFactory;
      _NotificationFactory = NotificationFactory;
      _$translate          = $translate;
   }

   confirm(question) {
      let deferred = _$q.defer();

      let contentScope = {
         ok     : deferred.resolve,
         cancel : deferred.reject
      };

      let buttons = [{ name: _$translate.instant('COMMON.OK'),     func: 'ok' },
                     { name: _$translate.instant('COMMON.CANCEL'), func: 'cancel'}];

      this.dialog(_$translate.instant('DIALOG_SERVICE.CONFIRM'), question, buttons, contentScope);
      return deferred.promise;
   }

   notification(message, type) {
      let notify = new _NotificationFactory({
         position: 'bottom-right'
      });

      const configs = {
         notification: {
            title: _$translate.instant('DIALOG_SERVICE.NOTIF'),
            content: message,
            color: 'info'
         },
         success: {
            title: _$translate.instant('DIALOG_SERVICE.SUCCESS'),
            content: message,
            color: 'success',
            autoclose: '3000'
         },
         error: {
            title: _$translate.instant('DIALOG_SERVICE.ERROR'),
            content: message,
            color: 'alert'
         },
         warning: {
            title: _$translate.instant('DIALOG_SERVICE.WARN'),
            content: message,
            color: 'warning',
            autoclose: '5000'
         }
      };
      notify.addNotification(configs[type] || configs.notification);
   }

   dialog(header, content, buttons, scope) {
   /* header  - text for header into modal window;
      content - html which will be shown into modal;
      buttons - array of objects with properties:
               "name" for buttons text which will be shown,
               "func" for functions name frome attachment scope.
      scope   - an object with variables and functions for content and buttons. */
      let contentScope = assign(scope, {header, content, invoke, buttons});
      let config = {
         template: _dialog,
         contentScope
      };
      let modal = new _ModalFactory(config);
      modal.activate();
   }
}
