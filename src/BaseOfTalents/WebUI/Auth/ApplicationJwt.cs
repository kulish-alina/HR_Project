using System;
using System.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using Thinktecture.IdentityModel.Tokens;

namespace WebUI.Auth
{
    public class ApplicationJwt : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly string _issuer;
        private readonly string _secret;

        public ApplicationJwt(string issuer, string secret)
        {
            _issuer = issuer;
            _secret = secret;
        }

        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            var signingKey = new HmacSigningCredentials(_secret);
            var issued = data.Properties.IssuedUtc;
            var expires = data.Properties.ExpiresUtc;

            return new JwtSecurityTokenHandler()
                .WriteToken(new JwtSecurityToken(_issuer, "Any", data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingKey));
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}