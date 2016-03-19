using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BotLibrary.Entities;
using BotLibrary.Entities.Setup;
using BotLibrary.Entities.Enum;

namespace BotWebApi.Models
{
    public class DummyBotContext : IBotContext
    {
        List<Candidate> _candidates = new List<Candidate>();
        List<Vacancy> _vacancies = new List<Vacancy>();

        public DummyBotContext()
        {
            Random r = new Random();

            Location _location = new Location()
            {
                Id = 1,
                EditTime = DateTime.Now,
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
                    CandidatesProgress = new Dictionary<Candidate, List<StageInfo>>()
                });
            }
            int _nameIndex = 0;
            for (int i = 1; i <= 200; i++)
            {
                _candidates.Add(new Candidate()
                {
                    Id = i,
                    EditTime = DateTime.Now,
                    PersonalInfo = new PersonalInfo()
                    {
                        Id = i,
                        EditTime = DateTime.Now.AddDays(i),
                        BirthDate = DateTime.Now.Subtract(new TimeSpan(i, i, i)),
                        Gender = i % 2 == 0 ? true : false,
                        FirstName = i % 2 == 0 ? Names.MaleFirstNames[_nameIndex] : Names.FemaleFirstNames[_nameIndex],
                        MiddleName = i.ToString(),
                        LastName = i.ToString(),
                        Photo = null
                    },
                    ContactInfo = new ContactInfo()
                    {
                        Id = i,
                        EditTime = DateTime.Now.AddDays(i),
                        Email = string.Format("email{0}@email.com", i),
                        PhoneNumbers = new List<string>() { "+390" + r.Next(000000001, 999999999).ToString() },
                        Skype = "skype" + i,
                    },
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
                    WorkInfo = new WorkInfo()
                    {
                        Id = i,
                        EditTime = DateTime.Now,
                        Experience = new Experience()
                        {
                            Id = i,
                            EditTime = DateTime.Now,
                            WorkExperience = new TimeSpan(i)
                        },
                        PositionDesired = "Junior Architechique",
                        Practice = "Good practice",
                        SalaryDesired = 500 + i,
                        Skills = _skills,
                        TypeOfEmployment = TypeOfEmployment.FullTime
                    },
                    VacanciesProgress = new Dictionary<Vacancy, StageInfo>()
                });
                _nameIndex++;
                if (Names.MaleFirstNames.Count - 1 == _nameIndex || Names.FemaleFirstNames.Count - 1 == _nameIndex) _nameIndex = 0;
            }


            List<StageInfo> _stages = new List<StageInfo>()
            {
                new StageInfo()
                {
                    Id = 1,
                    EditTime = DateTime.Now,
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
                    Id = 2,
                    EditTime = DateTime.Now,
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
                c.VacanciesProgress.Add(_vacancies[_vacancyIndex], _stages[0]);
                _vacancies[_vacancyIndex].CandidatesProgress.Add(c, new List<StageInfo>() { _stages[0] });
                _vacancyIndex++;
                if (_vacancyIndex == 19) _vacancyIndex = 0;
            }

        }

        public List<Candidate> Candidates
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

        public List<Vacancy> Vacancies
        {
            get
            {
                return _vacancies;
            }
        }
    }
}