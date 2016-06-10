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

            HasOptional(c => c.RelocationPlace).WithMany().HasForeignKey(x => x.RelocationPlaceId);

            HasOptional(c => c.Currency).WithMany().HasForeignKey(c => c.CurrencyId);

            HasRequired(c => c.Location).WithMany().HasForeignKey(c => c.LocationId);
            HasMany(c => c.Files);
            HasMany(c => c.VacanciesProgress).WithRequired(vs => vs.Candidate).HasForeignKey(vs => vs.CandidateId);

            HasMany(c => c.SocialNetworks).WithRequired().HasForeignKey(x => x.CandidateId);

            HasMany(c => c.LanguageSkills).WithMany().Map(x =>
            {
                x.MapRightKey("LanguageSkill_Id");
                x.MapLeftKey("Candidate_Id");
                x.ToTable("CandidateLanguageSkill");
            });

            HasMany(c => c.Comments).WithMany().Map(x =>
            {
                x.MapRightKey("Comment_Id");
                x.MapLeftKey("Candidate_Id");
                x.ToTable("CandidateComment");
            });
            HasMany(c => c.Sources);

            HasMany(v => v.Tags).WithMany().Map(x =>
            {
                x.MapRightKey("Tag_Id");
                x.MapLeftKey("Candidate_Id");
                x.ToTable("CandidateTag");
            });

            HasMany(c => c.PhoneNumbers).WithMany().Map(x =>
            {
                x.MapRightKey("PhoneNumber_Id");
                x.MapLeftKey("Candidate_Id");
                x.ToTable("CandidatePhoneNumber");
            });

            HasMany(v => v.Skills).WithMany().Map(x =>
            {
                x.MapRightKey("Skill_Id");
                x.MapLeftKey("Candidate_Id");
                x.ToTable("CandidateSkill");
            });
        }
    }
}