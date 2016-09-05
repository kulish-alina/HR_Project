using System.Collections.Generic;

namespace CVParserSeeSharp.CVStructure
{
    public class LogicalCV
    {
        public LogicalCV()
        {
            PersonalInformation = new List<string>();
            Skills = new List<string>();
            Education = new List<string>();
            Experience = new List<string>();
            AdditionalInformation = new List<string>();
        }
        public List<string> PersonalInformation { get; set; }
        public List<string> Skills { get; set; }
        public List<string> Education { get; set; }
        public List<string> Experience { get; set; }
        public List<string> AdditionalInformation { get; set; }
    }
}
