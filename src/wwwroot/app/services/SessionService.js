
import {
   includes,
   cloneDeep
} from 'lodash';

let _$state,
   _userService,
   _storageService,
   _loginService,
   _loggerService;

let _stateToRedirect = {};
const tokenInfo = 'access_token';

//this is stub till the #237 (access rights for ui routing) is not finished
const accessArray = ['login', 'loading', 'recoverAccount'];

export default class SessionService {
   constructor($state, UserService, LocalStorageService, LoginService, LoggerService) {
      'ngInject';
      _$state = $state;
      _userService = UserService;
      _storageService = LocalStorageService;
      _loginService = LoginService;
      _loggerService = LoggerService;
   }

   validateAccess(event, toState, toParams) {
      if (_stateToRedirect && (toState.name === _stateToRedirect.name) ||
         includes(accessArray, toState.name)) {
         return;
      }

      if (!_userService.isCurrentUserEmpty()) {
         _loggerService.log('There is a user');
         return;
      }

      _loggerService.log('No user!');
      _stateToRedirect = cloneDeep(toState);
      event.preventDefault();

      if (!isAuthorized()) {
         _loggerService.log('Non authorized!!!');
         return _$state.go('login');
      }

      _loginService.setCurrentUser(true);
      if (_userService.isCurrentUserEmpty()) {
         _storageService.remove(tokenInfo);
         _loggerService.log('Outdated session');

         return _$state.go('login');
      }

      _loggerService.log('User Authorized');
      return _$state.go(_stateToRedirect.name, toParams);
   }

   getStateToRedirect() {
      return _stateToRedirect;
   }
}

function isAuthorized() {
   let token = _loginService.token;
   _loggerService.log(`JWT! ${token}`);
   return token !== undefined && token !== null && token !== '';
}
