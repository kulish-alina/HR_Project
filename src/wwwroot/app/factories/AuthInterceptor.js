
export default function _authInterceptor($injector, $translate) {
   return {
      request: (config) => {
         let userService = $injector.get('UserService');
         let loggerService = $injector.get('LoggerService');

         let accessToken = userService.getCurrentUser().token;
         loggerService.debug('Stored access token', accessToken);

         config.headers = config.headers || {};

         if (accessToken) {
            config.headers.Authorization = `Token ${accessToken}`;
         }
         return config;
      },
      requestError: (response) => {
         if (response.status === 401 ||
            response.status === 403 ||
            response.status === 419) {
            // TODO: ask user to signin (with login form or with a just a modal reminder)
            let UserDialogService = $injector.get('UserDialogService');
            UserDialogService.notification($translate.instant('LOGIN.SESSION_EXPIRED'), 'error');

            // TODO: Make new the same call to api
            // example : return $http(response.config);
         }
         return response;
      }
   };
}
