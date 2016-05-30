const PERMISSIONS_URL = 'permissions/';
const ROLES_URL       = 'roles/';

import {
   reduce
} from 'lodash';

let _$q, _HttpService, ;
export default class RoleService {
   constructor($q, HttpService, _HttpCacheService) {
      'ngInject';
      _$q               = $q;
      _HttpService      = HttpService;
      _HttpCacheService = HttpCacheService;
   }
   getPermissions() {
      return _HttpService.get(PERMISSIONS_URL).then((perm) => {
         return reduce(perm, (memo, permis) => {
            memo[permis.group] = memo[permis.group] || [];
            memo[permis.group].push(permis);
            return memo;
         },{});
      });
   }
   getRoles () {
      return _$q.when({
         Administrator : 268435454,
         Manager       : 56788558,
         Frelancer     : 5565844
      });
   }

   getRoleById(id) {
      return id;
   }

   saveRole(role) {
      if (role.id) {
         return _HttpService.put(`${ROLES_URL}/${role}`);
      } else {
         return _HttpService.post(ROLES_URL, role).then(_role => {
            _HttpCacheService.clearCache(ROLES_URL);
            return _role;
         });
      }
   }
}
