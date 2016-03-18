using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotDomain.Entities
{
    public class File: BaseEntity
    {
        public FileInfo FileInfo { get; set; }
        public string Description { get; set; }
    }
}
