
import {
   includes,
   cloneDeep
} from 'lodash';

let _$state,
   _userService,
   _storageService,
   _loginService,
   _loggerService;

let _stateToRedirect;
const tokenInfo = 'access_token';

//this is stub till the #237 (access rights for ui routing) is not finished
const accessArray = ['login', 'loading'];

export default class SessionService {
   constructor($state, UserService, LocalStorageService, LoginService, LoggerService) {
      'ngInject';
      _$state = $state;
      _userService = UserService;
      _storageService = LocalStorageService;
      _loginService = LoginService;
      _loggerService = LoggerService;
   }

   validateAccess(event, toState) {
      if (toState.name === _stateToRedirect.name || includes(accessArray, toState.name)) {
         return;
      }
      _stateToRedirect = cloneDeep(toState);

      let token = _storageService.get(tokenInfo);
      event.preventDefault();

      if (!token) {
         return _$state.go('login');
      }

      let user = {};
      _loginService.getUser(token).then(result => {
         user = result;
         if (user.login) {
            _userService.setCurrentUser(user);
            _loggerService.log('User Authorized');

            return _$state.go(_stateToRedirect.name, _stateToRedirect.params);
         } else {
            _storageService.remove(tokenInfo);
            _loggerService.log('Outdated session');

            return _$state.go('login');
         }
      });
   }

   getStateToRedirect() {
      return _stateToRedirect;
   }
}

// function isAuthenticated() {
//    return _storageService.get(tokenInfo);
// }
