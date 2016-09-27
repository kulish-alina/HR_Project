using MinimumEditDistance;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CVParser.Core.GatherStrategies
{
    public class FirstLastNameStrategy : IStrategy
    {
        private static readonly List<string> names = File.ReadAllLines(@"d:\parserDirectory\names.txt").ToList();

        public IEnumerable<string> Execute(IEnumerable<IEnumerable<string>> information)
        {
            var foundedNameParts = new List<string>();
            var numbersRegexp = new Regex(@"\d");
            var atRegexp = new Regex(@"@");
            var twoDotsRegexp = new Regex(":");
            var nameRegexp = new Regex(@"name", RegexOptions.IgnoreCase);
            foreach (var list in information)
            {
                NameTriggeredOn nameTriggered = NameTriggeredOn.None;
                foreach (var line in list)
                {
                    if (nameTriggered == NameTriggeredOn.Previous)
                    {
                        nameTriggered = NameTriggeredOn.Current;
                    }
                    else
                    {
                        nameTriggered = NameTriggeredOn.None;
                    }
                    string clearedLine = line;
                    if (numbersRegexp.IsMatch(line) || atRegexp.IsMatch(line)) break;
                    if (twoDotsRegexp.IsMatch(line))
                    {
                        if (nameRegexp.IsMatch(line))
                        {
                            clearedLine = nameRegexp.Replace(line, "");
                            clearedLine = twoDotsRegexp.Replace(clearedLine, String.Empty);
                            if (String.IsNullOrEmpty(clearedLine))
                            {
                                nameTriggered = NameTriggeredOn.Previous;
                            }
                            else
                            {
                                nameTriggered = NameTriggeredOn.Current;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    var splittedLine = clearedLine.Trim(' ').Split(new char[] { ' ', '_' });
                    if (splittedLine.Count() <= 3)
                    {
                        foreach (var term in splittedLine)
                        {
                            var distansec = names.Select(x => new { Distance = Levenshtein.CalculateDistance(x, term, 1), Name = x, Term = term });
                            var relativeCoffs = distansec.Select(x =>
                            {
                                double biggestTermLength = 0;
                                if (x.Name.Length > x.Term.Length)
                                {
                                    biggestTermLength = x.Name.Length;
                                }
                                else {
                                    biggestTermLength = x.Term.Length;
                                }
                                double relativeCoeff = Math.Round((biggestTermLength - x.Distance) / biggestTermLength, 2);
                                if (nameTriggered == NameTriggeredOn.Current)
                                {
                                    relativeCoeff += 0.25;
                                }
                                return new { RelativeCoeff = relativeCoeff, Name = x.Name, Term = term };
                            }).Where(x => x.RelativeCoeff > 0.8).ToList();

                            if (relativeCoffs.Count > 0)
                            {
                                var conductedName = new StringBuilder(term);
                                foreach (var nameWord in splittedLine)
                                {
                                    if (nameWord != term)
                                    {
                                        conductedName.Append(" " + nameWord);
                                    }
                                }
                                foundedNameParts.Add(conductedName.ToString());
                            }
                        }
                    }
                }
            }
            return foundedNameParts;
        }

        enum NameTriggeredOn
        {
            None,
            Current,
            Previous
        }
    }
}
