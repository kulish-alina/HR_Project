using Domain.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class VacancyStage : BaseEntity
    {
        public virtual Vacancy Vacacny { get; set; }
        public virtual Stage Stage { get; set; }
        public int Order { get; set; }
        public bool IsCommentRequired { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
