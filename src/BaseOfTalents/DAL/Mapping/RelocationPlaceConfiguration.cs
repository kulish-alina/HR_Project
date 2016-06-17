using BaseOfTalents.DAL.Mapping;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mapping
{
    public class RelocationPlaceConfiguration : BaseEntityConfiguration<RelocationPlace>
    {
        public RelocationPlaceConfiguration()
        {
            HasRequired(x => x.Country).WithMany().HasForeignKey(x => x.CountryId);
            HasMany(x => x.Cities).WithMany(x => x.RelocationPlaces);
        }
    }
}
