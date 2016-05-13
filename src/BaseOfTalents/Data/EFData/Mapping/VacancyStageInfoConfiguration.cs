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
            HasRequired(vsi => vsi.VacancyStage).WithMany();
            HasOptional(vsi => vsi.Comment).WithOptionalDependent();

            HasRequired(vsi => vsi.Candidate).WithMany().HasForeignKey(vsi => vsi.CandidateId);
            HasRequired(vsi => vsi.Vacancy).WithMany().HasForeignKey(vsi => vsi.VacancyId);
        }
    }
}
