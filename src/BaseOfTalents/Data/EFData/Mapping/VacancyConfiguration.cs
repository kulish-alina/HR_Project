using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFData.Mapping
{
    public class VacancyConfiguration : BaseEntityConfiguration<Vacancy>
    {
        public VacancyConfiguration()
        {
            HasMany(v => v.Level);
            //HasMany(c => c.Events).WithOptional(e => e.Vacancy);
            HasMany(v => v.Tags);
            HasMany(v => v.CandidatesProgress);
            HasMany(v => v.Files);
           
            HasOptional(v => v.ParentVacancy).WithOptionalDependent();
            HasOptional(v => v.Industry).WithOptionalDependent();

            HasRequired(v => v.Department).WithMany().HasForeignKey(v => v.DepartmentId);
            HasRequired(v => v.Responsible).WithMany().HasForeignKey(v => v.ResponsibleId);
            HasRequired(v => v.LanguageSkill).WithMany().HasForeignKey(v => v.LanguageSkillId);

            HasMany(v => v.Locations).WithMany().Map(x =>
            {
                x.MapRightKey("Location_Id");
                x.MapLeftKey("Vacancy_Id");
                x.ToTable("VacancyLocation");
            });

            HasMany(v => v.RequiredSkills).WithMany().Map(x =>
            {
                x.MapRightKey("Skill_Id");
                x.MapLeftKey("Vacancy_Id");
                x.ToTable("VacancySkill");
            });

            HasMany(c => c.Comments).WithMany().Map(x =>
            {
                x.MapRightKey("Comment_Id");
                x.MapLeftKey("Vacancy_Id");
                x.ToTable("VacancyComment");
            });

        }
    }
}
