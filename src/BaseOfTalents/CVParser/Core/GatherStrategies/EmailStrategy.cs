using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CVParser.Core.GatherStrategies
{
    public class EmailStrategy : IStrategy
    {
        public IEnumerable<string> Execute(IEnumerable<IEnumerable<string>> information)
        {
            var foundedEmails = new List<string>();
            var emailRegularExpression = new Regex(@"[\w.]{3,}@\w+(\.\w{2,3})+");
            foreach (var list in information)
            {
                foreach (var line in list)
                {
                    var emailMatchCollection = emailRegularExpression.Matches(line);
                    if (emailMatchCollection.Count != 0)
                    {
                        foreach (Match match in emailMatchCollection)
                        {
                            foundedEmails.Add(match.Value);
                        }
                    }
                }
            }
            return foundedEmails;
        }
    }
}
