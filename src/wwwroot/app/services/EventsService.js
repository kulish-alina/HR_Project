import utils  from '../utils.js';
import {
   each,
   set
} from 'lodash';

const EVENT_URL = 'event';
const EVENT_PROP = ['responsibleId', 'eventTypeId', 'vacancyId', 'candidateId'];
const EVENT_PROP_FRONT = ['responsible', 'eventType', 'vacancy', 'candidate'];
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
      return _HttpService.post(`${EVENT_URL}/search`, condition).then((events) => {
         return each(events, (event) => this._convertFromServerFormat(event));
      });
   }

   save(entity) {
      this._convertToServerFormat(entity);
      if (entity.id) {
         return _HttpService.put(`${EVENT_URL}/${entity.id}`, entity).then(this._convertIdsToString);
      } else {
         return _HttpService.post(EVENT_URL, entity).then(this._convertIdsToString);
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

   _convertToServerFormat(event) {
      this._convertIdsToInt(event);
      each(EVENT_PROP_FRONT, (prop) => {
         delete event[prop];
      });
      event.eventDate = utils.formatDateToServer(event.eventDate);
   }

   _convertFromServerFormat(event) {
      event.eventDate = utils.formatDateFromServer(event.eventDate);
      this._fillEntities(event);
      return this._convertIdsToString(event);
   }

   _convertIdsToString(event) {
      return each(EVENT_PROP, (prop) => {
         event[prop] = `${event[prop]}`;
      });
   }

   _convertIdsToInt(event) {
      return each(EVENT_PROP, (prop) => {
         event[prop] = parseInt(event[prop]);
      });
   }

   _fillEntities(event) {
      let userPromise    = _UserService.getUserById(event.responsibleId).then(user =>
                        set(event, 'responsible', user));
      let vacancyPromise = event.vacancyId ? _VacancyService.getVacancy(event.vacancyId).then(vacancy =>
                        set(event, 'vacancy', vacancy))  : _$q.when(true);
      let candidatePromise = event.candidateId ? _CandidateService.getCandidate(event.candidateId).then(candidate =>
                        set(event, 'candidate', candidate)) : _$q.when(true);
      let thesaurusPromise = event.eventTypeId ? _ThesaurusService.getThesaurusTopic('eventtype', event.eventTypeId)
                        .then(eventType => set(event, 'eventType', eventType)) : _$q.when(true);
      return _$q.all(userPromise, vacancyPromise, candidatePromise, thesaurusPromise);
   }
}
