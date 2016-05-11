using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFData.Mapping
{
    public class VacancyStageConfiguration : BaseEntityConfiguration<VacancyStage>
    {
        public VacancyStageConfiguration()
        {
            Property(vs => vs.Order).IsRequired();
            Property(vs => vs.IsCommentRequired).IsRequired();
            HasRequired(vs => vs.Vacacny).WithMany().HasForeignKey(vs=>vs.VacancyId);
            HasRequired(vs => vs.Stage).WithMany().HasForeignKey(vs => vs.StageId);

        }
    }
}
