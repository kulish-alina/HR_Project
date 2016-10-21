using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using WebUI.Infrastructure.Auth;
using WebUI.Results;
using WebUI.Services;

namespace WebUI.Filters
{
    public class AuthAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        IAuthContainer<string> _container;

        public AuthAttribute()
            : base()
        {
            // That will stay here as it is, because autofac has no possibility
            // to relosve the dependencies of attributes metadata
            _container = new TokenContainer();
        }

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            string parameter;
            if (!GetAuthParameter(context, out parameter))
            {
                return Task.FromResult(0);
            }
            var data = _container.Get(parameter);

            if (data == null)
            {
                context.ErrorResult = new AuthFailedResult("Unauthorized access", context.Request);
                return Task.FromResult(0);
            }
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, data.Item1.Email),
                new Claim("Permission", data.Item2.Permissions.ToString())
            };

            var id = new ClaimsIdentity(claims, "Token");
            var principal = new ClaimsPrincipal(new[] { id });

            context.Principal = principal;
            return Task.FromResult(0);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var challenge = new AuthenticationHeaderValue("Token");
            context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);
            return Task.FromResult(0);
        }

        private bool GetAuthParameter(HttpAuthenticationContext context, out string parameter)
        {
            var request = context.Request;
            var auth = request.Headers.Authorization;
            parameter = String.Empty;
            if (auth == null)
            {
                context.ErrorResult = new AuthFailedResult("Not authentificated request. Provide auth data.", request);
                return false;
            }

            if (auth.Scheme != _container.Scheme)
            {
                context.ErrorResult = new AuthFailedResult("Unknown scheme", request);
                return false;
            }

            if (string.IsNullOrEmpty(auth.Parameter))
            {
                context.ErrorResult = new AuthFailedResult("Missing credentials", request);
                return false;
            }

            parameter = auth.Parameter;
            //TODO: Parse parameters if needed

            //TODO: Check token not to be outdated

            return true;
        }
    }
}