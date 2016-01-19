using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibrary.Entities
{
    public class ContactInfo: BaseEntity
    {
        List<string> PhoneNumbers { get; set; }
        string Email { get; set; }
        string Skype { get; set; }
    }
}
