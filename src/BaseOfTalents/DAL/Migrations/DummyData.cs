using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Enum.Setup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Migrations
{
    public static class DummyData
    {
        public static readonly List<MailContent> Mails = new List<MailContent>
        {
            new MailContent
            {
                Invitation = "Hello our dear incomer,",
                Body = @"   Your login is: #UserLogin
    With password: #UserPassword",
                Farewell = "We hope that you will enjoy usage of our new service!",
                Subject = "Meeting HR-bot"
            }
        };

        public static readonly List<Currency> Currencies = new List<Currency>
        {
            new Currency { Title = "UAH" },
            new Currency { Title = "USD" },
            new Currency { Title = "EUR" }
        };

        public static readonly List<Skill> Skills = new List<Skill>
        {
            new Skill {Title = "SQL"},
            new Skill {Title = "WinForms"},
            new Skill {Title = "DevExpress"},
            new Skill {Title = ".Net"},
            new Skill {Title = "C#"},
            new Skill {Title = "Spring .Net"},
            new Skill {Title = "JQuery"},
            new Skill {Title = "JavaScript"},
            new Skill {Title = "ASP .NET MVC"},
            new Skill {Title = "HTML5+CSS3"},
            new Skill {Title = "NHibernate"},
            new Skill {Title = "Entity Framework"}
        };

        public static readonly List<Tag> Tags = new List<Tag>
        {
            new Tag {Title = "SQL"},
            new Tag {Title = "WinForms"},
            new Tag {Title = "DevExpress"},
            new Tag {Title = ".Net"},
            new Tag {Title = "C#"},
            new Tag {Title = "Spring .Net"},
            new Tag {Title = "JQuery"},
            new Tag {Title = "JavaScript"},
            new Tag {Title = "ASP .NET MVC"},
            new Tag {Title = "HTML5+CSS3"},
            new Tag {Title = "NHibernate"},
            new Tag {Title = "Entity Framework"}
        };

        public static readonly List<Industry> Industries = new List<Industry>
        {
            new Industry {Title = "IT"},
            new Industry {Title = "Accounting"},
            new Industry {Title = "Administration"}
        };

        public static readonly List<Level> Levels = new List<Level>
        {
            new Level {Title = "Trainee"},
            new Level {Title = "Junior"},
            new Level {Title = "Middle"},
            new Level {Title = "Senior"}
        };

        public static readonly List<DepartmentGroup> DepartmentGroups = new List<DepartmentGroup>
        {
            new DepartmentGroup {Title = "Contract"},
            new DepartmentGroup {Title = "Nonprod"},
            new DepartmentGroup {Title = "Prod"}
        };

        private static readonly Random rnd = new Random();
        private static readonly object syncLock = new object();

        public static readonly List<Department> Departments = new List<Department>
        {
            new Department {Title = "Contract Programming", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "Sites Design", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "Web apps", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "Accounting", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "Administration", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "Executives", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "DevWorkshop", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "HR", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "Managers", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "Managers", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "QA", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "UPM", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "EPE", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "Soft Web", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "AR", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "Bank/Donor", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "iTMS", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "Genetics 1", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "Genetics 2", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "Genetics 3", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "Genetics 5", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "Genetics Analysts", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "Genetics Autotesting", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "HLA", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "Portal", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "Regr.Testing", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "Total QC", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "CM", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "DBA", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "MIS Tech Support", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "SA", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "SE", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "Architects", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "BI", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "CSF", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "Interfaces", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "Lab 5.0", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "LabMic", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "Mic 5.0", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "RNV", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "Reports", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "Genetics Support", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "Support", DepartmentGroup = DepartmentGroups.GetRandom()},
            new Department {Title = "Update", DepartmentGroup = DepartmentGroups.GetRandom()}
        };

        public static readonly List<Language> Languages = new List<Language>
        {
            new Language {Title = "English"},
            new Language {Title = "Polish"}
        };

        public static readonly List<LanguageSkill> LanguageSkills = new List<LanguageSkill>
        {
            new LanguageSkill {Language = Languages[0], LanguageLevel = LanguageLevel.Beginner},
            new LanguageSkill {Language = Languages[0], LanguageLevel = LanguageLevel.Advanced},
            new LanguageSkill {Language = Languages[0], LanguageLevel = LanguageLevel.Fluent},
            new LanguageSkill {Language = Languages[0], LanguageLevel = LanguageLevel.Intermediate},
            new LanguageSkill {Language = Languages[0], LanguageLevel = LanguageLevel.PreIntermediate},
            new LanguageSkill {Language = Languages[0], LanguageLevel = LanguageLevel.UpperIntermediate}
        };

        public static readonly List<Country> Countries = new List<Country>
        {
            new Country {Title = "Ukraine"}
        };

        public static readonly List<City> Cities = new List<City>
        {
            new City {Country = Countries[0], Title = "Kiev"},
            new City {Country = Countries[0], Title = "Kharkiv"},
            new City {Country = Countries[0], Title = "Odessa"},
            new City {Country = Countries[0], Title = "Dnipropetrovsk", HasOffice = true },
            new City {Country = Countries[0], Title = "Berdiansk", HasOffice = true },
            new City {Country = Countries[0], Title = "Zaporizhia", HasOffice = true },
            new City {Country = Countries[0], Title = "Lviv", HasOffice = true },
            new City {Country = Countries[0], Title = "Kryvyi Rih"},
            new City {Country = Countries[0], Title = "Mykolaiv"},
            new City {Country = Countries[0], Title = "Mariupol"},
            new City {Country = Countries[0], Title = "Luhansk"},
            new City {Country = Countries[0], Title = "Donetsk"},
            new City {Country = Countries[0], Title = "Sevastopol"},
            new City {Country = Countries[0], Title = "Vinnytsia"},
            new City {Country = Countries[0], Title = "Makiivka"},
            new City {Country = Countries[0], Title = "Simferopol"},
            new City {Country = Countries[0], Title = "Kherson"},
            new City {Country = Countries[0], Title = "Poltava"},
            new City {Country = Countries[0], Title = "Chernihiv"}
        };

        public static readonly List<Permission> Permissions = new List<Permission>
        {
            new Permission
            {
                AccessRights = AccessRight.AddCandidate,
                Description = "Right to create a candidate",
                Group = "Candidates"
            },
            new Permission
            {
                AccessRights = AccessRight.AddCandidateToVacancy,
                Description = "Right to attach exsisting candidate to a vacancy",
                Group = "Vacancies"
            },
            new Permission
            {
                AccessRights = AccessRight.AddEvent,
                Description = "Right to create an event",
                Group = "Calendar"
            },
            new Permission
            {
                AccessRights = AccessRight.AddRole,
                Description = "Right to create a role",
                Group = "Roles"
            },
            new Permission
            {
                AccessRights = AccessRight.AddVacancy,
                Description = "Right to create a vacancy",
                Group = "Vacancies"
            },
            new Permission
            {
                AccessRights = AccessRight.EditCandidate,
                Description = "Right to edit a candidate",
                Group = "Candidates"
            },
            new Permission
            {
                AccessRights = AccessRight.EditEvent,
                Description = "Right to edit an event",
                Group = "Calendar"
            },
            new Permission
            {   AccessRights = AccessRight.EditRole,
                Description = "Right to edit a role",
                Group = "Roles"
            },
            new Permission
            {
                AccessRights = AccessRight.EditUserProfile,
                Description = "Right to edit user profile",
                Group = "Users"
            },
            new Permission
            {
                AccessRights = AccessRight.EditVacancy,
                Description = "Right to edit a vacancy",
                Group = "Vacancies"
            },
            new Permission
            {
                AccessRights = AccessRight.GenerateReports,
                Description = "Right to generate reports",
                Group = "Reports"
            },
            new Permission
            {
                AccessRights = AccessRight.InviteNewMember,
                Description = "Right to invite a new member to program",
                Group = "Users"
            },
            new Permission
            {
                AccessRights = AccessRight.RemoveCandidate,
                Description = "Right to remove candidate",
                Group = "Candidates"
            },
            new Permission
            {
                AccessRights = AccessRight.RemoveCandidateFromVacancy,
                Description = "Right to remove candidate from a vacancy",
                Group = "Vacancies"
            },
            new Permission
            {
                AccessRights = AccessRight.RemoveEvent,
                Description = "Right to remove event",
                Group = "Calendar"
            },
            new Permission
            {
                AccessRights = AccessRight.RemoveRole,
                Description = "Right to remove role",
                Group = "Roles"
            },
            new Permission
            {
                AccessRights = AccessRight.RemoveUserProfile,
                Description = "Right to remove user profile",
                Group = "Users"
            },
            new Permission
            {
                AccessRights = AccessRight.RemoveVacancy,
                Description = "Right to remove vacancy",
                Group = "Vacancies"
            },
            new Permission
            {
                AccessRights = AccessRight.SearchCandidatesInExternalSource,
                Description = "Right to search candidates on another work-searching sites",
                Group = "Candidates"
            },
            new Permission
            {
                AccessRights = AccessRight.SearchCandidatesInInternalSource,
                Description = "Right to search candidates inside the base",
                Group = "Candidates"
            },
            new Permission
            {
                AccessRights = AccessRight.SystemSetup,
                Description = "Right to provide system setup",
                Group = "System"
            },
            new Permission
            {
                AccessRights = AccessRight.ViewCalendar,
                Description = "Right to view a calendar",
                Group = "Calendar"
            },
            new Permission
            {
                AccessRights = AccessRight.ViewListOfCandidates,
                Description = "Right to view list of candidates",
                Group = "Candidates"
            },
            new Permission
            {
                AccessRights = AccessRight.ViewListOfVacancies,
                Description = "Right to view list of vacancies",
                Group = "Vacancies"
            },
            new Permission {AccessRights = AccessRight.ViewRoles, Description = "Right to view roles", Group = "Roles"},
            new Permission
            {
                AccessRights = AccessRight.ViewUserProfile,
                Description = "Right to view user profile",
                Group = "Users"
            },
            new Permission
            {
                AccessRights = AccessRight.ViewUsers,
                Description = "Right to view users",
                Group = "Users"
            },
            new Permission
            {
                AccessRights = AccessRight.None,
                Description = "Right on getting access to nothing",
                Group = "Empty"
            }
        };

        public static readonly List<Stage> Stages = new List<Stage>
        {
            new Stage {Title = "Pool",                  CommentField = false, IsRequired = true, IsCommentRequired = false, IsDefault=true, Order=1, StageType = StageType.MainStage},
            new Stage {Title = "Selected",              CommentField = false, IsRequired = true, IsCommentRequired = false, IsDefault=true, Order=2, StageType = StageType.MainStage},
            new Stage {Title = "HR Interview",          CommentField = true, IsRequired = true, IsCommentRequired = true, IsDefault=true, Order=3, StageType = StageType.MainStage},
            new Stage {Title = "Test task",             CommentField = true, IsRequired = false, IsCommentRequired = true, IsDefault=true, Order=4, StageType = StageType.MainStage},
            new Stage {Title = "Tech Interview",        CommentField = true, IsRequired = true, IsCommentRequired = true, IsDefault=true, Order=5, StageType = StageType.MainStage},
            new Stage {Title = "Additional interview",  CommentField = true, IsRequired = false, IsCommentRequired = true, IsDefault=true, Order=6, StageType = StageType.MainStage},
            new Stage {Title = "Final Interview",       CommentField = true, IsRequired = true, IsCommentRequired = true, IsDefault=true, Order=7, StageType = StageType.MainStage},
            new Stage {Title = "Job Offer Issued",      CommentField = true, IsRequired = true, IsCommentRequired = false, IsDefault=true, Order=8, StageType = StageType.MainStage},
            new Stage {Title = "Job Offer Accepted",    CommentField = true, IsRequired = true, IsCommentRequired = false, IsDefault=true, Order=9, StageType = StageType.MainStage},
            new Stage {Title = "Hired",                 CommentField = true, IsRequired = true, IsCommentRequired = true, IsDefault=true, Order=10, StageType = StageType.HireStage},
            new Stage {Title = "Job Offer Rejected",    CommentField = true, IsRequired = false, IsCommentRequired = true, IsDefault=true, Order=11, StageType = StageType.RejectStage},
            new Stage {Title = "Rejected by Company",   CommentField = true, IsRequired = false, IsCommentRequired = true, IsDefault=true, Order=12, StageType = StageType.RejectStage},
            new Stage {Title = "Rejected by Candidate", CommentField = true, IsRequired = false, IsCommentRequired = true, IsDefault=true, Order=13, StageType = StageType.RejectStage}
        };


        public static readonly List<SocialNetwork> Socials = new List<SocialNetwork>
        {
            new SocialNetwork { Title = "Facebook"},
            new SocialNetwork { Title = "VK"},
            new SocialNetwork { Title = "LinkedIn"},
            new SocialNetwork { Title = "MySpace"},
            new SocialNetwork { Title = "Google+"}

        };

        public static readonly List<EventType> EventTypes = new List<EventType>
        {
            new EventType
            {
                Title = "HR Interview"
            },
            new EventType
            {
                Title = "Tech Interview"
            },
            new EventType
            {
                Title = "Final Interview"
            },
            new EventType
            {
                Title = "Candidate's Birthday"
            },
            new EventType
            {
                Title = "Candidate's First Day"
            },
            new EventType
            {
                Title = "Vacancy deadline"
            }
        };

        public static List<User> Users;
        public static List<Event> Events;
        public static List<Role> Roles;
        public static List<Vacancy> Vacancies;

        public static readonly List<Source> Sources = new List<Source>
        {
            new Source { Title = "By recommendation" },
            new Source { Title = "LinkedIn" },
            new Source { Title = "Vkontakte" },
            new Source { Title = "Djinni" },
            new Source { Title = "RabotaUa" },
            new Source { Title = "WorkUa" },
            new Source { Title = "HeadHunter" },
        };

        public static readonly List<Candidate> Candidates;

        private static readonly List<string> names = new List<string>
        {
            "Велизар",
            "Велимир",
            "Венедикт",
            "Вениамин",
            "Венцеслав",
            "Веньямин",
            "Викентий",
            "Виктор",
            "Викторий",
            "Викул",
            "Викула",
            "Вилен",
            "Виленин",
            "Вильгельм",
            "Виссарион",
            "Вит",
            "Виталий",
            "Витовт",
            "Витольд",
            "Владилен",
            "Владимир",
            "Владислав",
            "Владлен",
            "Влас",
            "Власий",
            "Вонифат",
            "Вонифатий",
            "Всеволод",
            "Всеслав",
            "Вукол",
            "Вышеслав",
            "Вячеслав",
            "Гавриил",
            "Гаврил",
            "Гаврила",
            "Галактион",
            "Гедеон",
            "Гедимин",
            "Геласий",
            "Гелий",
            "Геннадий",
            "Генрих",
            "Георгий",
            "Герасим",
            "Гервасий",
            "Герман",
            "Гермоген",
            "Геронтий",
            "Гиацинт",
            "Глеб",
            "Гораций",
            "Горгоний",
            "Гордей",
            "Гостомысл",
            "Гремислав",
            "Григорий",
            "Гурий",
            "Гурьян",
            "Давид",
            "Давыд",
            "Далмат",
            "Даниил",
            "Данил",
            "Данила",
            "Дементий",
            "Демид",
            "Демьян",
            "Денис",
            "Денисий",
            "Димитрий",
            "Диомид",
            "Дионисий",
            "Дмитрий",
            "Добромысл",
            "Добрыня",
            "Довмонт",
            "Доминик",
            "Донат",
            "Доримедонт",
            "Дормедонт",
            "Дормидбнт",
            "Дорофей",
            "Досифей",
            "Евгений",
            "Евграф",
            "Евграфий",
            "Евдоким",
            "Евлампий",
            "Евлогий",
            "Евмен",
            "Евмений",
            "Евсей",
            "Евстафий",
            "Евстахий",
            "Евстигней",
            "Евстрат",
            "Евстратий"
        };

        private static readonly List<string> lastNames = new List<string>
        {
            "Иванов",
            "Смирнов",
            "Кузнецов",
            "Попов",
            "Васильев",
            "Петров",
            "Соколов",
            "Михайлов",
            "Новиков",
            "Фёдоров",
            "Морозов",
            "Волков",
            "Алексеев",
            "Лебедев",
            "Семёнов",
            "Егоров",
            "Павлов",
            "Козлов",
            "Степанов",
            "Николаев",
            "Орлов",
            "Андреев",
            "Макаров",
            "Никитин",
            "Захаров"
        };

        private static readonly List<string> professons = new List<string>
        {
            "Страховой аналитик",
            "Аудиолог",
            "Математик",
            "Статистик",
            "Специалист в области биомедицины",
            "Научный аналитик",
            "Стоматолог-гигиенист",
            "Инженер-программист",
            "Терапевт",
            "Специалист компьютерных систем",
            "Журналист",
            "Лесоруб",
            "Военнослужащий в нижних воинских званиях",
            "Шеф-повар",
            "Конферансье (тамада)",
            "Фоторепортер",
            "Редактор",
            "Водитель такси",
            "Пожарный",
            "Почтовый курьер"
        };

        static DummyData()
        {
            Roles = GetRoles();
            Users = GetAdmin();
            Candidates = GetCandidates(197);
            Vacancies = GetVacancies(240);
            Events = GetEvents(100);
        }

        private static List<Event> GetEvents(int count)
        {
            var events = new List<Event>();
            for (int i = 0; i < count; i++)
            {
                events.Add(
                    new Event
                    {
                        Candidate = Candidates.GetRandom(),
                        Vacancy = Vacancies.GetRandom(),
                        Description = LoremIpsum(1, 3, 1, 2, 1),
                        EventTypeId = RandomNumber(1, 5),
                        EventDate = DateTime.Now.AddDays(RandomNumber(1, 60)),
                        Responsible = Users.GetRandom(),
                    });
            }
            return events;
        }



        private static List<User> GetAdmin()
        {
            return new List<User>
            {
                new User
                    {
                        BirthDate = DateTime.Now.AddYears(-RandomNumber(20, 40)),
                        Email = "admin@gmail.com",
                        FirstName = names.GetRandom(),
                        isMale = true,
                        LastName = lastNames.GetRandom(),
                        City = Cities.Where(city => city.HasOffice).ToList().GetRandom(),
                        Login = "admin",
                        MiddleName = names.GetRandom(),
                        Password = "admin",
                        PhoneNumbers = new List<PhoneNumber> { new PhoneNumber { Number = "380" + GetRandomNumbers(9) } },
                        Role = Roles[0],
                        Skype = GetRandomString(8)
                    }
            };
        }

        private static List<Role> GetRoles()
        {
            return new List<Role>() {
                new Role {
                    Title = "Adminstrator",
                    Permissions = Permissions
                }
            };
        }

        public static List<Vacancy> GetVacancies(int count)
        {
            var vacancies = new List<Vacancy>();
            var stages = Stages.Where(x => x.IsDefault);
            var citiesWithOffice = Cities.Where(city => city.HasOffice);
            for (var i = 0; i < count; i++)
            {
                var vacancy = new Vacancy
                {
                    DeadlineDate = DateTime.Now.AddDays(RandomNumber(-40, 40)),
                    Department = Departments.GetRandom(),
                    Description = LoremIpsum(5, 40, 1, 5, 1),
                    Industry = Industries.GetRandom(),
                    LanguageSkill = LanguageSkills.GetRandom(),
                    Levels = Levels.Take(RandomNumber(0, Levels.Count)).ToList(),
                    Cities = citiesWithOffice.Take(RandomNumber(1, citiesWithOffice.Count())).ToList(),
                    RequiredSkills = Enumerable.Repeat(Skills.GetRandom(), RandomNumber(0, 5)).Distinct().ToList(),
                    Responsible = Users.GetRandom(),
                    SalaryMax = RandomNumber(1000, 2000),
                    SalaryMin = RandomNumber(0, 1000),
                    Currency = Currencies.GetRandom(),
                    StartDate = DateTime.Now.AddDays(RandomNumber(-80, -40)),
                    Title = professons.GetRandom(),
                    TypeOfEmployment = TypeOfEmployment.FullTime,
                    EndDate = DateTime.Now.AddDays(RandomNumber(0, 30)),
                    Tags = Enumerable.Repeat(Tags.GetRandom(), RandomNumber(0, 5)).Distinct().ToList(),
                    Comments = Enumerable.Repeat(new Comment { Message = LoremIpsum(3, 15, 1, 2, 1), Author = Users.GetRandom() }, RandomNumber(0, 5)).Distinct().ToList(),
                    State = EntityState.Pending,
                    StageFlow = stages.Select(x => new ExtendedStage { Stage = x, Order = x.Order }).ToList()
                };
                vacancies.Add(vacancy);
            }
            return vacancies;
        }


        private static List<Candidate> GetCandidates(int count)
        {
            var candidates = new List<Candidate>();
            for (var i = 0; i < count; i++)
            {
                var candidate = new Candidate
                {
                    CityId = RandomNumber(1, Cities.Count - 1),
                    BirthDate = DateTime.Now.AddYears(RandomNumber(-40, -20)),
                    Comments =
                        Enumerable.Repeat(new Comment { Message = LoremIpsum(3, 15, 1, 2, 1), Author = Users.GetRandom() }, RandomNumber(0, 5)).Distinct().ToList(),
                    Education = GetRandomString(15),
                    FirstName = names.GetRandom(),
                    IndustryId = RandomNumber(1, Industries.Count - 1),
                    Description = professons.GetRandom(),
                    Email = string.Format("{0}@gmail.com", GetRandomString(5).Trim()),
                    Files = new List<File>(),
                    IsMale = true,
                    LanguageSkills =
                        Enumerable.Repeat(LanguageSkills.GetRandom(), RandomNumber(0, 5)).Distinct().ToList(),
                    LastName = lastNames.GetRandom(),
                    MiddleName = names.GetRandom(),
                    PhoneNumbers =
                        Enumerable.Repeat(new PhoneNumber { Number = "380" + GetRandomNumbers(9) }, RandomNumber(0, 5))
                            .Distinct()
                            .ToList(),
                    PositionDesired = professons.GetRandom(),
                    Practice = GetRandomString(20),
                    RelocationAgreement = true,
                    SalaryDesired = RandomNumber(300, 3000),
                    Currency = Currencies.GetRandom(),
                    Skills = Enumerable.Repeat(Skills.GetRandom(), RandomNumber(0, 5)).Distinct().ToList(),
                    Skype = "skyper." + GetRandomNumbers(4),
                    //Sources = Enumerable.Repeat(CandidateSources.GetRandom(), RandomNumber(0, 5)).Distinct().ToList(),
                    StartExperience = DateTime.Now.AddYears(-RandomNumber(0, 10)),
                    Tags = new List<Tag>(),
                    TypeOfEmployment = TypeOfEmployment.FullTime,
                    Level = Levels.GetRandom(),
                    Creator = Users.GetRandom(),
                    VacanciesProgress = new List<VacancyStageInfo>()
                };

                candidate.RelocationPlaces =
                            Enumerable.Repeat(
                                new RelocationPlace
                                {
                                    Country = Countries.GetRandom(),
                                    City = Cities.GetRandom(),
                                },
                                RandomNumber(1, 2))
                                .Distinct()
                                .ToList();
                candidates.Add(candidate);
            }
            return candidates;
        }

        public static IEnumerable<Note> Notes
        {
            get
            {
                var notes = new List<Note>();
                foreach (var user in Users)
                {
                    notes.Add(new Note { Message = LoremIpsum(3, 30, 1, 10, 1), User = user });
                }
                return notes;
            }
        }

        public static T GetRandom<T>(this List<T> source)
        {
            return source.PickRandom(1).Single();
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }

        private static string GetRandomString(int count)
        {
            var chars = "abcdefghijklmnopqrstuvwxyz ";
            var stringChars = new char[count];
            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[RandomNumber(0, chars.Length)];
            }
            return new string(stringChars);
        }

        private static string GetRandomNumbers(int count)
        {
            var nums = "1234567890";
            var stringChars = new char[count];
            for (var i = 0; i < count; i++)
            {
                stringChars[i] = nums[RandomNumber(0, nums.Length)];
            }
            return new string(stringChars);
        }

        static string LoremIpsum(int minWords, int maxWords, int minSentences, int maxSentences, int numParagraphs)
        {
            var words = new[]{"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
          "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
          "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"};

            int numSentences = RandomNumber(0, maxSentences - minSentences) + minSentences + 1;
            int numWords = RandomNumber(0, maxWords - minWords) + minWords + 1;

            System.Text.StringBuilder result = new System.Text.StringBuilder();
            for (int p = 0; p < numParagraphs; p++)
            {
                for (int s = 0; s < numSentences; s++)
                {
                    for (int w = 0; w < numWords; w++)
                    {
                        if (w > 0) { result.Append(" "); }
                        result.Append(words[RandomNumber(0, words.Length)]);
                    }
                    result.Append(". ");
                }
            }
            return result.ToString();
        }

        #region randomizer

        //Function to get random number

        public static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            {
                // synchronize
                return rnd.Next(min, max);
            }
        }

        #endregion randomizer
    }
}