using System.Collections;
using System.Collections.Generic;

namespace BaseOfTalents.Domain.Entities.Enum.Setup
{
    public class Country : BaseEntity
    {
        public string Title { get; set; }

        public virtual ICollection<City> Cities { get; set; }
    }
}