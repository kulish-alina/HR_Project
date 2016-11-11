using System.Collections.Generic;

namespace Domain.Entities.Enum.Setup
{
    public class City : BaseEntity
    {
        public string Title { get; set; }

        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        public bool HasOffice { get; set; }

        public virtual ICollection<RelocationPlace> RelocationPlaces { get; set; }
    }
}