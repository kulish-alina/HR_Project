using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Enum.Setup;
using Domain.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Migrations
{
    public static class DummyData
    {
        public static readonly List<Skill> Skills = new List<Skill>()
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

        public static readonly List<Tag> Tags = new List<Tag>()
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

        public static readonly List<Industry> Industries = new List<Industry>()
            {
                new Industry { Title="IT" },
                new Industry { Title="Accounting" },
                new Industry { Title="Administration" }
            };

        public static readonly List<Level> Levels = new List<Level>()
            {
                new Level { Title="Trainee" },
                new Level { Title="Junior" },
                new Level { Title="Middle" },
                new Level { Title="Senior" }
            };

        public static readonly List<DepartmentGroup> DepartmentGroups = new List<DepartmentGroup>()
            {
                new DepartmentGroup { Title="Contract" },
                new DepartmentGroup { Title="Nonprod" },
                new DepartmentGroup { Title="Prod" }
            };

        public static readonly List<Department> Departments = new List<Department>()
            {
                  new Department { Title = "Contract Programming",  DepartmentGroup = DepartmentGroups[0]},
                  new Department { Title = "Sites Design",          DepartmentGroup = DepartmentGroups[0]},
                  new Department { Title = "Web apps",              DepartmentGroup = DepartmentGroups[0]},
                  new Department { Title = "Accounting",            DepartmentGroup = DepartmentGroups[1]},
                  new Department { Title = "Administration",        DepartmentGroup = DepartmentGroups[1]},
                  new Department { Title = "Executives",            DepartmentGroup = DepartmentGroups[1]},
                  new Department { Title = "DevWorkshop",           DepartmentGroup = DepartmentGroups[1]},
                  new Department { Title = "HR",                    DepartmentGroup = DepartmentGroups[1]},
                  new Department { Title = "Managers",              DepartmentGroup = DepartmentGroups[1]},
                  new Department { Title = "Managers",              DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "QA",                    DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "UPM",                   DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "EPE",                   DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "Soft Web",              DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "AR",                    DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "Bank/Donor",            DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "iTMS",                  DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "Genetics 1",            DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "Genetics 2",            DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "Genetics 3",            DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "Genetics 5",            DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "Genetics Analysts",     DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "Genetics Autotesting",  DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "HLA",                   DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "Portal",                DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "Regr.Testing",          DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "Total QC",              DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "CM",                    DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "DBA",                   DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "MIS Tech Support",      DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "SA",                    DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "SE",                    DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "Architects",            DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "BI",                    DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "CSF",                   DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "Interfaces",            DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "Lab 5.0",               DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "LabMic",                DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "Mic 5.0",               DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "RNV",                   DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "Reports",               DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "Genetics Support",      DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "Support",               DepartmentGroup = DepartmentGroups[2]},
                  new Department { Title = "Update",                DepartmentGroup = DepartmentGroups[2]},
            };

        public static readonly List<Language> Languages = new List<Language>()
            {
                new Language { Title="English" },
                new Language { Title="Polish" }
            };

        public static readonly List<Country> Countries = new List<Country>()
            {
                new Country { Title="Ukraine" }
            };

        public static readonly List<Location> Locations = new List<Location>()
            {
                new Location { Country=Countries[0], Title="Kiev" },
                new Location { Country=Countries[0], Title="Kharkiv" },
                new Location { Country=Countries[0], Title="Odessa" },
                new Location { Country=Countries[0], Title="Dnipropetrovsk" },
                new Location { Country=Countries[0], Title="Zaporizhia" },
                new Location { Country=Countries[0], Title="Lviv" },
                new Location { Country=Countries[0], Title="Kryvyi Rih" },
                new Location { Country=Countries[0], Title="Mykolaiv" },
                new Location { Country=Countries[0], Title="Mariupol" },
                new Location { Country=Countries[0], Title="Luhansk" },
                new Location { Country=Countries[0], Title="Donetsk" },
                new Location { Country=Countries[0], Title="Sevastopol" },
                new Location { Country=Countries[0], Title="Vinnytsia" },
                new Location { Country=Countries[0], Title="Makiivka" },
                new Location { Country=Countries[0], Title="Simferopol" },
                new Location { Country=Countries[0], Title="Kherson" },
                new Location { Country=Countries[0], Title="Poltava" },
                new Location { Country=Countries[0], Title="Chernihiv" },
            };

        public static readonly List<Permission> Permissions = new List<Permission>()
            {
                new Permission { AccessRights=AccessRights.AddCandidate,                        Description = "Right to create a candidate",                                Group = "Candidates" },
                new Permission { AccessRights=AccessRights.AddCandidateToVacancy,               Description = "Right to attach exsisting candidate to a vacancy",           Group = "Vacancies" },
                new Permission { AccessRights=AccessRights.AddEvent,                            Description = "Right to create an event",                                   Group = "Calendar" },
                new Permission { AccessRights=AccessRights.AddRole,                             Description = "Right to create a role",                                     Group = "Roles" },
                new Permission { AccessRights=AccessRights.AddVacancy,                          Description = "Right to create a vacancy",                                  Group = "Vacancies" },
                new Permission { AccessRights=AccessRights.EditCandidate,                       Description = "Right to edit a candidate",                                  Group = "Candidates" },
                new Permission { AccessRights=AccessRights.EditEvent,                           Description = "Right to edit an event",                                     Group = "Calendar" },
                new Permission { AccessRights=AccessRights.EditRole,                            Description = "Right to edit a role",                                       Group = "Roles" },
                new Permission { AccessRights=AccessRights.EditUserProfile,                     Description = "Right to edit user profile",                                 Group = "Users" },
                new Permission { AccessRights=AccessRights.EditVacancy,                         Description = "Right to edit a vacancy",                                    Group = "Vacancies" },
                new Permission { AccessRights=AccessRights.GenerateReports,                     Description = "Right to generate reports",                                  Group = "Reports" },
                new Permission { AccessRights=AccessRights.InviteNewMember,                     Description = "Right to invite a new member to program",                    Group = "Users" },
                new Permission { AccessRights=AccessRights.RemoveCandidate,                     Description = "Right to remove candidate",                                  Group = "Candidates" },
                new Permission { AccessRights=AccessRights.RemoveCandidateFromVacancy,          Description = "Right to remove candidate from a vacancy",                   Group = "Vacancies" },
                new Permission { AccessRights=AccessRights.RemoveEvent,                         Description = "Right to remove event",                                      Group = "Calendar" },
                new Permission { AccessRights=AccessRights.RemoveRole,                          Description = "Right to remove role",                                       Group = "Roles" },
                new Permission { AccessRights=AccessRights.RemoveUserProfile,                   Description = "Right to remove user profile",                               Group = "Users" },
                new Permission { AccessRights=AccessRights.RemoveVacancy,                       Description = "Right to remove vacancy",                                    Group = "Vacancies" },
                new Permission { AccessRights=AccessRights.SearchCandidatesInExternalSource,    Description = "Right to search candidates on another work-searching sites", Group = "Candidates" },
                new Permission { AccessRights=AccessRights.SearchCandidatesInInternalSource,    Description = "Right to search candidates inside the base",                 Group = "Candidates" },
                new Permission { AccessRights=AccessRights.SystemSetup,                         Description = "Right to provide system setup",                              Group = "System" },
                new Permission { AccessRights=AccessRights.ViewCalendar,                        Description = "Right to view a calendar",                                   Group = "Calendar" },
                new Permission { AccessRights=AccessRights.ViewListOfCandidates,                Description = "Right to view list of candidates",                           Group = "Candidates" },
                new Permission { AccessRights=AccessRights.ViewListOfVacancies,                 Description = "Right to view list of vacancies",                            Group = "Vacancies" },
                new Permission { AccessRights=AccessRights.ViewRoles,                           Description = "Right to view roles",                                        Group = "Roles" },
                new Permission { AccessRights=AccessRights.ViewUserProfile,                     Description = "Right to view user profile",                                 Group = "Users" },
                new Permission { AccessRights=AccessRights.ViewUsers,                           Description = "Right to view users",                                        Group = "Users" }
            };

        public static readonly List<Stage> Stages = new List<Stage>()
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

        public static readonly List<Photo> Photos = new List<Photo>
            {
                new Photo {Description="photo 1", ImagePath=@"~\images\ph11.jpg" },
                new Photo {Description="photo 2", ImagePath=@"~\images\ph12.jpg" },
                new Photo {Description="photo 3", ImagePath=@"~\images\ph13.jpg" },
                new Photo {Description="photo 4", ImagePath=@"~\images\ph14.jpg" },
                new Photo {Description="photo 5", ImagePath=@"~\images\ph15.jpg" },
                new Photo {Description="photo 6", ImagePath=@"~\images\ph16.jpg" },
                new Photo {Description="photo 7", ImagePath=@"~\images\ph17.jpg" },
                new Photo {Description="photo 8", ImagePath=@"~\images\ph18.jpg" },
                new Photo {Description="photo 9", ImagePath=@"~\images\ph19.jpg" }
            };

        public static readonly List<Role> Roles = GetRoles(25);

        public static readonly List<User> Users = GetUsers(152);

        private static List<User> GetUsers(int count)
        {
            var users = new List<User>
            {

            };
        }

        private static List<Role> GetRoles(int count)
        {
            var roles = new List<Role>();

            for (int i = 0; i < count; i++)
            {
                roles.Add(new Role
                {
                    Title = "Role " + i,
                    Permissions = new List<Permission>
                    {
                        Permissions.GetRandom(),
                        Permissions.GetRandom(),
                        Permissions.GetRandom(),
                        Permissions.GetRandom()
                    }
                });
            }
            return roles;
        }

        static Random rnd = new Random();
        public static T GetRandom<T>(this List<T> list)
        {
            return list[rnd.Next(list.Count)];
        }
    }
}
