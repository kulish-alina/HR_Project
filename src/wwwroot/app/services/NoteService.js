const NOTE_URL = 'note';
let _HttpService, _UserService, _LoggerService, _$q, _$translate;

export default class NoteService {
   constructor(HttpService, UserService, LoggerService, $q, $translate) {
      'ngInject';
      _HttpService   = HttpService;
      _UserService   = UserService;
      _LoggerService = LoggerService;
      _$q            = $q;
      _$translate    = $translate;
   }

   save(entity) {
      if (entity.id) {
         return _HttpService.put(`${NOTE_URL}/${entity.id}`, entity);
      } else {
         let currentUser = _UserService.getCurrentUser();
         entity.userId = currentUser.id;
         return _HttpService.post(NOTE_URL, entity);
      }
   }

   remove(entity) {
      if (entity.id) {
         return _HttpService.remove(`${NOTE_URL}/${entity.id}`, entity);
      } else {
         _LoggerService.debug(_$translate.instant('ERRORS.NOTE_REMOVE_ERROR'), entity);
         return _$q.reject();
      }
   }

   getNotesByUser() {
      let currentUser = _UserService.getCurrentUser();
      return _HttpService.get(`${NOTE_URL}/user/${currentUser.id}`);
   }
}
