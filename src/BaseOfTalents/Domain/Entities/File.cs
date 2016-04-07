using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Domain.Entities
{
    public class File : BaseEntity
    {
        public string FilePath { get; set; }
        public string Description { get; set; }
    }
}
