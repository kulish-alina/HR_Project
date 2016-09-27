using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CVParser.Core.GatherStrategies
{
    public class PhoneNumberStrategy : IStrategy
    {
        public IEnumerable<string> Execute(IEnumerable<IEnumerable<string>> information)
        {
            var foundedPhones = new List<string>();
            var phoneRegularExpression = new Regex(@"(\+3-?8)?-?[(-]?[0-9]{3}[)-]?[0-9]{3}-?[0-9]{2}-?[0-9]{2}");
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
            return foundedPhones;
        }
    }
}
