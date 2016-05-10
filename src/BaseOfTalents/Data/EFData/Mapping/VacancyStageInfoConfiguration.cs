using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFData.Mapping
{
    public class VacancyStageInfoConfiguration : BaseEntityConfiguration<VacancyStageInfo>
    {
        public VacancyStageInfoConfiguration()
        {
            Map(m => m.Requires("IsDeleted").HasValue(false)).Ignore(m => m.IsDeleted);

            HasRequired(vsi => vsi.VacancyStage).WithRequiredDependent();
            HasOptional(vsi => vsi.Comment).WithOptionalDependent();

            HasRequired(vsi => vsi.Candidate).WithMany().HasForeignKey(vsi => vsi.CandidateId);
        }
    }
}
