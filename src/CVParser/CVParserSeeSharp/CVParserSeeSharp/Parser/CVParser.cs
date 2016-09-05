using CVParserSeeSharp.CVStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CVParserSeeSharp.Parser
{
    public class CVParser
    {
        static bool lastWasEmpty = false;
        static bool lastOperationWasSeparation = true;

        static List<string> keyWords = new List<string> {
                "skill", "personal", "information", "info", "work", "experience", "education", "languag", "about me", "summary", "history", "techincal", "knowledge"
            };

        public static CV Parse(List<string> textElements)
        {
            var cv = new CV();
            var logicalBlock = new LogicalBlock();
            for (int i = 0; i < textElements.Count(); i++)
            {
                var currentElementTextContent = textElements[i];
                if (!String.IsNullOrEmpty(currentElementTextContent))
                {
                    if (LineIsLogicalSeparator(currentElementTextContent))
                    {
                        if (logicalBlock.RelatedInformation.Count != 0)
                        {
                            cv.Blocks.Add(logicalBlock);
                            logicalBlock = new LogicalBlock();
                        }
                        lastOperationWasSeparation = true;
                    }
                    else
                    {
                        lastOperationWasSeparation = false;
                    }
                    lastWasEmpty = false;
                    logicalBlock.RelatedInformation.Add(currentElementTextContent);
                }
                else
                {
                    lastWasEmpty = true;
                    //lastOperationWasSeparation = false;
                }
            }
            if (logicalBlock.RelatedInformation.Count != 0)
            {
                cv.Blocks.Add(logicalBlock);
            }
            return cv;
        }


        private static bool LineIsLogicalSeparator(string line)
        {
            var replacedLine = Regex.Replace(line, @" ?/ ?", ", ");
            replacedLine = Regex.Replace(replacedLine, "\x200f", "");
            var splittedLine = replacedLine.Trim(new char[] { ' ', '\0' }).Split(new char[] { ' ' });
            var isLineShort = splittedLine.Length < 6;

            var hasNotSplitWordsRegExp = new Regex(@"descript|сompan|manage|java|develop|role|customer|proj|durati|respons|duties|objective|^\d+$", RegexOptions.IgnoreCase);
            var punctuationRegExp = new Regex(@"\w\s?(\W\s\w|[-,.;])");
            var hasNonWordBeforeText = new Regex(@"^\W");

            var shoto = punctuationRegExp.Matches(replacedLine);
            var shoto2 = hasNotSplitWordsRegExp.Matches(replacedLine);
            var shoto3 = hasNonWordBeforeText.Matches(replacedLine);

            var regularExpSift = !punctuationRegExp.IsMatch(replacedLine) && !hasNotSplitWordsRegExp.IsMatch(replacedLine) && !hasNonWordBeforeText.IsMatch(replacedLine);
            var lineShortAndLastOPWasNotSeparation = isLineShort && !lastOperationWasSeparation;
            var isLogicalSeparator = regularExpSift
                && (keyWords.Any(x => replacedLine.ToLower().Contains(x))
                || lastWasEmpty);
            isLogicalSeparator = isLogicalSeparator && lineShortAndLastOPWasNotSeparation;

            return ((new Regex(@"skills?", RegexOptions.IgnoreCase)).IsMatch(line) || (new Regex(@"experience", RegexOptions.IgnoreCase)).IsMatch(line)) && isLineShort;
        }
    }
}
