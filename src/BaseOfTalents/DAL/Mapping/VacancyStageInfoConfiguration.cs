using Domain.Entities;

namespace DAL.Mapping
{
    public class VacancyStageInfoConfiguration : BaseEntityConfiguration<VacancyStageInfo>
    {
        public VacancyStageInfoConfiguration()
        {
            HasRequired(x => x.Stage).WithMany().HasForeignKey(x => x.StageId);
            HasOptional(x => x.Comment);
            HasRequired(vsi => vsi.Candidate).WithMany().HasForeignKey(vsi => vsi.CandidateId);
            HasRequired(vsi => vsi.Vacancy).WithMany().HasForeignKey(vsi => vsi.VacancyId);
        }
    }
}