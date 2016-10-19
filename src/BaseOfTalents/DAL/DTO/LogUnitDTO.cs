using Domain.Entities.Enum;
using System.Collections.Generic;

namespace DAL.DTO
{
    public class LogUnitDTO : BaseEntityDTO
    {
        public UserDTO User { get; set; }
        public int UserId { get; set; }
        public string Field { get; set; }
        public IEnumerable<string> NewValues { get; set; }
        public IEnumerable<string> PastValues { get; set; }
        public FieldType FieldType { get; set; }
    }
}
