using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CVParser.Core.GatherStrategies
{
    public class SkypeStrategy : IStrategy
    {
        public IEnumerable<string> Execute(IEnumerable<IEnumerable<string>> information)
        {
            var skypeRegularExpression = new Regex(@"skype", RegexOptions.IgnoreCase);
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
            var clearExpression = new Regex(@"skype[ :]?[ -–]? ?", RegexOptions.IgnoreCase);
            return foundedSkypes.Select(x => clearExpression.Replace(x, string.Empty));
        }
    }
}
