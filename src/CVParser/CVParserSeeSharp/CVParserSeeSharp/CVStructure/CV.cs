using System.Collections.Generic;

namespace CVParserSeeSharp.CVStructure
{
    public class CV
    {
        public CV()
        {
            Blocks = new List<LogicalBlock>();
        }
        public List<LogicalBlock> Blocks { get; set; }
    }
}
