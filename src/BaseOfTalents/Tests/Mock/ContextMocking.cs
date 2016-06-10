using BaseOfTalents.Domain.Entities;
using BaseOfTalents.Domain.Entities.Enum;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Mock
{
    public class ContextMocking
    {
        #region Data
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

        public static readonly List<Department> Departments = new List<Department>
        {
            new Department {Title = "Contract Programming", DepartmentGroup = DepartmentGroups[0]},
            new Department {Title = "Sites Design", DepartmentGroup = DepartmentGroups[1]},
            new Department {Title = "Web apps", DepartmentGroup = DepartmentGroups[2]}
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

        public static readonly List<Location> Locations = new List<Location>
        {
            new Location {Country = Countries[0], Title = "Kiev"},
            new Location {Country = Countries[0], Title = "Kharkiv"},
            new Location {Country = Countries[0], Title = "Odessa"},
            new Location {Country = Countries[0], Title = "Dnipropetrovsk"},
            new Location {Country = Countries[0], Title = "Zaporizhia"},
            new Location {Country = Countries[0], Title = "Lviv"},
            new Location {Country = Countries[0], Title = "Kryvyi Rih"},
            new Location {Country = Countries[0], Title = "Mykolaiv"},
            new Location {Country = Countries[0], Title = "Mariupol"},
            new Location {Country = Countries[0], Title = "Luhansk"},
            new Location {Country = Countries[0], Title = "Donetsk"},
            new Location {Country = Countries[0], Title = "Sevastopol"},
            new Location {Country = Countries[0], Title = "Vinnytsia"},
            new Location {Country = Countries[0], Title = "Makiivka"},
            new Location {Country = Countries[0], Title = "Simferopol"},
            new Location {Country = Countries[0], Title = "Kherson"},
            new Location {Country = Countries[0], Title = "Poltava"},
            new Location {Country = Countries[0], Title = "Chernihiv"}
        };

        public static readonly List<Permission> Permissions = new List<Permission>
        {
            new Permission
            {
                AccessRights = AccessRights.AddCandidate,
                Description = "Right to create a candidate",
                Group = "Candidates"
            },
            new Permission
            {
                AccessRights = AccessRights.AddCandidateToVacancy,
                Description = "Right to attach exsisting candidate to a vacancy",
                Group = "Vacancies"
            },
            new Permission
            {
                AccessRights = AccessRights.AddEvent,
                Description = "Right to create an event",
                Group = "Calendar"
            },
            new Permission
            {
                AccessRights = AccessRights.AddRole,
                Description = "Right to create a role",
                Group = "Roles"
            },
            new Permission
            {
                AccessRights = AccessRights.AddVacancy,
                Description = "Right to create a vacancy",
                Group = "Vacancies"
            },
            new Permission
            {
                AccessRights = AccessRights.EditCandidate,
                Description = "Right to edit a candidate",
                Group = "Candidates"
            },
            new Permission
            {
                AccessRights = AccessRights.EditEvent,
                Description = "Right to edit an event",
                Group = "Calendar"
            },
            new Permission
            {   AccessRights = AccessRights.EditRole,
                Description = "Right to edit a role",
                Group = "Roles"
            },
            new Permission
            {
                AccessRights = AccessRights.EditUserProfile,
                Description = "Right to edit user profile",
                Group = "Users"
            },
            new Permission
            {
                AccessRights = AccessRights.EditVacancy,
                Description = "Right to edit a vacancy",
                Group = "Vacancies"
            },
            new Permission
            {
                AccessRights = AccessRights.GenerateReports,
                Description = "Right to generate reports",
                Group = "Reports"
            },
            new Permission
            {
                AccessRights = AccessRights.InviteNewMember,
                Description = "Right to invite a new member to program",
                Group = "Users"
            },
            new Permission
            {
                AccessRights = AccessRights.RemoveCandidate,
                Description = "Right to remove candidate",
                Group = "Candidates"
            },
            new Permission
            {
                AccessRights = AccessRights.RemoveCandidateFromVacancy,
                Description = "Right to remove candidate from a vacancy",
                Group = "Vacancies"
            },
            new Permission
            {
                AccessRights = AccessRights.RemoveEvent,
                Description = "Right to remove event",
                Group = "Calendar"
            },
            new Permission
            {
                AccessRights = AccessRights.RemoveRole,
                Description = "Right to remove role",
                Group = "Roles"
            },
            new Permission
            {
                AccessRights = AccessRights.RemoveUserProfile,
                Description = "Right to remove user profile",
                Group = "Users"
            },
            new Permission
            {
                AccessRights = AccessRights.RemoveVacancy,
                Description = "Right to remove vacancy",
                Group = "Vacancies"
            },
            new Permission
            {
                AccessRights = AccessRights.SearchCandidatesInExternalSource,
                Description = "Right to search candidates on another work-searching sites",
                Group = "Candidates"
            },
            new Permission
            {
                AccessRights = AccessRights.SearchCandidatesInInternalSource,
                Description = "Right to search candidates inside the base",
                Group = "Candidates"
            },
            new Permission
            {
                AccessRights = AccessRights.SystemSetup,
                Description = "Right to provide system setup",
                Group = "System"
            },
            new Permission
            {
                AccessRights = AccessRights.ViewCalendar,
                Description = "Right to view a calendar",
                Group = "Calendar"
            },
            new Permission
            {
                AccessRights = AccessRights.ViewListOfCandidates,
                Description = "Right to view list of candidates",
                Group = "Candidates"
            },
            new Permission
            {
                AccessRights = AccessRights.ViewListOfVacancies,
                Description = "Right to view list of vacancies",
                Group = "Vacancies"
            },
            new Permission {AccessRights = AccessRights.ViewRoles, Description = "Right to view roles", Group = "Roles"},
            new Permission
            {
                AccessRights = AccessRights.ViewUserProfile,
                Description = "Right to view user profile",
                Group = "Users"
            },
            new Permission
            {
                AccessRights = AccessRights.ViewUsers,
                Description = "Right to view users",
                Group = "Users"
            }
        };

        public static readonly List<Stage> Stages = new List<Stage>
        {
            new Stage {Title = "Pool"},
            new Stage {Title = "Selected"},
            new Stage {Title = "HR Interview"},
            new Stage {Title = "Test task"},
            new Stage {Title = "Tech Interview"},
            new Stage {Title = "Additional interview"},
            new Stage {Title = "Final Interview"},
            new Stage {Title = "Job Offer Issued"},
            new Stage {Title = "Job Offer Accepted"},
            new Stage {Title = "Hired"},
            new Stage {Title = "Rejected"}
        };

        public static readonly List<Photo> Photos = new List<Photo>
        {
            new Photo {Description = "photo 1", ImagePath = @"~\images\ph11.jpg"},
            new Photo {Description = "photo 2", ImagePath = @"~\images\ph12.jpg"},
            new Photo {Description = "photo 3", ImagePath = @"~\images\ph13.jpg"},
            new Photo {Description = "photo 4", ImagePath = @"~\images\ph14.jpg"},
            new Photo {Description = "photo 5", ImagePath = @"~\images\ph15.jpg"},
            new Photo {Description = "photo 6", ImagePath = @"~\images\ph16.jpg"},
            new Photo {Description = "photo 7", ImagePath = @"~\images\ph17.jpg"},
            new Photo {Description = "photo 8", ImagePath = @"~\images\ph18.jpg"},
            new Photo {Description = "photo 9", ImagePath = @"~\images\ph19.jpg"}
        };

        public static readonly List<SocialNetwork> Socials = new List<SocialNetwork>
        {
            new SocialNetwork {ImagePath = @"~\images\fb.jpg", Title = "Facebook"},
            new SocialNetwork {ImagePath = @"~\images\vk.jpg", Title = "VK"},
            new SocialNetwork {ImagePath = @"~\images\li.jpg", Title = "LinkedIn"},
            new SocialNetwork {ImagePath = @"~\images\ms.jpg", Title = "MySpace"}
        };
        public static readonly List<EventType> Events = new List<EventType>
        {
            new EventType
            {
                Title = "HR Interview",
                ImagePath = @"~\images\HR.jpg"
            },
            new EventType
            {
                Title = "Tech Interview",
                ImagePath = @"~\images\msTech.jpg"
            },
            new EventType
            {
                Title = "Final Interview",
                ImagePath = @"~\images\Final.jpg"
            },
            new EventType
            {
                Title = "Candidate's Birthday",
                ImagePath = @"~\images\Birthday.jpg"
            },
            new EventType
            {
                Title = "Candidate's First Day",
                ImagePath = @"~\images\First.jpg"
            }
        };
        public static readonly List<CandidateSource> CandidateSources = new List<CandidateSource>
        {
            new CandidateSource {Source = Source.WorkUa, Path = "Path Work"},
            new CandidateSource {Source = Source.Djinni, Path = "Path Jinn"},
            new CandidateSource {Source = Source.RabotaUa, Path = "Path Rabota"}
        };

        private static List<Role> Roles = new List<Role>
        {
            new Role { Title="admin", Permissions = new List<Permission>() { Permissions[0],Permissions[1]} },
            new Role { Title = "user", Permissions = new List<Permission>() { Permissions[2], Permissions[3] } },
            new Role { Title = "guest", Permissions = new List<Permission>() { Permissions[4], Permissions[5] } }
        };

        public static readonly List<User> Users = new List<User>
        {
                    new User
                    {
                        BirthDate = new DateTime(1975, 3, 24) ,
                        Email = "first@user.ua",
                        FirstName = "Сергей",
                        isMale = true,
                        LastName = "Сергеев",
                        Location = Locations[0],
                        Login = "serg",
                        MiddleName = "Сергеевич",
                        Password = "sergpass",
                        PhoneNumbers = new List<PhoneNumber> { new PhoneNumber { Number = "099 999 9999" } },
                        Photo = Photos[0],
                        Role = Roles[0],
                        Skype = "first.skype"
                    },
                    new User
                    {
                        BirthDate = new DateTime(1985, 5, 1) ,
                        Email = "sec@user.ua",
                        FirstName = "Игорь",
                        isMale = true,
                        LastName = "Игоренко",
                        Location = Locations[1],
                        Login = "igor",
                        MiddleName = "Игоревич",
                        Password = "igpass",
                        PhoneNumbers = new List<PhoneNumber> { new PhoneNumber { Number = "088 888 9999" } },
                        Photo = Photos[1],
                        Role = Roles[1],
                        Skype = "sec.skype"
                    },
                    new User
                    {
                        BirthDate = new DateTime(1975, 4, 1) ,
                        Email = "third@user.ua",
                        FirstName = "Константин",
                        isMale = true,
                        LastName = "Константинченко",
                        Location = Locations[0],
                        Login = "serg",
                        MiddleName = "Константинович",
                        Password = "sergpass",
                        PhoneNumbers = new List<PhoneNumber> { new PhoneNumber { Number = "099 999 9999" } },
                        Photo = Photos[0],
                        Role = Roles[2],
                        Skype = "first.skype"
                    }
        };

        private static List<Vacancy> Vacancies = new List<Vacancy>
        {
                    new Vacancy
                    {
                        DeadlineDate = new DateTime(2016, 8, 1),
                        Department = Departments[0],
                        Description = "first vacancy description",
                        Industry = Industries[0],
                        LanguageSkill = LanguageSkills[0],
                        Levels = Levels.Take(1).ToList(),
                        Locations = Locations.Take(1).ToList(),
                        RequiredSkills = Skills.Take(2).ToList(),
                        Responsible = Users[0],
                        SalaryMax = 3000,
                        SalaryMin = 2500,
                        StartDate = new DateTime(2016,5,1),
                        Title = "Senior Java Developer",
                        TypeOfEmployment = TypeOfEmployment.FullTime,
                        EndDate = new DateTime(2016,7,11),
                        Tags = Tags.Take(3).ToList(),
                        State = EntityState.Open
                    },
                    new Vacancy
                    {
                        DeadlineDate = new DateTime(2016, 8, 11),
                        Department = Departments[1],
                        Description = "second vacancy description",
                        Industry = Industries[1],
                        LanguageSkill = LanguageSkills[1],
                        Levels = Levels.Take(1).ToList(),
                        Locations = Locations.Take(1).ToList(),
                        RequiredSkills = Skills.Take(2).ToList(),
                        Responsible = Users[0],
                        SalaryMax = 2000,
                        SalaryMin = 1500,
                        StartDate = new DateTime(2016,5,1),
                        Title = "Middle .NET Developer",
                        TypeOfEmployment = TypeOfEmployment.FullTime,
                        EndDate = new DateTime(2016,7,24),
                        Tags = Tags.Take(4).ToList(),
                        State = EntityState.Open
                    },
                    new Vacancy
                    {
                        DeadlineDate = new DateTime(2016, 11, 13),
                        Department = Departments[0],
                        Description = "Third vacancy description",
                        Industry = Industries[2],
                        LanguageSkill = LanguageSkills[2],
                        Levels = Levels.Take(1).ToList(),
                        Locations = Locations.Take(1).ToList(),
                        RequiredSkills = Skills.Take(2).ToList(),
                        Responsible = Users[0],
                        SalaryMax = 1000,
                        SalaryMin = 500,
                        StartDate = new DateTime(2016,5,1),
                        Title = "Junior Java Developer",
                        TypeOfEmployment = TypeOfEmployment.FullTime,
                        EndDate = new DateTime(2016,7,11),
                        Tags = Tags.Take(3).ToList(),
                        State = EntityState.Open
                    },
        };

        #endregion

        #region DbSets

        #endregion
    }
}
