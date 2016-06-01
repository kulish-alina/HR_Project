const PERMISSIONS_URL = 'permissions/';
const ROLES_URL       = 'roles/';

import {
   reduce
} from 'lodash';

let _$q, _HttpService, _HttpCacheService, _LoggerService;
export default class RoleService {
   constructor($q, HttpService, HttpCacheService, LoggerService) {
      'ngInject';
      _HttpService      = HttpService;
      _HttpCacheService = HttpCacheService;
      _LoggerService    = LoggerService;
      _$q               = $q;
   }

   getPermissions() {
      return _HttpCacheService.get(PERMISSIONS_URL).then((perm) => {
         return reduce(perm, (memo, permis) => {
            memo[permis.group] = memo[permis.group] || [];
            memo[permis.group].push(permis);
            return memo;
         },{});
      });
   }

   getRoles () {
      return _HttpCacheService.get(ROLES_URL).then((rol) => {
         return reduce(rol, (memo, role) => {
            memo[role.title] = role;
            return memo;
         }, {});
      });
   }

   saveRole(role) {
      if (role.id) {
         return _HttpService.put(`${ROLES_URL}/${role.id}`, role);
      } else {
         return _HttpService.post(ROLES_URL, role).then(_role => {
            _HttpCacheService.clearCache(ROLES_URL);
            return _role;
         });
      }
   }

   removeRole(role) {
      if (role.id) {
         const additionalUrl = ROLES_URL + role.id;
         return _HttpService.remove(additionalUrl, role);
         _HttpCacheService.clearCache(ROLES_URL);
      } else {
         _LoggerService.debug('Can\'t remove role', role);
         return _$q.when(true);
      }
   }
}
