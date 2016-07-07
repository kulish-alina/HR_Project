using Domain.Entities.Enum.Setup;

namespace Domain.Entities
{
    public class RelocationPlace : BaseEntity
    {
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        public int? CityId { get; set; }
        public virtual City City { get; set; }
    }
}
