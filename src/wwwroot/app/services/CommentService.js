const NOTE_URL = 'notes/';
let _HttpService;

export default class CommentService {
   constructor(HttpService) {
      'ngInject';
      _HttpService = HttpService;
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
}
