let _$q;
export default class RoleService {
   constructor($q) {
      'ngInject';
      _$q = $q;
   }
   getPermissions() {
      return _$q.when({
         InviteNewMember :                  { id:	1,  description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' },
         EditUserProfile :                  { id:	2,  description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' },
         ViewUsers :                        { id:	3,  description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' },
         ViewUserProfile :                  { id:	4,  description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' },
         RemoveUserProfile :                { id:	5,  description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' },
         SystemSetup :                      { id:	6,  description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' },
         AddRole :                          { id:	7,  description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' },
         EditRole :                         { id:	8,  description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' },
         ViewRoles :                        { id:	9,  description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' },
         RemoveRole :                       { id:	10, description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' },
         GenerateReports :                  { id:	11, description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' },
         AddVacancy :                       { id:	12, description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' },
         EditVacancy :                      { id:	13, description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' },
         ViewListOfVacancies :              { id:	14, description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' },
         RemoveVacancy :                    { id:	15, description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' },
         AddCandidateToVacancy :            { id:	16, description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' },
         RemoveCandidateFromVacancy :       { id:	17, description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' },
         AddCandidate :                     { id:	18, description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' },
         EditCandidate :                    { id:	19, description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' },
         RemoveCandidate :                  { id:	20, description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' },
         ViewListOfCandidates :             { id:	21, description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' },
         SearchCandidatesInInternalSource : { id:	22, description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' },
         SearchCandidatesInExternalSource : { id:	23, description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' },
         ViewCalendar :                     { id:	24, description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' },
         AddEvent :                         { id:	25, description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' },
         EditEvent :                        { id:	26, description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' },
         RemoveEvent :                      { id:	27, description:
         'Lorem Ipsum is simply dummy text of the printing and typesetting industry. ' }
      });
   }
   getRoles () {
      //ThesaurusService.getThesaursTopics('roles');
      return {
         Administrator : 268435454,
         Manager       : 56788558,
         Frelancer     : 5565844
      };
   }

   saveRole(role) {
      console.log('role saved', role);
   }
}
