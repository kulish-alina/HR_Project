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
        private static readonly List<string> names = File.ReadAllLines(@".\Data\names.txt").ToList();

        /// <summary>
        /// Collects the related data from logical block
        /// </summary>
        /// <param name="information">Information that will be disassembled</param>
        /// <returns>Returns the first and the last name that was parsed. AGREEMENT: by order the first name goes first, the last name goes second</returns>
        public IEnumerable<string> Execute(IEnumerable<IEnumerable<string>> information)
        {
            var foundedNameParts = new List<string>();
            var twoDotsRegexp = new Regex(":");
            var nameRegexp = new Regex(@"ім'я|name|имя|Ф\.?[И]\.?О\.?", RegexOptions.IgnoreCase);
            foreach (var infoBlock in information)
            {
                NameTriggeredOn nameTriggered = NameTriggeredOn.None;
                foreach (var infoLine in infoBlock)
                {
                    string clearedLine = infoLine;
                    nameTriggered = updateNameTriggerState(nameTriggered);
                    if (isLineHasNumbersOrMail(infoLine)) break;
                    if (twoDotsRegexp.IsMatch(infoLine))
                    {
                        if (nameRegexp.IsMatch(infoLine))
                        {
                            clearedLine = nameRegexp.Replace(infoLine, String.Empty);
                            clearedLine = twoDotsRegexp.Replace(clearedLine, String.Empty);
                            if (String.IsNullOrEmpty(clearedLine))
                            {
                                nameTriggered = NameTriggeredOn.PreviousIteration;
                            }
                            else
                            {
                                nameTriggered = NameTriggeredOn.CurrentIteration;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    var splittedLine = clearedLine.Trim(' ').Split(new char[] { ' ', '_' }).Where(x => !Regex.IsMatch(x, @"[*\s]"));
                    if (splittedLine.Count() <= 3)
                    {
                        foreach (var t in splittedLine)
                        {
                            var term = NickBuhro.Translit.Transliteration.CyrillicToLatin(t);
                            var relativeCoeffs = names.Select(name =>
                            {
                                double biggestTermLength = 0;
                                biggestTermLength = name.Length > term.Length ? name.Length : term.Length;
                                var distanceBetween = Levenshtein.CalculateDistance(name, term, 1);
                                double relativeCoeff = Math.Round((biggestTermLength - distanceBetween) / biggestTermLength, 2);
                                if (nameTriggered == NameTriggeredOn.CurrentIteration)
                                {
                                    relativeCoeff += 0.25;
                                }
                                return new { RelativeCoeff = relativeCoeff, Name = name, Term = term };
                            }).Where(x => x.RelativeCoeff > 0.8).ToList();
                            if (relativeCoeffs.Any())
                            {
                                var nameCapacitor = new StringBuilder(t);
                                foreach (var nameWord in splittedLine)
                                {
                                    if (nameWord != term)
                                    {
                                        nameCapacitor.Append(" " + nameWord);
                                    }
                                }
                                foundedNameParts.Add(nameCapacitor.ToString());
                            }
                        }
                    }
                }
            }
            return foundedNameParts;
        }

        private NameTriggeredOn updateNameTriggerState(NameTriggeredOn nameTriggered)
        {
            return nameTriggered == NameTriggeredOn.PreviousIteration ? NameTriggeredOn.CurrentIteration : NameTriggeredOn.None;
        }

        private bool isLineHasNumbersOrMail(string line)
        {
            return new Regex(@"\d").IsMatch(line) || new Regex(@"@").IsMatch(line);
        }

        private enum NameTriggeredOn
        {
            None,
            CurrentIteration,
            PreviousIteration
        }
    }
}
