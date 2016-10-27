using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CVParser.Core.GatherStrategies
{
    public class ExperienceStrategy : IStrategy
    {
        public IEnumerable<string> Execute(IEnumerable<IEnumerable<string>> information)
        {
            var dateRegEx = new Regex(@"(([0-3]?\d[–./-])?([01]\d[–./-])?(19|20)\d{2}|\w{3,}\s?(19|20)\d{2})\s*(-|–|to)\s*(([0-3]?\d[–./-])?([01]\d[–./-])?(19|20)\d{2}|(\w{3,}\s)?(19|20)\d{2}|(\w+ ?\w?))");
            var foundedDates = new List<string>();
            foreach (var list in information)
            {
                foreach (var line in list)
                {
                    var expRegexResult = dateRegEx.Matches(line);
                    if (expRegexResult.Count != 0)
                    {
                        var reg = new Regex(@"19\d{2}|20\d{2}");
                        var nowRegEx = new Regex(@"now|current|present|сейчас|сегодня|настоящее", RegexOptions.IgnoreCase);
                        var years = reg.Matches(expRegexResult[0].Value);
                        var present = nowRegEx.IsMatch(expRegexResult[0].Value);
                        if (present)
                        {
                            foundedDates.Add(DateTime.Now.Year.ToString());
                        }
                        foreach (Match year in years)
                        {
                            foundedDates.Add(year.Value);
                        }
                    }
                }
            }

            var intDates = foundedDates.Select(x => Convert.ToInt32(x)).ToList();
            if (intDates.Any())
            {
                var max = intDates.Max();
                var min = intDates.Min();
                return new List<string>() { (max - min).ToString() };
            }
            return new List<string>();
        }
    }
}
