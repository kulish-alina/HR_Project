import _confirm from './confirm.view.html';

let _$q, _ModalFactory, _NotificationFactory, _FoundationApi;

export default class UserDialogService {

   constructor($q, ModalFactory, NotificationFactory, FoundationApi) {
      'ngInject';
      _$q                  = $q;
      _ModalFactory        = ModalFactory;
      _NotificationFactory = NotificationFactory;
      _FoundationApi       = FoundationApi;
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
            cancell : () => {
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
            title: 'Notification!',
            content: message,
            color: 'info'
         },
         success: {
            title: 'Success!',
            content: message,
            color: 'success',
            autoclose: '3000'
         },
         error: {
            title: 'Error!',
            content: message,
            color: 'alert'
         },
         warning: {
            title: 'Warning!',
            content: message,
            color: 'warning',
            autoclose: '5000'
         }
      };
      notify.addNotification(configs[type] || configs.notification);
   }
}
