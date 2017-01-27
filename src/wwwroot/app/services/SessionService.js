
import {
   includes,
   cloneDeep
} from 'lodash';

let _$state,
   _userService,
   _loginService,
   _loggerService;

let _stateToRedirect = {};
let _redirectParams = {};

//this is stub till the #237 (access rights for ui routing) is not finished
const accessArray = ['login', 'loading', 'recoverAccount'];

export default class SessionService {
   constructor($state, UserService, LoginService, LoggerService) {
      'ngInject';
      _$state = $state;
      _userService = UserService;
      _loginService = LoginService;
      _loggerService = LoggerService;
   }

   validateAccess(event, toState, toParams) {
      if (_stateToRedirect && (toState.name === _stateToRedirect.name) ||
         includes(accessArray, toState.name)) {
         return;
      }
      event.preventDefault();
      _stateToRedirect = cloneDeep(toState);
      _redirectParams = cloneDeep(toParams);
      _loggerService.debug('State will redirect to:', toState, toParams);

      if (!isAuthorized()) {
         _loggerService.log('Non authorized!!!');
         return _$state.go('login');
      }

      _loggerService.log('User Authorized');
      return _$state.go(_stateToRedirect.name, toParams);
   }

   getStateToRedirect() {
      return _stateToRedirect;
   }

   getRedirectParams() {
      return _redirectParams;
   }
}

function isAuthorized() {
   let token = _loginService.token;
   _loggerService.log(`JWT! ${token}`);
   return token !== undefined && token !== null && token !== '' && !_userService.isCurrentUserEmpty();
}
