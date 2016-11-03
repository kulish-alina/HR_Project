const PERMISSIONS_URL = 'permission/';
const ROLES_URL       = 'role/';

import {
   groupBy,
   first,
   filter
} from 'lodash';

let _$q, _HttpService, _HttpCacheService, _LoggerService, _RolesService;
export default class RolesService {
   constructor($q, HttpService, HttpCacheService, LoggerService) {
      'ngInject';
      _HttpService      = HttpService;
      _HttpCacheService = HttpCacheService;
      _LoggerService    = LoggerService;
      _$q               = $q;
      _RolesService     = this;
   }

   getPermissions() {
      return _HttpCacheService.get(PERMISSIONS_URL).then((perm) => {
         return groupBy(perm, 'group');
      });
   }

   getRoles (predicate) {
      return _HttpCacheService.get(ROLES_URL).then((roles) => filter(roles, predicate));
   }

   getById (id) {
      return _RolesService.getRoles({id}).then(first);
   }

   saveRole(role) {
      if (role.id) {
         return _HttpService.put(`${ROLES_URL}${role.id}`, role);
      } else {
         return _HttpService.post(ROLES_URL, role).then(_role => {
            _HttpCacheService.clearCache(ROLES_URL);
            return _role;
         });
      }
   }

   removeRole(role) {
      if (role.id) {
         _HttpCacheService.clearCache(ROLES_URL);
         return _HttpService.remove(`${ROLES_URL}${role.id}`, role);
      } else {
         _LoggerService.debug('Can\'t remove role', role);
         return _$q.reject();
      }
   }
}
