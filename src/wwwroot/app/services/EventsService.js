const EVENT_URL = 'event';
let _HttpService, _$q, _HttpCacheService;
let currentUser = {};

export default class EventsService {
   constructor(HttpService, $q, HttpCacheService) {
      'ngInject';
      _HttpService = HttpService;
      _$q = $q;
      _HttpCacheService = HttpCacheService;
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
