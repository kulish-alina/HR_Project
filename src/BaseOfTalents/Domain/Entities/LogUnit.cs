using Domain.Entities.Enum;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class LogUnit : BaseEntity
    {
        public string Field { get; set; }
        public virtual ICollection<LogValue> PastValues { get; set; }
        public virtual ICollection<LogValue> NewValues { get; set; }
        public FieldType FieldType { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
    }
}
