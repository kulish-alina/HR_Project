using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Setup;
using System.Linq.Expressions;
using Domain.Entities.Enum.Setup;

namespace Data.DumbData
{
    public class DummyBotContext
    {
        List<Candidate> _candidates = new List<Candidate>();
        List<Vacancy> _vacancies = new List<Vacancy>();
        List<SocialNetwork> _socialNetworks = new List<SocialNetwork>();


        static List<Role> roles = new List<Role>()
            {
                new Role { Title="Manager" },
                new Role { Title="Recruiter" },
                new Role { Title="Freelancer" }
            };

        static List<Industry> industries = new List<Industry>()
            {
                new Industry { Title="IT" },
                new Industry { Title="Accounting" },
                new Industry { Title="Administration" }
            };
        static List<Level> levels = new List<Level>()
            {
                new Level { Title="Trainee" },
                new Level { Title="Junior" },
                new Level { Title="Middle" },
                new Level { Title="Senior" }
            };
        static List<Tag> _tags = new List<Tag>()
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

        static List<Language> _languages = new List<Language>()
        {
                new Language { Title="English" },
                new Language { Title="Polish" }
        };

        static List<Skill> _skills = new List<Skill>()
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

        static List<DepartmentGroup> _departmentGroups = new List<DepartmentGroup>()
            {
                new DepartmentGroup { Title="Contract" },
                new DepartmentGroup { Title="Nonprod" },
                new DepartmentGroup { Title="Prod" }
            };



        static List<Department> _departments = new List<Department>()
            {
                  new Department { Title = "Contract Programming",  DepartmentGroup = _departmentGroups[0]},
                  new Department { Title = "Sites Design",          DepartmentGroup = _departmentGroups[0]},
                  new Department { Title = "Web apps",              DepartmentGroup = _departmentGroups[0]},
                  new Department { Title = "Accounting",            DepartmentGroup = _departmentGroups[1]},
                  new Department { Title = "Administration",        DepartmentGroup = _departmentGroups[1]},
                  new Department { Title = "Executives",            DepartmentGroup = _departmentGroups[1]},
                  new Department { Title = "DevWorkshop",           DepartmentGroup = _departmentGroups[1]},
                  new Department { Title = "HR",                    DepartmentGroup = _departmentGroups[1]},
                  new Department { Title = "Managers",              DepartmentGroup = _departmentGroups[1]},
                  new Department { Title = "Managers",              DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "QA",                    DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "UPM",                   DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "EPE",                   DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "Soft Web",              DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "AR",                    DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "Bank/Donor",            DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "iTMS",                  DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "Genetics 1",            DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "Genetics 2",            DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "Genetics 3",            DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "Genetics 5",            DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "Genetics Analysts",     DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "Genetics Autotesting",  DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "HLA",                   DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "Portal",                DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "Regr.Testing",          DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "Total QC",              DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "CM",                    DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "DBA",                   DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "MIS Tech Support",      DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "SA",                    DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "SE",                    DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "Architects",            DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "BI",                    DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "CSF",                   DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "Interfaces",            DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "Lab 5.0",               DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "LabMic",                DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "Mic 5.0",               DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "RNV",                   DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "Reports",               DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "Genetics Support",      DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "Support",               DepartmentGroup = _departmentGroups[2]},
                  new Department { Title = "Update",                DepartmentGroup = _departmentGroups[2]},
            };

        static List<Country> _countries = new List<Country>()
            {
                new Country { Title="Ukraine" }
            };

        static List<Stage> _stages = new List<Stage>()
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

        static List<Location> _locations = new List<Location>()
            {
                new Location { Country=_countries[0], Title="Kiev" },
                new Location { Country=_countries[0], Title="Kharkiv" },
                new Location { Country=_countries[0], Title="Odessa" },
                new Location { Country=_countries[0], Title="Dnipropetrovsk" },
                new Location { Country=_countries[0], Title="Zaporizhia" },
                new Location { Country=_countries[0], Title="Lviv" },
                new Location { Country=_countries[0], Title="Kryvyi Rih" },
                new Location { Country=_countries[0], Title="Mykolaiv" },
                new Location { Country=_countries[0], Title="Mariupol" },
                new Location { Country=_countries[0], Title="Luhansk" },
                new Location { Country=_countries[0], Title="Donetsk" },
                new Location { Country=_countries[0], Title="Sevastopol" },
                new Location { Country=_countries[0], Title="Vinnytsia" },
                new Location { Country=_countries[0], Title="Makiivka" },
                new Location { Country=_countries[0], Title="Simferopol" },
                new Location { Country=_countries[0], Title="Kherson" },
                new Location { Country=_countries[0], Title="Poltava" },
                new Location { Country=_countries[0], Title="Chernihiv" },
            };

        public DummyBotContext()
        {

            #region Candidate
            Comment candidateComment = new Comment()
            {
                Message = "msg",
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


            LanguageSkill languageSkill = new LanguageSkill()
            {
                Language = _languages[0],
                LanguageLevel = LanguageLevel.Fluent,
            };


            Photo photo = new Photo()
            {
                Description = "descr",
                ImagePath = "path"
            };

            SocialNetwork socialNetwork = new SocialNetwork()
            {
                Id = 1,
                ImagePath = "path",
                Title = "Path"
            };
            _socialNetworks.Add(socialNetwork);

            CandidateSocial candidateSocial = new CandidateSocial()
            {
                Path = "path",
                SocialNetwork = socialNetwork,
            };

            Candidate candidate = new Candidate()
            {
                Id = 1,
                Skype = "skype",
                BirthDate = DateTime.Now,
                Comments = new List<Comment>() { candidateComment },
                Description = "descrpition",
                Education = "High",
                Email = "email",
                Industry = industries[0],
                StartExperience = DateTime.Now,
                Tags = new List<Tag>() { _tags[0] },
                Files = new List<File>() { candidateFile },
                Sources = new List<CandidateSource>() { candidateSource },
                FirstName = "TESTNAME",
                IsMale = true,
                LanguageSkills = new List<LanguageSkill>() { languageSkill },
                LastName = "lname",
                Location = _locations[0],
                MiddleName = "mname",
                PhoneNumbers = new List<PhoneNumber>() { new PhoneNumber() { Id = 1 ,Number = "+380978762352" } },
                Photo = photo,
                PositionDesired = "architecht",
                Practice = "best",
                RelocationAgreement = true,
                SalaryDesired = 10500,
                Skills = new List<Skill>() { _skills[0] },
                SocialNetworks = new List<CandidateSocial>() { candidateSocial },
                TypeOfEmployment = TypeOfEmployment.FullTime,
                VacanciesProgress = new List<VacancyStageInfo>() { }
            };
            #endregion

            Comment vacancyComment = new Comment()
            {
                Message = "msg",
            };

            File vacancyFile = new File()
            {

                Description = "file",
                FilePath = "path",
            };

            Permission permission = new Permission()
            {
                Id = 1,

                AccessRights = AccessRights.AddCandidateToVacancy,
                Description = "Permis",
            };

            Role role = new Role()
            {
                Id = 1,

                Title = "adm",
                Permissions = new List<Permission>() { permission },
            };

            User user = new User()
            {
                Id=1,
                BirthDate = DateTime.Now,
                Email = "mail",
                FirstName = "fname",
                isMale = true,
                LastName = "lastname",
                Location = _locations[0],
                Login = "login",
                Password = "pass",
                MiddleName = "mname",
                PhoneNumbers = new List<PhoneNumber>() { new PhoneNumber() { Number = "+3565234662" } },
                Photo = photo,
                Role = role,
                Skype = "skype",
            };

            Vacancy vacancy = new Vacancy()
            {
                Id = 1,
                TypeOfEmployment = TypeOfEmployment.FullTime,
                Title = "Architecht",
                Comments = new List<Comment>() { vacancyComment },
                DeadlineDate = DateTime.Now,
                Description = "descr",
                EndDate = DateTime.Now,
                Files = new List<File>() { vacancyFile },
                LanguageSkill = languageSkill,
                Level = new List<Level>() { levels[2] },
                Locations = new List<Location>() { _locations[0] },
                ParentVacancy = null,
                RequiredSkills = new List<Skill>() { _skills[0] },
                Responsible = user,
                SalaryMax = 100500,
                SalaryMin = 15,
                StartDate = DateTime.Now,
                Department = _departments[0],
                CandidatesProgress = new List<VacancyStageInfo>()

            };

            Comment vsicomment = new Comment()
            {
                Message = "msg",
            };

            VacancyStage vs = new VacancyStage()
            {
                IsCommentRequired = true,
                Order = 1,
                Stage = _stages[0],
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

            _vacancies.Add(vacancy);
            _candidates.Add(candidate);
        }


        public IList<Candidate> Candidates
        {
            get
            {
                return _candidates;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IList<Location> Locations
        {
            get
            {
                return _locations;
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public IList<Department> Departments
        {
            get
            {
                return _departments;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IList<Skill> Skills
        {
            get
            {
                return _skills;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IList<Language> Languages
        {
            get
            {
                return _languages;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IList<Stage> Stages
        {
            get
            {
                return _stages;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IList<SocialNetwork> SocialNetworks
        {
            get
            {
                return _socialNetworks;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IList<Vacancy> Vacancies
        {
            get
            {
                return _vacancies;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
