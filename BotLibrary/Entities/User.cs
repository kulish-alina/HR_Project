using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibrary.Entities
{
    public class User : BaseEntity
    {
        PersonalInfo PersonalInfo { get; set; }
        ContactInfo ContactInfo { get; set; }
        string Login { get; set; }
        string Password { get; set; }
        Location Location { get; set; }
        Role Role { get; set; }
        List<File> Files { get; set; }
    }
}
