using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotDomain.Entities
{
    public class ContactInfo: BaseEntity
    {
        public List<string> PhoneNumbers { get; set; }
        public string Email { get; set; }
        public string Skype { get; set; }
    }
}
