using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CVParser.Core.GatherStrategies
{
    public class BirthDateStrategy : IStrategy
    {
        public IEnumerable<string> Execute(IEnumerable<IEnumerable<string>> information)
        {
            var dateRegularExpressions = new List<Regex>
                {
                    new Regex(@"[0-3]?\d[-/.][01]\d[-/.](19[5-9]\d|20[01]\d)"),
                    new Regex(@"[01]\d[-/.][0-3]?\d[-/.](19[5-9]\d|20[01]\d)")
                };
            var dates = new List<string>();
            foreach (var list in information)
            {
                foreach (var line in list)
                {
                    foreach (var regex in dateRegularExpressions)
                    {
                        var dateRegExResultCollection = regex.Matches(line);
                        if (dateRegExResultCollection.Count != 0)
                        {
                            foreach (Match match in dateRegExResultCollection)
                            {
                                dates.Add(match.Value);
                            }
                        }
                    }
                    if (dates.Count == 0)
                    {
                        var yearRegEx = new Regex(@"(19[5-9]\d)");
                        var yearRegexResultCollection = yearRegEx.Matches(line);
                        if (yearRegexResultCollection.Count != 0)
                        {
                            dates.Add(yearRegexResultCollection[0].Value);
                        }
                    }
                }
            }
            return dates;
        }
    }
}
