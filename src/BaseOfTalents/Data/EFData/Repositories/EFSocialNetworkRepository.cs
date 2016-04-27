using Data.Infrastructure;
using Domain.Entities.Setup;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFData.Repositories
{
    public class EFSocialNetworkRepository : EFBaseEntityRepository<SocialNetwork>, ISocialNetworkRepository
    {
        public EFSocialNetworkRepository(IDbFactory factory) : base(factory)
        {

        }
    }
}
