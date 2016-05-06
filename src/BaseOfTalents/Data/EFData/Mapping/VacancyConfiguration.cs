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
            HasMany(v => v.CandidatesProgress);
            HasMany(v => v.Files);
           
            HasOptional(v => v.ParentVacancy).WithMany().HasForeignKey(x=>x.ParentVacancyId);
            HasOptional(v => v.Industry).WithMany().HasForeignKey(x=>x.IndustryId);

            HasRequired(v => v.Department).WithMany().HasForeignKey(v => v.DepartmentId);
            HasRequired(v => v.Responsible).WithMany().HasForeignKey(v => v.ResponsibleId);

            HasOptional(v => v.LanguageSkill).WithOptionalDependent();

            HasMany(v => v.Locations).WithMany().Map(x =>
            {
                x.MapRightKey("Location_Id");
                x.MapLeftKey("Vacancy_Id");
                x.ToTable("VacancyLocation");
            });

            HasMany(v => v.Tags).WithMany().Map(x=>
            {
                x.MapRightKey("Tag_Id");
                x.MapLeftKey("Vacancy_Id");
                x.ToTable("VacancyTag");
            });

            HasMany(v => v.Levels).WithMany().Map(x => 
            {
                x.MapRightKey("Level_Id");
                x.MapLeftKey("Vacancy_Id");
                x.ToTable("VacancyLevel");
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
