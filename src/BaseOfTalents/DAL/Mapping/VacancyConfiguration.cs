using Domain.Entities;

namespace DAL.Mapping
{
    public class VacancyConfiguration : BaseEntityConfiguration<Vacancy>
    {
        public VacancyConfiguration()
        {
            Property(v => v.Title).IsRequired();
            Property(v => v.State).IsRequired();
            HasMany(x => x.StageFlow).WithOptional();

            HasMany(c => c.CandidatesProgress).WithRequired().HasForeignKey(x => x.VacancyId);

            HasMany(v => v.Files).WithMany().Map(x =>
            {
                x.MapRightKey("FileId");
                x.MapLeftKey("VacancyId");
                x.ToTable("FileToVacancy");
            });

            HasMany(x => x.History).WithMany().Map(x =>
            {
                x.MapRightKey("LogUnitId");
                x.MapLeftKey("VacancyId");
                x.ToTable("LogUnitToVacancy");
            });
            HasOptional(x => x.ClosingCandidate).WithMany(x => x.ClosedVacancies).HasForeignKey(x => x.ClosingCandidateId);

            HasOptional(v => v.LanguageSkill);
            HasOptional(v => v.ParentVacancy).WithMany().HasForeignKey(x => x.ParentVacancyId);

            HasRequired(v => v.Industry).WithMany().HasForeignKey(x => x.IndustryId);
            HasRequired(v => v.Department).WithMany().HasForeignKey(v => v.DepartmentId);
            HasRequired(v => v.Responsible).WithMany().HasForeignKey(v => v.ResponsibleId);
            HasMany(v => v.CandidatesProgress).WithRequired().HasForeignKey(vsi => vsi.VacancyId);
            HasMany(c => c.StatesInfo).WithRequired(x => x.Vacancy).HasForeignKey(x => x.VacancyId);

            HasMany(v => v.Cities).WithMany().Map(x =>
            {
                x.MapRightKey("CityId");
                x.MapLeftKey("VacancyId");
                x.ToTable("VacancyToCity");
            });

            HasMany(v => v.Tags).WithMany().Map(x =>
            {
                x.MapRightKey("TagId");
                x.MapLeftKey("VacancyId");
                x.ToTable("VacancyToTag");
            });

            HasMany(v => v.Levels).WithMany().Map(x =>
            {
                x.MapRightKey("LevelId");
                x.MapLeftKey("VacancyId");
                x.ToTable("VacancyToLevel");
            });

            HasMany(v => v.RequiredSkills).WithMany().Map(x =>
            {
                x.MapRightKey("SkillId");
                x.MapLeftKey("VacancyId");
                x.ToTable("VacancyToSkill");
            });

            HasMany(c => c.Comments).WithMany().Map(x =>
            {
                x.MapRightKey("CommentId");
                x.MapLeftKey("VacancyId");
                x.ToTable("VacancyToComment");
            });
        }
    }
}