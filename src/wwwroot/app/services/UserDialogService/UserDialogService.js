import _confirm from './confirm.view.html';

let _$q, _ModalFactory, _NotificationFactory, _FoundationApi, _$translate;

export default class UserDialogService {

   constructor($q, $translate, ModalFactory, NotificationFactory, FoundationApi) {
      'ngInject';
      _$q                  = $q;
      _ModalFactory        = ModalFactory;
      _NotificationFactory = NotificationFactory;
      _FoundationApi       = FoundationApi;
      _$translate          = $translate;
   }

   confirm(question) {
      let deferred = _$q.defer();
      let config = {
         template: _confirm,
         contentScope: {
            question,
            ok      : () => {
               deferred.resolve();
               _FoundationApi.closeActiveElements();
            },
            cancel : () => {
               deferred.reject();
               _FoundationApi.closeActiveElements();
            }
         }
      };
      let modal = new _ModalFactory(config);
      modal.activate();
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
}
