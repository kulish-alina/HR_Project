using System;
using System.IdentityModel.Tokens;

namespace WebUI.Auth
{
    public static class PayloadDecoder
    {
        public static int TryGetId(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();

            var tokenObj = jwtHandler.ReadToken(token) as JwtSecurityToken;
            object id;
            var result = tokenObj.Payload.TryGetValue("id", out id);
            return Convert.ToInt32(id);
        }
    }
}