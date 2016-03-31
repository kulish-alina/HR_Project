using BotData.Abstract;
using BotLibrary.Entities;
using BotLibrary.Entities.Enum;
using BotLibrary.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BotData.DumbData
{
    public class DummyBotContext : IContext
    {
        List<Candidate> _candidates = new List<Candidate>();
        List<Vacancy> _vacancies = new List<Vacancy>();
    
        public DummyBotContext()
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
                            Name = "English",
                            LanguageLevel = LanguageLevel.Fluent
                        }
                    };
            List<Skill> _skills = new List<Skill>()
            {
                 new Skill()
                 {
                     Id = 1,
                     EditTime = DateTime.Now,
                     Title = "Stress blocker"
                 }
            };
            Role _adminRole = new Role()
            {
                Id = 1,
                EditTime = DateTime.Now,
                Name = "Admin",
            };

            Permission _permission = new Permission()
            {
                Id = 1,
                EditTime = DateTime.Now,
                Role = _adminRole,
                Description = "Can do anything"
            };

            User _user = new User()
            {
                Id = 1,
                EditTime = DateTime.Now,
                Login = "admin",
                Password = "admin",
                Role = _adminRole,
                Location = _location,
            };

            for (int i = 1; i <= 20; i++)
            {
                _vacancies.Add(new Vacancy()
                {
                    Id = i,
                    EditTime = DateTime.Now,
                    DeadlineDate = DateTime.Now.AddMonths(i),
                    Department = new Department()
                    {
                        Id = i,
                        EditTime = DateTime.Now,
                        Title = "Deratment Number" + i,
                    },
                    Description = "Junior Arcihtech " + i,
                    EndDate = DateTime.MinValue,
                    IsDeadlineAddedToCalendar = i % 2 == 0 ? true : false,
                    Level = Level.Junior,
                    Location = _location,
                    Name = "Junior Arcihtech " + i,
                    RequiredLanguages = _languages,
                    RequiredSkills = _skills,
                    Responsible = _user,
                    SalaryMax = 1000,
                    SalaryMin = 500,
                    StartDate = DateTime.Now,
                    Status = new VacancyStatus()
                    {
                        Id = 1,
                        EditTime = DateTime.Now,
                        Description = "First status",
                        Title = "Start",
                    },
                    TypeOfEmployment = TypeOfEmployment.FullTime,
                    ChildredVacanciesCount = 0,
                    Comments = new List<Comment>(),
                    Files = new List<File>(),
                    MasterVacancy = false,
                    CandidatesProgress = new List<CandidateStageInfo>()
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
