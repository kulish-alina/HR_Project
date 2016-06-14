const NOTE_URL = 'note';
let currentUserId = null;
let _HttpService, _UserService, _LoggerService, _$q;

export default class NoteService {
   constructor(HttpService, UserService, LoggerService, $q) {
      'ngInject';
      _HttpService = HttpService;
      _UserService = UserService;
      _LoggerService = LoggerService;
      _$q = $q;
   }

   save(entity) {
      if (entity.id) {
         return _HttpService.put(`${NOTE_URL}/${entity.id}`, entity);
      } else {
         return _UserService.getUserById(1).then((user) => {
            entity.userId = user.id;
            return _HttpService.post(NOTE_URL, entity);
         });
      }
   }

   remove(entity) {
      if (entity.id) {
         return _HttpService.remove(`${NOTE_URL}/${entity.id}`, entity);
      } else {
         _LoggerService.debug('Can\'t remove new note', entity);
         return _$q.when(true);
      }
   }

   getNotesByUser() {
      return _UserService.getUserById(1).then((user) => {
         currentUserId = user.id;
         return _HttpService.get(`${NOTE_URL}/user/${currentUserId}`).then((notes) => {
            return notes;
         });
      });
   }
}
