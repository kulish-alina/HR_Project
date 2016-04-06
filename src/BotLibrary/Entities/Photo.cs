using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibrary.Entities
{
    public class Photo
    {
        public FileInfo Image { get; set; }
        public string Description { get; set; }
    }
}
