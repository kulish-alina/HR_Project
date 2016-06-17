using BaseOfTalents.Domain.Entities;

namespace BaseOfTalents.DAL.Mapping
{
    public class CandidateConfiguration : BaseEntityConfiguration<Candidate>
    {
        public CandidateConfiguration()
        {
            Property(c => c.FirstName).IsRequired();
            Property(c => c.MiddleName).IsRequired();
            Property(c => c.LastName).IsRequired();
            Property(c => c.IsMale).IsRequired();
            Property(c => c.Email).IsRequired();

            Property(c => c.Skype).IsOptional();
            Property(c => c.PositionDesired).IsRequired();
            Property(c => c.IsMale).IsRequired();

            HasOptional(c => c.Industry).WithMany().HasForeignKey(x => x.IndustryId);

            HasMany(c => c.RelocationPlaces).WithRequired();

            HasOptional(c => c.Currency).WithMany().HasForeignKey(c => c.CurrencyId);

            HasRequired(c => c.City).WithMany().HasForeignKey(c => c.CityId);
            HasMany(c => c.Files);
            HasMany(c => c.VacanciesProgress).WithRequired(vs => vs.Candidate).HasForeignKey(vs => vs.CandidateId);

            HasMany(x => x.Events).WithOptional(x => x.Candidate).HasForeignKey(x => x.CandidateId);

            HasMany(c => c.SocialNetworks).WithRequired().HasForeignKey(x => x.CandidateId);

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
            HasMany(c => c.Sources);

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