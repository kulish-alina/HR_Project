import _dialog from './dialog.view.html';
import './dialog.scss';

import {
   assign,
   forEach,
   isFunction
} from 'lodash';

let _$q, _ValidationService, _FoundationApi, _ModalFactory, _NotificationFactory, _$translate;

export default class UserDialogService {

   constructor($q, $translate, ValidationService, FoundationApi, ModalFactory, NotificationFactory) {
      'ngInject';
      _$q                  = $q;
      _$translate          = $translate;
      _ValidationService   = ValidationService;
      _FoundationApi       = FoundationApi;
      _ModalFactory        = ModalFactory;
      _NotificationFactory = NotificationFactory;
   }

   confirm(question) {
      let deferred = _$q.defer();

      let buttons = [{ name: _$translate.instant('COMMON.CANCEL'), func: deferred.reject  },
                     { name: _$translate.instant('COMMON.OK'),     func: deferred.resolve }];

      this.dialog(_$translate.instant('DIALOG_SERVICE.CONFIRM'), question, buttons);
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
               "func" for functions which will be fired after click,
               "needValidate for flag which will be true if need to call validation
      scope   - an object with variables for content. */
      let wrappedButtons = forEach(buttons, (value) => {
         value.func = _closeElementWrapp(value.func);
         if (value.needValidate) {
            value.func = _validationWrapp(value.func);
         }
      },{});

      let contentScope = assign(scope, {
         header,
         buttons: wrappedButtons
      });
      let config = {
         template: _dialog.split('<!-- content will be here -->').join(content),
         contentScope,
         overlayClose: 'false'
      };
      let modal = new _ModalFactory(config);
      modal.activate();
   }
}

function _closeElementWrapp(func) {
   return () => {
      if (isFunction(func)) {
         func();
      }
      _FoundationApi.closeActiveElements();
   };
}

function _validationWrapp(callback) {
   return (form) => {
      if (_ValidationService.validate(form)) {
         callback();
      }
   };
}
