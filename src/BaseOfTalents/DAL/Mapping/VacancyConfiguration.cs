using BaseOfTalents.Domain.Entities;

namespace BaseOfTalents.DAL.Mapping
{
    public class VacancyConfiguration : BaseEntityConfiguration<Vacancy>
    {
        public VacancyConfiguration()
        {
            HasMany(v => v.CandidatesProgress);
            HasMany(v => v.Files);

            HasOptional(v => v.ParentVacancy).WithMany(v => v.ChildVacancies).HasForeignKey(v => v.ParentVacancyId);
            HasOptional(v => v.LanguageSkill);
            HasOptional(v => v.ParentVacancy).WithMany().HasForeignKey(x => x.ParentVacancyId);
            HasOptional(v => v.Industry).WithMany().HasForeignKey(x => x.IndustryId);

            HasRequired(v => v.Department).WithMany().HasForeignKey(v => v.DepartmentId);
            HasRequired(v => v.Responsible).WithMany().HasForeignKey(v => v.ResponsibleId);

            HasMany(v => v.CandidatesProgress).WithRequired().HasForeignKey(vsi => vsi.VacancyId);
            HasMany(v => v.ChildVacancies).WithMany().Map(x =>
            {
                x.MapRightKey("ChildVacancyId");
                x.MapLeftKey("ParentVacancyId");
                x.ToTable("ParentVacancyToChildVacancy");
            });

            HasMany(v => v.Cities).WithMany().Map(x =>
            {
                x.MapRightKey("LocationId");
                x.MapLeftKey("VacancyId");
                x.ToTable("VacancyToLocation");
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