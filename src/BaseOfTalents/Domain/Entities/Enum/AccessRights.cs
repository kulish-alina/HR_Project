using System;

namespace Domain.Entities.Enum
{
    [Flags]
    public enum AccessRight
    {
        None = 0,
        InviteNewMember = 1,
        EditUserProfile = 2,
        ViewUsers = 4,
        ViewUserProfile = 8,
        RemoveUserProfile = 16,
        SystemSetup = 32,
        AddRole = 64,
        EditRole = 128,
        ViewRoles = 256,
        RemoveRole = 512,
        GenerateReports = 1024,
        AddVacancy = 2048,
        EditVacancy = 4096,
        ViewListOfVacancies = 8192,
        RemoveVacancy = 16384,
        AddCandidateToVacancy = 32768,
        RemoveCandidateFromVacancy = 65536,
        AddCandidate = 131072,
        EditCandidate = 262144,
        RemoveCandidate = 524288,
        ViewListOfCandidates = 1048576,
        SearchCandidatesInInternalSource = 2097152,
        SearchCandidatesInExternalSource = 4194304,
        ViewCalendar = 8388608,
        AddEvent = 16777216,
        EditEvent = 33554432,
        RemoveEvent = 67108864
    }
}