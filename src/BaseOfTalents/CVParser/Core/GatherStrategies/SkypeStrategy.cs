using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CVParser.Core.GatherStrategies
{
    public class SkypeStrategy : IStrategy
    {
        public IEnumerable<string> Execute(IEnumerable<IEnumerable<string>> information)
        {
            var skypeRegularExpression = new Regex(@"skype|скайп", RegexOptions.IgnoreCase);
            var foundedSkypes = new List<string>();
            foreach (var list in information)
            {
                foreach (var line in list)
                {
                    var skypeMatchCollection = skypeRegularExpression.Matches(line);
                    if (skypeMatchCollection.Count != 0)
                    {
                        foundedSkypes.Add(line);
                    }
                }
            }
            var getSkypeExpression = new Regex(@"[.*\s-]?skype\s?[\s-:–]?[\s–]?(\w+\W?\w*)?", RegexOptions.IgnoreCase);
            var clearExpression = new Regex(@"[.*\s-]?skype\s?[\s-:–]?[\s–]?", RegexOptions.IgnoreCase);
            return foundedSkypes
                .Select(x => getSkypeExpression.Match(x).Value)
                .Select(x => clearExpression.Replace(x, string.Empty));
        }
    }
}
