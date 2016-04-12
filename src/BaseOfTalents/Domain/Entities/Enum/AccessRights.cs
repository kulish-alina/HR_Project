using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enum
{
    public enum AccessRights
    {
        InviteNewMember,
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
