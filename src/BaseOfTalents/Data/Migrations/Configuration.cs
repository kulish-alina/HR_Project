namespace Data.Migrations
{
    using Domain.Entities;
    using Domain.Entities.Enum;
    using Domain.Entities.Enum.Setup;
    using Domain.Entities.Setup;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
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
            List<Skill> skills = new List<Skill>()
            {
                new Skill { Title="SQL" },
                new Skill { Title="WinForms" },
                new Skill { Title="DevExpress" },
                new Skill { Title=".Net" },
                new Skill { Title="C#" },
                new Skill { Title="Spring .Net" },
                new Skill { Title="JQuery" },
                new Skill { Title="JavaScript" },
                new Skill { Title="ASP .NET MVC" },
                new Skill { Title="HTML5+CSS3" },
                new Skill { Title="NHibernate" },
                new Skill { Title="Entity Framework" }
            };

            context.Skills.AddRange(skills);
            context.SaveChanges();

            List<Tag> tags = new List<Tag>()
            {
                new Tag { Title="SQL" },
                new Tag { Title="WinForms" },
                new Tag { Title="DevExpress" },
                new Tag { Title=".Net" },
                new Tag { Title="C#" },
                new Tag { Title="Spring .Net" },
                new Tag { Title="JQuery" },
                new Tag { Title="JavaScript" },
                new Tag { Title="ASP .NET MVC" },
                new Tag { Title="HTML5+CSS3" },
                new Tag { Title="NHibernate" },
                new Tag { Title="Entity Framework" }
            };
            context.Tags.AddRange(tags);
            context.SaveChanges();

            List<Industry> industries = new List<Industry>()
            {
                new Industry { Title="IT" },
                new Industry { Title="Accounting" },
                new Industry { Title="Administration" }
            };
            context.Industries.AddRange(industries);
            context.SaveChanges();

            List<Level> levels = new List<Level>()
            {
                new Level { Title="Trainee" },
                new Level { Title="Junior" },
                new Level { Title="Middle" },
                new Level { Title="Senior" }
            };
            context.Levels.AddRange(levels);
            context.SaveChanges();

            List<Role> roles = new List<Role>()
            {
                new Role { Title="Manager" },
                new Role { Title="Recruiter" },
                new Role { Title="Freelancer" }
            };
            context.Roles.AddRange(roles);
            context.SaveChanges();

            List<DepartmentGroup> departmentGroups = new List<DepartmentGroup>()
            {
                new DepartmentGroup { Title="Contract" },
                new DepartmentGroup { Title="Nonprod" },
                new DepartmentGroup { Title="Prod" }
            };
            context.DepartmentGroups.AddRange(departmentGroups);
            context.SaveChanges();

            List<Department> departments = new List<Department>()
            {
                  new Department { Title = "Contract Programming",  DepartmentGroup = departmentGroups[0]},
                  new Department { Title = "Sites Design",          DepartmentGroup = departmentGroups[0]},
                  new Department { Title = "Web apps",              DepartmentGroup = departmentGroups[0]},
                  new Department { Title = "Accounting",            DepartmentGroup = departmentGroups[1]},
                  new Department { Title = "Administration",        DepartmentGroup = departmentGroups[1]},
                  new Department { Title = "Executives",            DepartmentGroup = departmentGroups[1]},
                  new Department { Title = "DevWorkshop",           DepartmentGroup = departmentGroups[1]},
                  new Department { Title = "HR",                    DepartmentGroup = departmentGroups[1]},
                  new Department { Title = "Managers",              DepartmentGroup = departmentGroups[1]},
                  new Department { Title = "Managers",              DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "QA",                    DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "UPM",                   DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "EPE",                   DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "Soft Web",              DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "AR",                    DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "Bank/Donor",            DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "iTMS",                  DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "Genetics 1",            DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "Genetics 2",            DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "Genetics 3",            DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "Genetics 5",            DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "Genetics Analysts",     DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "Genetics Autotesting",  DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "HLA",                   DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "Portal",                DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "Regr.Testing",          DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "Total QC",              DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "CM",                    DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "DBA",                   DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "MIS Tech Support",      DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "SA",                    DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "SE",                    DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "Architects",            DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "BI",                    DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "CSF",                   DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "Interfaces",            DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "Lab 5.0",               DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "LabMic",                DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "Mic 5.0",               DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "RNV",                   DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "Reports",               DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "Genetics Support",      DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "Support",               DepartmentGroup = departmentGroups[2]},
                  new Department { Title = "Update",                DepartmentGroup = departmentGroups[2]},
            };
            context.Departments.AddRange(departments);
            context.SaveChanges();

            List<Language> languages = new List<Language>()
            {
                new Language { Title="English" },
                new Language { Title="Polish" }
            };

            context.Languages.AddRange(languages);
            context.SaveChanges();

            List<Country> countries = new List<Country>()
            {
                new Country { Title="Ukraine" }
            };

            context.Countries.AddRange(countries);
            context.SaveChanges();

            List<Location> locations = new List<Location>()
            {
                new Location { Country=countries[0], Title="Kiev" },
                new Location { Country=countries[0], Title="Kharkiv" },
                new Location { Country=countries[0], Title="Odessa" },
                new Location { Country=countries[0], Title="Dnipropetrovsk" },
                new Location { Country=countries[0], Title="Zaporizhia" },
                new Location { Country=countries[0], Title="Lviv" },
                new Location { Country=countries[0], Title="Kryvyi Rih" },
                new Location { Country=countries[0], Title="Mykolaiv" },
                new Location { Country=countries[0], Title="Mariupol" },
                new Location { Country=countries[0], Title="Luhansk" },
                new Location { Country=countries[0], Title="Donetsk" },
                new Location { Country=countries[0], Title="Sevastopol" },
                new Location { Country=countries[0], Title="Vinnytsia" },
                new Location { Country=countries[0], Title="Makiivka" },
                new Location { Country=countries[0], Title="Simferopol" },
                new Location { Country=countries[0], Title="Kherson" },
                new Location { Country=countries[0], Title="Poltava" },
                new Location { Country=countries[0], Title="Chernihiv" },
            };

            context.Locations.AddRange(locations);
            context.SaveChanges();

            List<Stage> stages = new List<Stage>()
            {
                new Stage { Title="Pool" },
                new Stage { Title="Selected" },
                new Stage { Title="HR Interview" },
                new Stage { Title="Test task" },
                new Stage { Title="Tech Interview" },
                new Stage { Title="Additional interview" },
                new Stage { Title="Final Interview",  },
                new Stage { Title="Job Offer Issued" },
                new Stage { Title="Job Offer Accepted", },
                new Stage { Title="Hired" },
                new Stage { Title="Rejected" },
            };

            context.Stages.AddRange(stages);
            context.SaveChanges();

            Photo photo = new Photo()
            {
                Description = "desc",
                ImagePath = "path",
            };
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
                Industry = context.Industries.First(),
                TypeOfEmployment = TypeOfEmployment.FullTime,
                Title = "Architecht",
                Comments = new List<Comment>() { },
                DeadlineDate = DateTime.Now,
                Description = "descr",
                EndDate = DateTime.Now,
                Files = new List<File>() { },
                LanguageSkill = new LanguageSkill() { LanguageId = 1, LanguageLevel = LanguageLevel.Fluent },
                Level =new List<Level>() { context.Levels.First() },
                Locations = new List<Location> { context.Locations.First() },
                ParentVacancy = null,
                RequiredSkills = new List<Skill>() { context.Skills.First() },
                SalaryMax = 100500,
                SalaryMin = 15,
                StartDate = DateTime.Now,
                Department = context.Departments.First(),
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
                Industry = context.Industries.First(),
                Description = "candidate is candi",
                Email = "killer666@mayl.op",
                Files = new List<File>() { },
                IsMale = true,
                LanguageSkills = new List<LanguageSkill>() { new LanguageSkill() { LanguageId=1, LanguageLevel = LanguageLevel.Advanced } },
                LastName = "Yehayy",
                MiddleName = "Caro",
                PhoneNumbers = new List<PhoneNumber>() {  },
                Photo = new Photo() { Description = "desc", ImagePath = "path" },
                PositionDesired = "Pos",
                Practice = "Good",
                RelocationAgreement = true,
                SalaryDesired = 500,
                Skills = new List<Skill>() { context.Skills.First() },
                Skype = "skyper133",
                SocialNetworks = new List<CandidateSocial>() { new CandidateSocial() { Path="path",  SocialNetwork = new SocialNetwork() { ImagePath="imgPath", Title="title" } } },
                Sources = new List<CandidateSource>() { new CandidateSource() { Source = Source.WorkUa, Path="Path" } },
                StartExperience = DateTime.Now,
                Tags = new List<Tag>() { context.Tags.First() },
                TypeOfEmployment = TypeOfEmployment.FullTime,
                VacanciesProgress = new List<VacancyStageInfo>() {  }
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
