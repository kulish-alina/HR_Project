namespace Data.Migrations
{
    using Domain.Entities;
    using Domain.Entities.Enum;
    using Domain.Entities.Enum.Setup;
    using Domain.Entities.Setup;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Data.EFData.BOTContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Data.EFData.BOTContext context)
        {


            context.Skills.AddRange(DummyData.Skills);
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



            User user = new User()
            {
                LocationId = 1,
                RoleId = 1,
                BirthDate = DateTime.Now,
                Email = "email",
                FirstName = "fname",
                isMale = true,
                LastName = "lastname",
                Login = "login",
                Password = "pass",
                MiddleName = "mname",
                PhoneNumbers = new List<PhoneNumber>() { new PhoneNumber() { Number = "+3565234662" } },
                Skype = "skype",
                Photo = photo
            };

            context.Users.Add(user);
            context.SaveChanges();

            Tag tag = new Tag()
            {
                Title = "tag"
            };
            context.Tags.Add(tag);
            context.SaveChanges();

            Vacancy vacancy = new Vacancy()
            {
                ResponsibleId = 1,
                DepartmentId = 1,
                IndustryId = 1,
                ParentVacancyId = null,
                TypeOfEmployment = TypeOfEmployment.FullTime,
                Title = "Architecht",
                Comments = new List<Comment>() { },
                DeadlineDate = DateTime.Now,
                Description = "descr",
                EndDate = DateTime.Now,
                Files = new List<File>() { },
                LanguageSkill = new LanguageSkill() { LanguageId = 1, LanguageLevel = LanguageLevel.Fluent },
                Levels = new List<Level>() { context.Levels.First() },
                Locations = new List<Location> { context.Locations.First() },
                RequiredSkills = new List<Skill>() { context.Skills.First() },
                SalaryMax = 100500,
                SalaryMin = 15,
                StartDate = DateTime.Now,
                CandidatesProgress = new List<VacancyStageInfo>(),
                Tags = new List<Tag>() { context.Tags.First() },
            };

            context.Vacancies.Add(vacancy);
            context.SaveChanges();

            Comment vacancycomment = new Comment()
            {
                Message = "good vacancy"
            };

            File file = new File()
            {
                FilePath = "path",
                Description = "descri"
            };

            var vacancyDb = context.Vacancies.First();
            vacancyDb.Comments.Add(vacancycomment);
            vacancyDb.Files.Add(file);
            context.Entry<Vacancy>(vacancyDb).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            Candidate candidate = new Candidate()
            {
                LocationId = 1,
                BirthDate = DateTime.Now,
                Comments = new List<Comment> { },
                Education = "Good",
                FirstName = "Jonny",
                IndustryId = 1,
                Description = "candidate is candi",
                Email = "killer666@mayl.op",
                Files = new List<File>() { },
                IsMale = true,
                LanguageSkills = new List<LanguageSkill>() { new LanguageSkill() { LanguageId = 1, LanguageLevel = LanguageLevel.Advanced } },
                LastName = "Yehayy",
                MiddleName = "Caro",
                PhoneNumbers = new List<PhoneNumber>() { },
                Photo = new Photo() { Description = "desc", ImagePath = "path" },
                PositionDesired = "Pos",
                Practice = "Good",
                RelocationAgreement = true,
                SalaryDesired = 500,
                Skills = new List<Skill>() { context.Skills.First() },
                Skype = "skyper133",
                SocialNetworks = new List<CandidateSocial>() { new CandidateSocial() { Path = "path", SocialNetwork = new SocialNetwork() { ImagePath = "imgPath", Title = "title" } } },
                Sources = new List<CandidateSource>() { new CandidateSource() { Source = Source.WorkUa, Path = "Path" } },
                StartExperience = DateTime.Now,
                Tags = new List<Tag>() { context.Tags.First() },
                TypeOfEmployment = TypeOfEmployment.FullTime,
                VacanciesProgress = new List<VacancyStageInfo>() { }
            };

            context.Candidates.Add(candidate);
            context.SaveChanges();

            Comment comment = new Comment()
            {
                Message = "Good paren'",
            };

            var candidateDb = context.Candidates.First();
            candidateDb.Comments.Add(comment);
            context.Entry<Candidate>(candidateDb).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            base.Seed(context);
        }


    }
}