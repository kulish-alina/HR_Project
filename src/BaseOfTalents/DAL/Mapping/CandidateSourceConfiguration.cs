using Domain.Entities;

namespace DAL.Mapping
{
    public class CandidateSourceConfiguration : BaseEntityConfiguration<CandidateSource>
    {
        public CandidateSourceConfiguration()
        {
            HasRequired(x => x.Source).WithMany().HasForeignKey(x => x.SourceId);
            Property(sn => sn.Path).IsRequired();

            HasRequired(x => x.Candidate).WithMany();
        }
    }
}