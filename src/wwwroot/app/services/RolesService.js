import {
   reduce,
   assign
} from 'lodash';

let _$q;
export default class RoleService {
   constructor($q) {
      'ngInject';
      _$q = $q;
   }
   getPermissions() {
      return _getPerm().then((perm) => {
         return reduce(perm, (memo, permis, key) => {
            const memoObj = assign({name: key}, permis);
            memo[permis.group] = memo[permis.group] || [];
            memo[permis.group].push(memoObj);
            return memo;
         },{});
      });
   }
   getRoles () {
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
function _getPerm() {
   return _$q.when({
      InviteNewMember :
      {
         id: 1,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'Users'
      },
      EditUserProfile :
      {
         id: 2,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'Users'
      },
      ViewUsers :
      {
         id: 3,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'Users'
      },
      ViewUserProfile : {
         id: 4,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'Users'
      },
      RemoveUserProfile :
      {
         id: 5,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'Users'
      },
      SystemSetup :
      {
         id: 6,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'System'
      },
      AddRole :
      {
         id: 7,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'Roles'
      },
      EditRole :
      {
         id: 8,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'Roles'
      },
      ViewRoles :
      {
         id: 9,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'Roles'
      },
      RemoveRole :
      {
         id: 10,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'Roles'
      },
      GenerateReports :
      {
         id: 11,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'Reports'
      },
      AddVacancy :
      {
         id: 12,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'Vacancies'
      },
      EditVacancy :
      {
         id:	13,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'Vacancies'
      },
      ViewListOfVacancies :
      {
         id: 14,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'Vacancies'
      },
      RemoveVacancy :
      {
         id: 15,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'Vacancies'
      },
      AddCandidateToVacancy :
      {
         id: 16,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'Vacancies'
      },
      RemoveCandidateFromVacancy :
      {
         id: 17,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'Vacancies'
      },
      AddCandidate :
      {
         id: 18,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'Candidates'
      },
      EditCandidate :
      {
         id: 19,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'Candidates'
      },
      RemoveCandidate :
      {
         id: 20,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'Candidates'
      },
      ViewListOfCandidates :
      {
         id: 21,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'Candidates'
      },
      SearchCandidatesInInternalSource :
      {
         id: 22,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'Candidates'
      },
      SearchCandidatesInExternalSource :
      {
         id: 23,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'Candidates'
      },
      ViewCalendar :
      {
         id: 24,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'Calendar'
      },
      AddEvent :
      {
         id: 25,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'Calendar'
      },
      EditEvent :
      {
         id: 26,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'Calendar'
      },
      RemoveEvent :
      {
         id: 27,
         description: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry.',
         group: 'Calendar'
      }
   });
}
