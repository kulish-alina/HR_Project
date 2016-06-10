namespace BaseOfTalents.Domain.Entities.Enum
{
    public enum AccessRights
    {
        InviteNewMember = 1,
        EditUserProfile,
        ViewUsers,
        ViewUserProfile,
        RemoveUserProfile,
        SystemSetup,
        AddRole,
        EditRole,
        ViewRoles,
        RemoveRole,
        GenerateReports,
        AddVacancy,
        EditVacancy,
        ViewListOfVacancies,
        RemoveVacancy,
        AddCandidateToVacancy,
        RemoveCandidateFromVacancy,
        AddCandidate,
        EditCandidate,
        RemoveCandidate,
        ViewListOfCandidates,
        SearchCandidatesInInternalSource,
        SearchCandidatesInExternalSource,
        ViewCalendar,
        AddEvent,
        EditEvent,
        RemoveEvent
    }
}