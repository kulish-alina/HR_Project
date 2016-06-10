using BaseOfTalents.Domain.Entities.Enum.Setup;
using System;
using System.Collections.Generic;

namespace BaseOfTalents.Domain.Entities
{
    public class User : BaseEntity
    {
        public User()
        {
            PhoneNumbers = new List<PhoneNumber>();
        }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool isMale { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Email { get; set; }
        public string Skype { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        public virtual Photo Photo { get; set; }

        public int LocationId { get; set; }
        public virtual Location Location { get; set; }

        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }
    }
}