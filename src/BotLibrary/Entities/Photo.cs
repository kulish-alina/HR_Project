using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotDomain.Entities
{
    public class Photo: BaseEntity
    {
        public FileInfo Image { get; set; }
        public string Description { get; set; }
    }
}
