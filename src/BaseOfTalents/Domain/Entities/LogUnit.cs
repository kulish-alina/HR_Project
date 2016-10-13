using Domain.Entities.Enum;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class LogUnit : BaseEntity
    {
        public virtual User User { get; set; }
        public int UserId { get; set; }
        public string Field { get; set; }
        public virtual ICollection<LogValue> Values { get; set; }
        public FieldType FieldType { get; set; }
    }
}
