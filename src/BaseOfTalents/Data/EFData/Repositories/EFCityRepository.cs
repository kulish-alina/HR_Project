using Domain.Entities.Setup;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFData.Repositories
{
    public class EFCityRepository : EFBaseEntityRepository<Location>, ILocationRepository
    {

    }
}
