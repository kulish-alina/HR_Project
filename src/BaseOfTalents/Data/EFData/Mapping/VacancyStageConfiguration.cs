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
            HasRequired(vs => vs.Vacacny).WithRequiredDependent();
            HasRequired(vs => vs.Stage).WithRequiredDependent();

            HasMany(c => c.Comments).WithMany().Map(x =>
            {
                x.MapRightKey("Comment_Id");
                x.MapLeftKey("VacancyStage_Id");
                x.ToTable("VacancyStageComment");
            });
        }
    }
}
