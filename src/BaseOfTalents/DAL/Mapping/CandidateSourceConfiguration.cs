using BaseOfTalents.Domain.Entities;

namespace BaseOfTalents.DAL.Mapping
{
    public class CandidateSourceConfiguration : BaseEntityConfiguration<CandidateSource>
    {
        public CandidateSourceConfiguration()
        {
            HasRequired(x => x.Source).WithMany().HasForeignKey(x => x.SourceId);
            Property(sn => sn.Path).IsRequired();
        }
    }
}