using System.Collections.Generic;

namespace CVParserSeeSharp.CVStructure
{
    public class LogicalBlock
    {
        public LogicalBlock()
        {
            RelatedInformation = new List<string>();
        }
        public List<string> RelatedInformation { get; set; }
    }
}
