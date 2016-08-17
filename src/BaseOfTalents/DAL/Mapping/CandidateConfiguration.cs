using Domain.Entities;

namespace DAL.Mapping
{
    public class CandidateConfiguration : BaseEntityConfiguration<Candidate>
    {
        public CandidateConfiguration()
        {
            Property(c => c.FirstName).IsRequired();
            Property(c => c.LastName).IsRequired();
            HasRequired(c => c.Industry).WithMany().HasForeignKey(x => x.IndustryId);

            Property(c => c.Skype).IsOptional();
            Property(c => c.PositionDesired).IsOptional();
            Property(c => c.IsMale).IsOptional();

            HasMany(c => c.RelocationPlaces).WithMany().Map(x =>
            {
                x.MapRightKey("RelocationPlaceId");
                x.MapLeftKey("CandidateId");
                x.ToTable("CandidateToRelocationPlace");
            });

            HasMany(x => x.ClosedVacancies).WithOptional(x => x.ClosingCandidate).HasForeignKey(x => x.ClosingCandidateId);

            HasOptional(c => c.Currency).WithMany().HasForeignKey(c => c.CurrencyId);

            HasOptional(c => c.City).WithMany().HasForeignKey(c => c.CityId);
            HasMany(v => v.Files).WithMany().Map(x =>
            {
                x.MapRightKey("FileId");
                x.MapLeftKey("CandidateId");
                x.ToTable("FileToCandidate");
            });

            HasMany(c => c.VacanciesProgress).WithRequired().HasForeignKey(x => x.CandidateId);

            HasMany(x => x.Events).WithOptional(x => x.Candidate).HasForeignKey(x => x.CandidateId);

            HasMany(c => c.SocialNetworks).WithRequired();

            HasMany(c => c.LanguageSkills).WithMany().Map(x =>
            {
                x.MapRightKey("LanguageSkillId");
                x.MapLeftKey("CandidateId");
                x.ToTable("CandidateToLanguageSkill");
            });

            HasMany(c => c.Comments).WithMany().Map(x =>
            {
                x.MapRightKey("CommentId");
                x.MapLeftKey("CandidateId");
                x.ToTable("CandidateToComment");
            });
            HasMany(c => c.Sources).WithRequired(x => x.Candidate);
            HasOptional(x => x.MainSource).WithMany().HasForeignKey(x => x.MainSourceId);

            HasMany(v => v.Tags).WithMany().Map(x =>
            {
                x.MapRightKey("TagId");
                x.MapLeftKey("CandidateId");
                x.ToTable("CandidateToTag");
            });

            HasMany(c => c.PhoneNumbers).WithMany().Map(x =>
            {
                x.MapRightKey("PhoneNumberId");
                x.MapLeftKey("CandidateId");
                x.ToTable("CandidateToPhoneNumber");
            });

            HasMany(v => v.Skills).WithMany().Map(x =>
            {
                x.MapRightKey("SkillId");
                x.MapLeftKey("CandidateId");
                x.ToTable("CandidateToSkill");
            });
        }
    }
}