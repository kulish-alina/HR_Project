using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CVParser.Core.GatherStrategies
{
    public class PhoneNumberStrategy : IStrategy
    {
        public IEnumerable<string> Execute(IEnumerable<IEnumerable<string>> information)
        {
            var foundedPhones = new List<string>();
            var phoneRegularExpression = new Regex(@"[(]?0\s?[()]?\s?\d\d[)\s-]?\s?\d\d\d[\s-]?\d\d[\s-]?\d\d");
            foreach (var list in information)
            {
                foreach (var line in list)
                {
                    var phoneMatchCollection = phoneRegularExpression.Matches(line);
                    if (phoneMatchCollection.Count != 0)
                    {
                        foreach (Match match in phoneMatchCollection)
                        {
                            foundedPhones.Add(match.Value);
                        }
                    }
                }
            }
            var clearExpression = new Regex(@"[\s()-]", RegexOptions.IgnoreCase);
            return foundedPhones.Select(x => clearExpression.Replace(x, string.Empty)).Select(x => string.Format("+38{0}", x));
        }
    }
}
