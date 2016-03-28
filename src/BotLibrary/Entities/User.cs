using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibrary.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public Photo Photo { get; set; }
        public List<string> PhoneNumbers { get; set; }
        public string Email { get; set; }
        public string Skype { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Location Location { get; set; }
        public Role Role { get; set; }
        public List<File> Files { get; set; }
    }
}
