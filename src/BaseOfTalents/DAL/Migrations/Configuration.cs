using System.Data.Entity.Migrations;

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
            context.Tags.AddRange(DummyData.Tags);
            context.SaveChanges();

            context.Skills.AddRange(DummyData.Skills);
            context.SaveChanges();

            context.EventTypes.AddRange(DummyData.Events);
            context.SaveChanges();

            context.SocialNetworks.AddRange(DummyData.Socials);
            context.SaveChanges();

            context.Tags.AddRange(DummyData.Tags);
            context.SaveChanges();

            context.Industries.AddRange(DummyData.Industries);
            context.SaveChanges();

            context.Levels.AddRange(DummyData.Levels);
            context.SaveChanges();

            context.DepartmentGroups.AddRange(DummyData.DepartmentGroups);
            context.SaveChanges();

            context.Departments.AddRange(DummyData.Departments);
            context.SaveChanges();

            context.Languages.AddRange(DummyData.Languages);
            context.SaveChanges();

            context.LanguageSkills.AddRange(DummyData.LanguageSkills);
            context.SaveChanges();

            context.Countries.AddRange(DummyData.Countries);
            context.SaveChanges();

            context.Locations.AddRange(DummyData.Locations);
            context.SaveChanges();

            context.Stages.AddRange(DummyData.Stages);
            context.SaveChanges();

            context.Permissions.AddRange(DummyData.Permissions);
            context.SaveChanges();

            context.Roles.AddRange(DummyData.Roles);
            context.SaveChanges();

            context.Users.AddRange(DummyData.Users);
            context.SaveChanges();

            context.Vacancies.AddRange(DummyData.Vacancies);
            context.SaveChanges();

            context.CandidateSources.AddRange(DummyData.CandidateSources);
            context.SaveChanges();

            context.Candidates.AddRange(DummyData.Candidates);
            context.SaveChanges();
        }
    }
}