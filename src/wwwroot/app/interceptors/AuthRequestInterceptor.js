export default function _authInterceptor(
   $injector,
   $translate) {
   'ngInject';
   return {
      request: (config) => {
         config.headers = config.headers || {};
         let localStorageService = $injector.get('LocalStorageService');
         let loggerService = $injector.get('LoggerService');

         let accessToken = localStorageService.get('access_token');
         loggerService.debug('Request auth token', accessToken);

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
