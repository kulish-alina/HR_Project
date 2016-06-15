const EVENT_URL = 'event';
let _HttpService, _$q, _LoggerService, _$translate;

export default class EventsService {
   constructor(HttpService, $q, $translate, LoggerService) {
      'ngInject';
      _HttpService = HttpService;
      _$q = $q;
      _$translate = $translate;
      _LoggerService = LoggerService;
   }

   getEvents(condition) {
      return _HttpService.post(`${EVENT_URL}/search`, condition);
   }

   save(entity) {
      if (entity.id) {
         return _HttpService.put(`${EVENT_URL}/${entity.id}`, entity);
      } else {
         return _HttpService.post(EVENT_URL, entity);
      }
   }

   remove(entity) {
      if (entity.id) {
         return _HttpService.remove(`${EVENT_URL}/${entity.id}`, entity);
      } else {
         _LoggerService.debug(_$translate.instant('ERRORS.EVENT_REMOVE_ERROR'), entity);
         return _$q.reject();
      }
   }

}
