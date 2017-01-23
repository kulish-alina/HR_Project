import jwtDecode from 'jwt-decode';

const url = 'account/';

let _$q,
   _httpService,
   _userService,
   _storageService,
   _loggerService;

const tokenInfo = 'access_token';

export default class LoginService {
   constructor($q, HttpService, UserService, LocalStorageService, LoggerService) {
      'ngInject';
      _$q = $q;
      _httpService = HttpService;
      _storageService = LocalStorageService;
      _userService = UserService;
      _loggerService = LoggerService;
   }

   set token(token) {
      _loggerService.debug('Login data', token);
      _storageService.set(tokenInfo, token);
   };

   get token() {
      return _storageService.get(tokenInfo);
   }

   login(credentials) {
      let entity = credentials;
      entity.grant_type = 'password';
      _loggerService.debug(entity);

      return _httpService.post(`${url}signin`, entity, 'url').then(identity => {
         _storageService.clear();
         this.token = identity.access_token;
         this.setCurrentUser(false);
         return identity;
      }).catch(error => {
         _loggerService.debug('Loggin failed', error.data.message);
         return _$q.reject(error);
      });
   };

   logout() {
      return _httpService.post(`${url}logout`).then(logoutResult => {
         if (logoutResult) {
            _storageService.remove(tokenInfo);
            _userService.setCurrentUser({});
         }
         return logoutResult;
      });
   }

   setCurrentUser(needToConvert) {
      let decodedToken = jwtDecode(this.token);
      _loggerService.debug('Decoded token:', decodedToken);
      _userService
         .getUserById(decodedToken.id, needToConvert)
         .then(user => _userService.setCurrentUser(user));
   }
}
