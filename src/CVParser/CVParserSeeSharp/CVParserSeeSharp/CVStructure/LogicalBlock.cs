using System.Collections.Generic;
using System.Linq;

namespace CVParser.CVStructure
{
    public class LogicalBlock
    {
        public LogicalBlock()
        {
            RelatedInformation = new List<string>();
        }
        public BlockType BlockType { get; set; }
        public List<string> RelatedInformation { get; set; }

        public override string ToString()
        {
            return RelatedInformation.First();
        }
    }
}
