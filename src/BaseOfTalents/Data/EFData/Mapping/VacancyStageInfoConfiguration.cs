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
            HasRequired(vsi => vsi.VacancyStage).WithRequiredDependent();
            HasOptional(vsi => vsi.Comment).WithOptionalDependent();
        }
    }
}
