import utils  from '../utils.js';
import {
   each,
   set
} from 'lodash';

const EVENT_URL = 'event';
const EVENT_PROP = ['responsibleId', 'eventTypeId', 'vacancyId', 'candidateId'];
let _HttpService, _$q, _LoggerService, _$translate, _ThesaurusService, _VacancyService,
   _CandidateService, _UserService;

export default class EventsService {
   constructor(HttpService, $q, $translate, LoggerService, VacancyService, CandidateService, UserService,
                           ThesaurusService) {
      'ngInject';
      _HttpService        = HttpService;
      _$q                 = $q;
      _$translate         = $translate;
      _LoggerService      = LoggerService;
      _UserService        = UserService;
      _VacancyService     = VacancyService;
      _CandidateService   = CandidateService;
      _ThesaurusService   = ThesaurusService;
   }

   getEventsByCandidate(candidateId) {
      return _HttpService.get(`${EVENT_URL}/candidate/${candidateId}`).then(this.convertFromServerFormat);
   }

   getEventsForPeriod(condition) {
      return _HttpService.post(`${EVENT_URL}/search`, condition).then(this.convertFromServerFormat);
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

   _convertFromServerFormat(event) {
      event.eventDate = utils.formatDateFromServer(event.eventDate);
      return this._fillEntities(event);
   }

   _convertIdsToString(event) {
      each(EVENT_PROP, (prop) => {
         event[prop] = toString(event[prop]);
      });
      return event;
   }

   _fillEntities(event) {
      let userPromise    = _UserService.getUserById(event.responsibleId).then(user =>
                        set(event, 'responsible', user));
      let vacancyPromise = _VacancyService.getVacancy(event.vacancyId).then(vacancy =>
                        set(event, 'vacancy', vacancy));
      let candidatePromise = _CandidateService.getCandidate(event.candidateId).then(candidate =>
                        set(event, 'candidate', candidate));
      let thesaurusPromise = _ThesaurusService.getThesaurusTopic('eventtype', event.eventTypeId)
                        .then(eventType => set(event, 'eventType', eventType));
      return _$q.all(userPromise, vacancyPromise, candidatePromise, thesaurusPromise);
   }
}
