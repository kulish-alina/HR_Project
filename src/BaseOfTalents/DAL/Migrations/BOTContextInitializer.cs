using Domain.Entities;
using Domain.Entities.Enum;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace DAL.Migrations
{
    public class BOTContextInitializer : DropCreateDatabaseIfModelChanges<BOTContext>
    {
        protected override void Seed(BOTContext context)
        {
            context.Tags.AddRange(DummyData.Tags);
            context.SaveChanges();
            context.Currencies.AddRange(DummyData.Currencies);
            context.SaveChanges();
            context.Skills.AddRange(DummyData.Skills);
            context.SaveChanges();
            context.EventTypes.AddRange(DummyData.EventTypes);
            context.SaveChanges();
            context.SocialNetworks.AddRange(DummyData.Socials);
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
            context.Cities.AddRange(DummyData.Cities);
            context.SaveChanges();
            context.Stages.AddRange(DummyData.Stages);
            context.SaveChanges();
            context.Permissions.AddRange(DummyData.Permissions);
            context.SaveChanges();
            context.Roles.AddRange(DummyData.Roles);
            context.SaveChanges();
            context.Users.AddRange(DummyData.Users);
            context.SaveChanges();
            context.Notes.AddRange(DummyData.Notes);
            context.SaveChanges();
            context.Vacancies.AddRange(DummyData.Vacancies);
            context.SaveChanges();
            context.Sources.AddRange(DummyData.Sources);
            context.SaveChanges();
            /*context.CandidateSources.AddRange(DummyData.CandidateSources);
            context.SaveChanges();*/
            context.Candidates.AddRange(DummyData.Candidates);
            context.SaveChanges();
            context.Events.AddRange(DummyData.Events);
            context.SaveChanges();
            var events = DummyData.Events.Take(10).ToList();
            foreach (var even in events)
            {
                even.Responsible = DummyData.Users[0];
                context.Events.AddOrUpdate(even);
            }
            context.Vacancies.ToList().ForEach(x =>
            {
                var candidate = context.Candidates.ToList().GetRandom();
                x.CandidatesProgress.Add(new VacancyStageInfo
                {
                    Candidate = candidate,
                    CandidateId = candidate.Id,
                    Comment = null,
                    StageId = 1,
                    StageState = StageState.Active,
                    Vacancy = x
                });
            });
            context.Candidates.ToList().ForEach(x =>
            {
                var vacancy = context.Vacancies.ToList().GetRandom();
                x.VacanciesProgress.Add(new VacancyStageInfo
                {
                    Candidate = x,
                    CandidateId = x.Id,
                    Comment = null,
                    StageId = 1,
                    StageState = StageState.Active,
                    Vacancy = vacancy
                });
                vacancy = context.Vacancies.ToList().GetRandom();
                x.VacanciesProgress.Add(new VacancyStageInfo
                {
                    Candidate = x,
                    CandidateId = x.Id,
                    Comment = null,
                    StageId = 1,
                    StageState = StageState.Active,
                    Vacancy = vacancy
                });
            });
            context.SaveChanges();
        }
    }
}
