export default function authRunner($state, UserService, LocalStorageService, LoginService, LoggerService) {
   $state.go('loading');
   let token = LocalStorageService.get('access_token');
   LoggerService.debug('runner', token);

   if (!token) {
      $state.go('login');
      return;
   }

   let user = {};
   LoginService.getUser(token).then(result => {
      user = result;
      if (user.login) {
         UserService.setCurrentUser(user);
         LoggerService.log('User Authorized');
         $state.go('home');
      } else {
         LocalStorageService.remove('access_token');
         LoggerService.log('Outdated session');
         $state.go('login');
      }
   });
};
