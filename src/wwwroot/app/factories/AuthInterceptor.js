
export default function _authInterceptor($injector/*, $http*/) {
   return {
      request: (config) => {
         config.headers = config.headers || {};
         let userService = $injector.get('UserService');
         let accessToken = userService.getCurrentUser().token;
         console.log(accessToken);

         if (accessToken) {
            config.headers.Authorization = `Token ${accessToken}`;
         }
         return config;
      },
      requestError: (response) => {
         if (response.status === 401 ||
            response.status === 403 ||
            response.status === 419) {
            //ask user to signin (with login form or with a just a modal reminder)
            let UserDialogService = $injector.get('UserDialogService');
            UserDialogService.notification('Error occured, session is outdated', 'error');
            //return $http(response.config);
            //Make new the same call to api
         }
         return response;
      }
   };
}
