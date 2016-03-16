using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibrary.Entities
{
    public class User : BaseEntity
    {
        public PersonalInfo PersonalInfo { get; set; }
        public ContactInfo ContactInfo { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Location Location { get; set; }
        public Role Role { get; set; }
        public List<File> Files { get; set; }
    }
}
