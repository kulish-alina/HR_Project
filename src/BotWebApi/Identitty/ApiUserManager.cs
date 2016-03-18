using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotWebApi.Identitty
{
    public class ApiUserManager : UserManager<ApiUser>
    {
        public ApiUserManager(IUserStore<ApiUser> store)
            : base(store)
        {
        }
    }
}