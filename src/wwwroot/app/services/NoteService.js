import utils  from '../utils.js';
import {
   each
} from 'lodash';
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
      this.convertDateToServerFormat(entity);
      if (entity.id) {
         return _HttpService.put(`${NOTE_URL}/${entity.id}`, entity).then((comment) => {
            return this.convertDateFromServerFormat(comment);
         });
      } else {
         let currentUser = _UserService.getCurrentUser();
         entity.userId = currentUser.id;
         return _HttpService.post(NOTE_URL, entity).then((comment) => {
            return this.convertDateFromServerFormat(comment);
         });
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
      return _HttpService.get(`${NOTE_URL}/user/${currentUser.id}`).then((comments) => {
         return each(comments, (comment) => {
            return this.convertDateFromServerFormat(comment);
         });
      });
   }

   convertDateFromServerFormat(entity) {
      entity.createdOn = utils.formatDateFromServer(entity.createdOn);
      return entity;
   }
   convertDateToServerFormat(entity) {
      entity.createdOn = utils.formatDateToServer(entity.createdOn);
      return entity;
   }
}
