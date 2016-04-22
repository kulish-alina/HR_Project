using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.ModelConfiguration;


namespace Data.EFData.Mapping
{
    public class CandidateConfiguration : EntityTypeConfiguration<Candidate>
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


            HasOptional(c => c.Photo).WithOptionalDependent();
            HasOptional(c => c.Industry).WithOptionalDependent();

            HasRequired(c => c.Location).WithMany().HasForeignKey(c => c.LocationId);
            HasMany(c => c.SocialNetworks);
            HasMany(c => c.Files);
            //HasMany(c => c.Events).WithOptional(e => e.Candidate);
            HasMany(c => c.VacanciesProgress).WithRequired(vs => vs.Candidate).HasForeignKey(vs => vs.CandidateId);

            HasMany(c => c.LanguageSkills).WithMany().Map(x=> 
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
            HasMany(c => c.Sources);//.WithRequired(cs => cs.Candidate).HasForeignKey(cs => cs.CandidateId);
            HasMany(c => c.Tags);
            HasMany(c => c.PhoneNumbers).WithMany().Map(x=> 
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
