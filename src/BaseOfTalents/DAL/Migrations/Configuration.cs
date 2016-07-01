using System.Data.Entity.Migrations;
using System.Linq;

namespace BaseOfTalents.DAL.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<BOTContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BOTContext context)
        {
            if (!context.Tags.Any())
            {
                context.Tags.AddRange(DummyData.Tags);
                context.SaveChanges();
            }
            if (!context.Currencies.Any())
            {
                context.Currencies.AddRange(DummyData.Currencies);
                context.SaveChanges();
            }
            if (!context.Skills.Any())
            {
                context.Skills.AddRange(DummyData.Skills);
                context.SaveChanges();
            }
            if (!context.EventTypes.Any())
            {
                context.EventTypes.AddRange(DummyData.EventTypes);
                context.SaveChanges();
            }
            if (!context.SocialNetworks.Any())
            {
                context.SocialNetworks.AddRange(DummyData.Socials);
                context.SaveChanges();
            }
            if (!context.Industries.Any())
            {
                context.Industries.AddRange(DummyData.Industries);
                context.SaveChanges();
            }
            if (context.Levels.Any())
            {
                context.Levels.AddRange(DummyData.Levels);
                context.SaveChanges();
            }
            if (!context.DepartmentGroups.Any())
            {
                context.DepartmentGroups.AddRange(DummyData.DepartmentGroups);
                context.SaveChanges();
            }
            if (!context.Departments.Any())
            {
                context.Departments.AddRange(DummyData.Departments);
                context.SaveChanges();
            }
            if (!context.Languages.Any())
            {
                context.Languages.AddRange(DummyData.Languages);
                context.SaveChanges();
            }
            if (!context.LanguageSkills.Any())
            {
                context.LanguageSkills.AddRange(DummyData.LanguageSkills);
                context.SaveChanges();
            }
            if (!context.Countries.Any())
            {
                context.Countries.AddRange(DummyData.Countries);
                context.SaveChanges();
            }
            if (!context.Cities.Any())
            {
                context.Cities.AddRange(DummyData.Cities);
                context.SaveChanges();
            }
            if (!context.Stages.Any())
            {
                context.Stages.AddRange(DummyData.Stages);
                context.SaveChanges();
            }
            if (!context.Permissions.Any())
            {
                context.Permissions.AddRange(DummyData.Permissions);
                context.SaveChanges();
            }
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(DummyData.Roles);
                context.SaveChanges();
            }
            if (!context.Users.Any())
            {
                context.Users.AddRange(DummyData.Users);
                context.SaveChanges();
            }
            if (!context.Notes.Any())
            {
                context.Notes.AddRange(DummyData.Notes);
                context.SaveChanges();
            }
            if (!context.Vacancies.Any())
            {
                context.Vacancies.AddRange(DummyData.Vacancies);
                context.SaveChanges();
            }
            if (!context.Sources.Any())
            {
                context.Sources.AddRange(DummyData.Sources);
                context.SaveChanges();
            }
            if (!context.CandidateSources.Any())
            {
                context.CandidateSources.AddRange(DummyData.CandidateSources);
                context.SaveChanges();
            }
            if (!context.Candidates.Any())
            {
                context.Candidates.AddRange(DummyData.Candidates);
                context.SaveChanges();
            }
            if (context.Events.Any())
            {
                context.Events.AddRange(DummyData.Events);
                context.SaveChanges();
            }
            var events = DummyData.Events.Take(10).ToList();
            foreach (var even in events)
            {
                even.Responsible = DummyData.Users[0];
                context.Events.AddOrUpdate(even);
            }
            context.SaveChanges();
        }
    }
}