using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Setup;
using System.Linq.Expressions;

namespace Data.DumbData
{
    public class DummyBotContext
    {
        List<Candidate> _candidates = new List<Candidate>();
        List<Vacancy> _vacancies = new List<Vacancy>();
    
       /* public DummyBotContext()
        {
            Random r = new Random();

            Location _location = new Location()
            {
                City = new City()
                {
                    Id = 1,
                    EditTime = DateTime.Now,
                    Country = new Country()
                    {
                        Id = 1,
                        EditTime = DateTime.Now,
                        Name = "Ukraine"
                    },
                    Name = "Dnipro"
                }
            };
            List<Language> _languages = new List<Language>()
                    {
                        new Language()
                        {
                            Id = 1,
                            EditTime = DateTime.Now,
                            State = BotLibrary.Entities.Enum.EntityState.Active,
                            Title = "lang"
                        }
                    };
            List<Skill> _skills = new List<Skill>()
            {
                 new Skill()
                 {
                     Id = 1,
                     EditTime = DateTime.Now,
                     Title = "Stress blocker",
                     State = BotLibrary.Entities.Enum.EntityState.Active
                 }
            };
            Permission _permission = new Permission()
            {
                Id = 1,
                EditTime = DateTime.Now,
                Role = null,
                Description = "Can do anything"
            };

            Role _adminRole = new Role()
            {
                Id = 1,
                EditTime = DateTime.Now,
                Name = "Admin",
                Permissions = new List<Permission>() { _permission },
                State = BotLibrary.Entities.Enum.EntityState.Active,
            };
            
            Photo _photo = new Photo()
            {
                Description = "my cat",
                ImagePath = "path"
            }

            User _user = new User()
            {
                Id = 1,
                EditTime = DateTime.Now,
                Login = "admin",
                Password = "admin",
                Role = _adminRole,
                Location = _location,
                BirthDate = DateTime.Now,
                State = BotLibrary.Entities.Enum.EntityState.Active,
                Email = "email",
                FirstName = "fnmame",
                isMale = true,
                LastName = "lname",
                MiddleName = "mname",
                PhoneNumbers = new List<string>( ) { "98309382231"},
                Photo = _photo,
                Skype = "skype"
            };


            for (int i = 1; i <= 20; i++)
            {
                _vacancies.Add(new Vacancy()
                {
                    Id = i,
                    EditTime = DateTime.Now,
                    DeadlineDate = DateTime.Now.AddMonths(i),
                    CandidatesProgress = new List<VacancyStageInfo>() {  }
                    Description = "Junior Arcihtech " + i,
                    EndDate = DateTime.MinValue,
                    Level = Level.Junior,
                    Location = _location,
                    RequiredSkills = _skills,
                    Responsible = _user,
                    SalaryMax = 1000,
                    SalaryMin = 500,
                    StartDate = DateTime.Now,
                    TypeOfEmployment = TypeOfEmployment.FullTime,
                    Comments = new List<Comment>(),
                    Files = new List<File>(),
                });
            }
            int _nameIndex = 0;
            for (int i = 1; i <= 200; i++)
            {
                _candidates.Add(new Candidate()
                {
                    Id = i,
                    EditTime = DateTime.Now,
                    BirthDate = DateTime.Now.Subtract(new TimeSpan(i, i, i)),
                    Gender = i % 2 == 0 ? true : false,
                    FirstName = i % 2 == 0 ? Storage.MaleFirstNames[_nameIndex] : Storage.FemaleFirstNames[_nameIndex],
                    MiddleName = i.ToString(),
                    LastName = i.ToString(),
                    Photo = null,
                    Email = string.Format("email{0}@email.com", i),
                    PhoneNumbers = new List<string>() { "+38093000" + i },
                    Skype = "skype" + i,
                    Comments = new List<Comment>()
                    {
                        new Comment()
                        {
                            Id = i,
                            EditTime = DateTime.Now,
                            CommentType = CommentType.Candidate,
                            Message = "Iam a message number " + i
                        }
                    },
                    Description = "Candidate's description",
                    Education = "Highest",
                    Languages = _languages,
                    Location = _location,
                    RelocationAgreement = i % 2 == 0 ? true : false,
                    Files = new List<File>(),
                    SocialNetworks = new List<SocialNetwork>(),
                    Sources = new List<Source>(),
                    Experience = new Experience()
                    {
                        Id = i,
                        EditTime = DateTime.Now,
                        WorkExperience = new TimeSpan(i)
                    },
                    PositionDesired = i % 2 == 0 ? (i % 3 == 0 ? "Senior" : "Middle") : "Junior",
                    Practice = "Good practice",
                    SalaryDesired = 500 + i,
                    Skills = _skills,
                    TypeOfEmployment = TypeOfEmployment.FullTime,
                    VacanciesProgress = new List<VacancyStageInfo>()
                });
                _nameIndex++;
                if (Storage.MaleFirstNames.Count - 1 == _nameIndex || Storage.FemaleFirstNames.Count - 1 == _nameIndex) _nameIndex = 0;
            }


            List<StageInfo> _stages = new List<StageInfo>()
            {
                new StageInfo()
                {
                    Stage = new Stage()
                    {
                        Id = 1,
                        EditTime = DateTime.Now,
                        Title = "Pool",
                        Description = "Starting location of people"
                    }
                },
                new StageInfo()
                {
                    Stage = new Stage()
                    {
                        Id = 2,
                        EditTime = DateTime.Now,
                        Title = "HR Interview",
                        Description = "Second location of people"
                    }
                }
            };

            int _vacancyIndex = 0;
            foreach (var c in _candidates)
            {
                if (_vacancyIndex == 20) _vacancyIndex = 0;
                c.VacanciesProgress.Add(new VacancyStageInfo()
                {
                    Vacancy = _vacancies[_vacancyIndex],
                    StageInfos = new List<StageInfo>() { _stages[_vacancyIndex % 2 == 0 ? 0 : 1] }
                });
                _vacancies[_vacancyIndex].CandidatesProgress.Add(new CandidateStageInfo()
                {
                    Candidate = c,
                    StageInfos = new List<StageInfo>() { _stages[_vacancyIndex % 2 == 0 ? 0 : 1] }
                });
                _vacancyIndex++;
            }

        }*/

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
