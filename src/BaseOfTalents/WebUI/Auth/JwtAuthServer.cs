using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using WebUI.Auth.Infrastructure;

namespace WebUI.Auth
{
    public class JwtAuthServer : OAuthAuthorizationServerProvider
    {
        private IAccountService accountService;

        public JwtAuthServer(IAccountService accService)
        {
            accountService = accService;
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            try
            {
                var user = accountService.Authentificate(context.UserName, context.Password);
                var identity = new ClaimsIdentity("JWT");

                identity.AddClaim(new Claim("sub", user.Email));
                identity.AddClaim(new Claim("id", user.Id.ToString()));


                var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                         "audience", (context.ClientId == null) ? string.Empty : context.ClientId
                    }
                });

                var ticket = new AuthenticationTicket(identity, props);
                context.Validated(ticket);
                return Task.FromResult<object>(null);
            }
            catch (ArgumentException ex)
            {
                context.SetError("invalid_grant", ex.Message);
                context.Rejected();
                return Task.FromResult<object>(null);
            }
        }

    }
}