using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class FileModel
    {
        public string FilePath { get; set; }
        public string Description { get; set; }
        public long Size { get; set; }
    }
}