const NOTE_URL = 'note/';
let _HttpService, _UserService;

export default class NoteService {
   constructor(HttpService, UserService) {
      'ngInject';
      _HttpService = HttpService;
      _UserService = UserService;
   }

   save(entity) {
      if (entity.id) {
         return _HttpService.put(`${NOTE_URL}/${entity}`);
      } else {
         return _HttpService.post(NOTE_URL, entity).then(comment => {
            return comment;
         });
      }
   }

   getNotesByUser() {
      let currentUserId = null;
      _UserService.getUserById(1).then((user) => {
         currentUserId = user.id;
         return _HttpService.get(NOTE_URL + currentUserId).then((comments) => {
            return comments;
         });
      });

   }
}
