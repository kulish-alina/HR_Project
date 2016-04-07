using Domain.Entities.Setup;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class StageInfo : BaseEntity
    {
        public Stage Stage { get; set; }
        public virtual Comment Comment { get; set; }
    }
}
