using System.Collections;
using System.Collections.Generic;
using BaseOfTalents.DAL;
using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities.Enum;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using DAL.Extensions;
using Domain.DTO.DTOModels;
using NUnit.Framework;

namespace Tests.Extensions
{
    [TestFixture]
    public class RoleExtensionTest : BaseTest
    {
        private IUnitOfWork _uow;
        private IEnumerable<Permission> _permissions = Data.Permissions;

        [SetUp]
        public void Init()
        {
            System.Diagnostics.Debug.WriteLine("SetUp");
            context = GenerateNewContext();
            context.Permissions.AddRange(_permissions);
            context.SaveChanges();
            _uow = new UnitOfWork(context);
        }

        [TearDown]
        public void Drop()
        {
            context.Database.Delete();
            context = null;
        }

        [Test, TestCaseSource(typeof(AccessRightsTestFactory), "Cases")]
        public void NumberCollectionMatching(int right, IEnumerable<Permission> expectedPermissions)
        {
            var roleDTO = new RoleDTO()
            {
                Permissions = right,
            };

            var role = new Role();
            role.Update(roleDTO, _uow);

            CollectionAssert.AreEquivalent(role.Permissions, expectedPermissions);
        }
    }

    public class AccessRightsTestFactory
    {
        private static List<Permission> permissions = Data.Permissions;

        public static IEnumerable Cases
        {
            get
            {
                yield return new TestCaseData((int)(AccessRight.AddCandidate | AccessRight.AddEvent | AccessRight.EditRole),
                    new List<Permission>()
                    {
                        permissions[0],
                        permissions[2],
                        permissions[7]
                    })
                    .SetDescription("Check on getting from a number list of permissions like: AddCandidate, AddEvent, EditRole")
                    .SetName("AddCandidate, addEvent, editRole access right test")
                    .SetCategory("Role update test");

                yield return new TestCaseData((int)(AccessRight.InviteNewMember),
                    new List<Permission>()
                    {
                        permissions[11]
                    })
                    .SetDescription("Check on getting from a number InviteNewMember permission")
                    .SetName("InviteNewMember access right test")
                    .SetCategory("Role update test");
                yield return new TestCaseData(0,
                   new List<Permission>()
                   {
                       permissions[27]
                   })
                   .SetDescription("Check none state access right test on passing")
                   .SetName("None access right test")
                   .SetCategory("Role update test");
            }
        }
    }

    public static class Data
    {
        public static List<Permission> Permissions = new List<Permission>() {
        new Permission
            {
                AccessRights = AccessRight.AddCandidate,
                Description = "Right to create a candidate",
                Group = "Candidates"  //0
            },
            new Permission
            {
                AccessRights = AccessRight.AddCandidateToVacancy,
                Description = "Right to attach exsisting candidate to a vacancy",
                Group = "Vacancies" //1
            },
            new Permission
            {
                AccessRights = AccessRight.AddEvent,
                Description = "Right to create an event",
                Group = "Calendar" //2
            },
            new Permission
            {
                AccessRights = AccessRight.AddRole,
                Description = "Right to create a role",
                Group = "Roles" //3
            },
            new Permission
            {
                AccessRights = AccessRight.AddVacancy,
                Description = "Right to create a vacancy",
                Group = "Vacancies" //4
            },
            new Permission
            {
                AccessRights = AccessRight.EditCandidate,
                Description = "Right to edit a candidate",
                Group = "Candidates" //5
            },
            new Permission
            {
                AccessRights = AccessRight.EditEvent,
                Description = "Right to edit an event",
                Group = "Calendar" //6
            },
            new Permission
            {   AccessRights = AccessRight.EditRole,
                Description = "Right to edit a role",
                Group = "Roles" //7
            },
            new Permission
            {
                AccessRights = AccessRight.EditUserProfile,
                Description = "Right to edit user profile",
                Group = "Users" //8
            },
            new Permission
            {
                AccessRights = AccessRight.EditVacancy,
                Description = "Right to edit a vacancy",
                Group = "Vacancies" //9
            },
            new Permission
            {
                AccessRights = AccessRight.GenerateReports,
                Description = "Right to generate reports",
                Group = "Reports" //10
            },
            new Permission
            {
                AccessRights = AccessRight.InviteNewMember,
                Description = "Right to invite a new member to program",
                Group = "Users" //11
            },
            new Permission
            {
                AccessRights = AccessRight.RemoveCandidate,
                Description = "Right to remove candidate",
                Group = "Candidates" //12
            },
            new Permission
            {
                AccessRights = AccessRight.RemoveCandidateFromVacancy,
                Description = "Right to remove candidate from a vacancy",
                Group = "Vacancies" //13
            },
            new Permission
            {
                AccessRights = AccessRight.RemoveEvent,
                Description = "Right to remove event",
                Group = "Calendar" //14
            },
            new Permission
            {
                AccessRights = AccessRight.RemoveRole,
                Description = "Right to remove role",
                Group = "Roles" //15
            },
            new Permission
            {
                AccessRights = AccessRight.RemoveUserProfile,
                Description = "Right to remove user profile",
                Group = "Users" //16
            },
            new Permission
            {
                AccessRights = AccessRight.RemoveVacancy,
                Description = "Right to remove vacancy",
                Group = "Vacancies" //17
            },
            new Permission
            {
                AccessRights = AccessRight.SearchCandidatesInExternalSource,
                Description = "Right to search candidates on another work-searching sites",
                Group = "Candidates" //18
            },
            new Permission
            {
                AccessRights = AccessRight.SearchCandidatesInInternalSource,
                Description = "Right to search candidates inside the base",
                Group = "Candidates" //19
            },
            new Permission
            {
                AccessRights = AccessRight.SystemSetup,
                Description = "Right to provide system setup",
                Group = "System" //20
            },
            new Permission
            {
                AccessRights = AccessRight.ViewCalendar,
                Description = "Right to view a calendar",
                Group = "Calendar" //21
            },
            new Permission
            {
                AccessRights = AccessRight.ViewListOfCandidates,
                Description = "Right to view list of candidates",
                Group = "Candidates" //22
            },
            new Permission
            {
                AccessRights = AccessRight.ViewListOfVacancies,
                Description = "Right to view list of vacancies",
                Group = "Vacancies" //23
            },
            new Permission
            {
                AccessRights = AccessRight.ViewRoles,
                Description = "Right to view roles",
                Group = "Roles" //24
            },
            new Permission
            {
                AccessRights = AccessRight.ViewUserProfile,
                Description = "Right to view user profile",
                Group = "Users" //25
            },
            new Permission
            {
                AccessRights = AccessRight.ViewUsers,
                Description = "Right to view users",
                Group = "Users" //26
            },
            new Permission
            {
                AccessRights = AccessRight.None,
                Description = "Right on getting access to nothing",
                Group = "Empty" //27
            }
        };
    }
}
