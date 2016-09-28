using System.Collections.Generic;

namespace CVParser.CVStructure
{
    public class Content
    {
        public Content()
        {
            Blocks = new List<LogicalBlock>();
        }
        public List<LogicalBlock> Blocks { get; set; }
    }
}
