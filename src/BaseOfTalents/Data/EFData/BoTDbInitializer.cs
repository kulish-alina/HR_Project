using System;
using System.Collections.Generic;
using System.Data.Entity;
using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Setup;

namespace Data.EFData
{
    public class BoTDbInitializer : DropCreateDatabaseAlways<BOTContext>
    {
        protected override void Seed(BOTContext context)
        {
            List<Country> countries = new List<Country>()
            {
                new Country { EditTime=DateTime.Now, Name="Ukraine" }
            };

            List<City> cities = new List<City>()
            {
                new City { EditTime = DateTime.Now, Country=countries[0], Name="Kiev" },
                new City { EditTime = DateTime.Now, Country=countries[0], Name="Kharkiv" },
                new City { EditTime = DateTime.Now, Country=countries[0], Name="Odessa" },
                new City { EditTime = DateTime.Now, Country=countries[0], Name="Dnipropetrovsk" },
                new City { EditTime = DateTime.Now, Country=countries[0], Name="Zaporizhia" },
                new City { EditTime = DateTime.Now, Country=countries[0], Name="Lviv" },
                new City { EditTime = DateTime.Now, Country=countries[0], Name="Kryvyi Rih" },
                new City { EditTime = DateTime.Now, Country=countries[0], Name="Mykolaiv" },
                new City { EditTime = DateTime.Now, Country=countries[0], Name="Mariupol" },
                new City { EditTime = DateTime.Now, Country=countries[0], Name="Luhansk" },
                new City { EditTime = DateTime.Now, Country=countries[0], Name="Donetsk" },
                new City { EditTime = DateTime.Now, Country=countries[0], Name="Sevastopol" },
                new City { EditTime = DateTime.Now, Country=countries[0], Name="Vinnytsia" },
                new City { EditTime = DateTime.Now, Country=countries[0], Name="Makiivka" },
                new City { EditTime = DateTime.Now, Country=countries[0], Name="Simferopol" },
                new City { EditTime = DateTime.Now, Country=countries[0], Name="Kherson" },
                new City { EditTime = DateTime.Now, Country=countries[0], Name="Poltava" },
                new City { EditTime = DateTime.Now, Country=countries[0], Name="Chernihiv" },
            };
            List<Stage> stages = new List<Stage>()
            {
                new Stage { EditTime = DateTime.Now, Title="Pool" },
                new Stage { EditTime = DateTime.Now, Title="Selected" },
                new Stage { EditTime = DateTime.Now, Title="HR Interview" },
                new Stage { EditTime = DateTime.Now, Title="Test task" },
                new Stage { EditTime = DateTime.Now, Title="Tech Interview" },
                new Stage { EditTime = DateTime.Now, Title="Additional interview" },
                new Stage { EditTime = DateTime.Now, Title="Final Interview",  },
                new Stage { EditTime = DateTime.Now, Title="Job Offer Issued" },
                new Stage { EditTime = DateTime.Now, Title="Job Offer Accepted", },
                new Stage { EditTime = DateTime.Now, Title="Hired" },
                new Stage { EditTime = DateTime.Now, Title="Rejected" },
            };

            #region Candidate
            Comment candidateComment = new Comment()
            {
                CommentType = CommentType.Candidate,
                Message = "msg",
                RelativeId = 0,
            };

            Experience experience = new Experience()
            {
                WorkExperience = new TimeSpan(1),
            };

            File candidateFile = new File()
            {
                Description = "description",
                FilePath = "path",
            };

            CandidateSource candidateSource = new CandidateSource()
            {
                Path = "Path",
                Source = Source.HeadHunter,
            };

            Language language = new Language()
            {
                Title = "language"
            };

            LanguageSkill languageSkill = new LanguageSkill()
            {
                Language = language,
                LanguageLevel = LanguageLevel.Fluent,
            };

            Country country = new Country()
            {
                Name = "name"
            };

            City city = new City()
            {
                Country = country,
                Name = "dnepr"
            };

            Photo photo = new Photo()
            {
                Description = "descr",
                ImagePath = "path"
            };

            Skill skill = new Skill()
            {
                Title = "C#"
            };

            SocialNetwork socialNetwork = new SocialNetwork()
            {
                ImagePath = "path",
                Title = "Path"
            };

            CandidateSocial candidateSocial = new CandidateSocial()
            {
                Path = "path",
                SocialNetwork = socialNetwork,
            };

            Candidate candidate = new Candidate()
            {
                Skype = "skype",
                BirthDate = DateTime.Now,
                Comments = new List<Comment>() { candidateComment },
                Description = "descrpition",
                Education = "High",
                Email = "email",
                Experience = experience,
                Files = new List<File>() { candidateFile },
                Sources = new List<CandidateSource>() { candidateSource },
                FirstName = "fname",
                IsMale = true,
                LanguageSkills = new List<LanguageSkill>() { languageSkill },
                LastName = "lname",
                City = city,
                MiddleName = "mname",
                PhoneNumbers = new List<string>() { "+380978762352" },
                Photo = photo,
                PositionDesired = "architecht",
                Practice = "best",
                RelocationAgreement = true,
                SalaryDesired = 10500,
                Skills = new List<Skill>() { skill },
                SocialNetworks = new List<CandidateSocial>() { candidateSocial },
                TypeOfEmployment = TypeOfEmployment.FullTime,
                VacanciesProgress = new List<VacancyStageInfo>() { }
            };
            #endregion

            Comment vacancyComment = new Comment()
            {
                CommentType = CommentType.Vacancy,
                Message = "msg",
                RelativeId = 0,
            };

            File vacancyFile = new File()
            {
                Description = "file",
                FilePath = "path",
            };

            Permission permission = new Permission()
            {
                AccessRights = AccessRights.AddCandidateToVacancy,
                Description = "Permis",
                Role = null
            };

            Role role = new Role()
            {
                Name = "adm",
                Permissions = new List<Permission>() { permission },
            };

            User user = new User()
            {
                BirthDate = DateTime.Now,
                Email = "mail",
                FirstName = "fname",
                isMale = true,
                LastName = "lastname",
                City = city,
                Login = "login",
                Password = "pass",
                MiddleName = "mname",
                PhoneNumbers = new List<string>() { "+3565234662" },
                Photo = photo,
                Role = role,
                Skype = "skype",
            };

            Department department = new Department()
            {
                Title = "title"
            };

            Team team = new Team()
            {
                Department = department,
                Title = "title"
            };

            Vacancy vacancy = new Vacancy()
            {
                TypeOfEmployment = TypeOfEmployment.FullTime,
                Title = "Architecht",
                Comments = new List<Comment>() { vacancyComment },
                DeadlineDate = DateTime.Now,
                Description = "descr",
                EndDate = DateTime.Now,
                Files = new List<File>() { vacancyFile },
                LanguageSkill = languageSkill,
                Level = Level.Senior,
                City = city,
                ParentVacancy = null,
                RequiredSkills = new List<Skill>() { skill },
                Responsible = user,
                SalaryMax = 100500,
                SalaryMin = 15,
                StartDate = DateTime.Now,
                Team = team,
                CandidatesProgress = new List<VacancyStageInfo>()

            };

            Comment vsicomment = new Comment()
            {
                CommentType = CommentType.StageInfo,
                Message = "msg",
                RelativeId = 0,
            };

            Stage stage = new Stage()
            {
                Title = "pool"
            };

            VacancyStage vs = new VacancyStage()
            {
                IsCommentRequired = true,
                Order = 1,
                Stage = stage,
                Vacacny = vacancy
            };

            VacancyStageInfo vsi = new VacancyStageInfo()
            {
                Candidate = candidate,
                Comment = vsicomment,
                VacancyStage = vs
            };

            candidate.VacanciesProgress.Add(vsi);
            vacancy.CandidatesProgress.Add(vsi);
            context.Vacancies.Add(vacancy);
            context.Candidates.Add(candidate);

            context.Cities.AddRange(cities);
            context.Countries.AddRange(countries);
            context.Stages.AddRange(stages);

            context.SaveChanges();
            base.Seed(context);
        }
    }
}


