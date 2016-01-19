using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibrary.Entities
{
    public class PersonalInfo : BaseEntity
    {
        string FirstName { get; set; }
        string MiddleName { get; set; }
        string LastName { get; set; }
        bool Gender { get; set; }
        DateTime BirthDate { get; set; }
        Photo Photo { get; set; }
    }
}
