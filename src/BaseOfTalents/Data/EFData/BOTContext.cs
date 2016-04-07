using Domain.Entities;
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

        public DbSet<Candidate>  Candidates { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }

        public DbSet<City> Cities { get; set; }

        public BOTContext() : base()
        {
        }
    }
}
