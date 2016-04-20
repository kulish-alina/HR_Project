//using Data.Migrations;
using Domain.Entities;
using Domain.Entities.Enum.Setup;
using Domain.Entities.Setup;
using System.Data.Entity;
using System.Diagnostics;
using System.Reflection;

namespace Data.EFData
{
    public class BOTContext : DbContext
    {
        public DbSet<Language> Languages { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentGroup> DepartmentGroups { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }

        public DbSet<Location> Locations { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Stage> Stages { get; set; }


        public BOTContext() : base()
        {
            //this.Configuration.LazyLoadingEnabled = false;
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<BOTContext, Configuration>("BOTContext"));
        }
    }
}
