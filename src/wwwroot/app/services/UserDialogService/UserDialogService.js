import _confirm from './confirm.view.html';

let _$q, _ModalFactory, _FoundationApi;

export default class UserDialogService {

   constructor($q, ModalFactory, FoundationApi) {
      'ngInject';
      _$q            = $q;
      _ModalFactory  = ModalFactory;
      _FoundationApi = FoundationApi;
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
}
