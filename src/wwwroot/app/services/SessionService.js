
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

   validateAccess(event, toState, toParams) {
      if (_stateToRedirect && (toState.name === _stateToRedirect.name) ||
         includes(accessArray, toState.name)) {
         return;
      }

      if (_userService.getCurrentUser().login) {
         _loggerService.log('There is a user');
         return;
      } else {
         _loggerService.log('No user!');
         _stateToRedirect = cloneDeep(toState);
         event.preventDefault();
         let authorized = isAuthorized();

         if (authorized) {
            let token = getJwt();
            _loggerService.log(`Authorized with jwt ${token}`);
            _loginService.getUser(token).then(result => {
               _loggerService.log('User', result);
               if (result.login) {
                  _userService.setCurrentUser(result);
                  _loggerService.log('User Authorized');
                  return _$state.go(_stateToRedirect.name, toParams);
               } else {
                  _storageService.remove(tokenInfo);
                  _loggerService.log('Outdated session');

                  return _$state.go('login');
               }
            });
         } else {
            _loggerService.log('Non authorized!!!');
            return _$state.go('login');
         }
      }
   }

   getStateToRedirect() {
      return _stateToRedirect;
   }
}


function isAuthorized() {
   let token = getJwt();
   _loggerService.log(`JWT! ${token}`);
   return token !== undefined && token !== null && token !== '';
}

function getJwt() {
   return _storageService.get(tokenInfo);
}
