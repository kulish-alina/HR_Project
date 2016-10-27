using CVParser.CVStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CVParser.Core
{
    public class ContentDivider
    {
        /// <summary>
        /// Divides the list by the content
        /// </summary>
        /// <param name="textElements">List of all the lines of file</param>
        /// <returns>Returns object divided by content</returns>
        public static Content Divide(IEnumerable<String> textElements)
        {
            var content = new Content();
            var blocksMeet = new Dictionary<BlockType, bool>
            {
                {
                    BlockType.Skill, false
                },{
                    BlockType.Education, false
                },{
                    BlockType.Experience, false
                },{
                    BlockType.Additional, false
                },{
                    BlockType.Personal, false
                }
            };
            var logicalBlock = new LogicalBlock();
            var prevBlock = BlockType.None;
            for (int i = 0; i < textElements.Count(); i++)
            {
                var currentElementTextContent = textElements.ElementAt(i);
                if (currentElementTextContent.Contains("Spire.Doc")) continue;
                currentElementTextContent = (new Regex("[ \n]+").Replace(currentElementTextContent, " ").Trim(new char[] { ' ', '\n' }));
                if (!String.IsNullOrEmpty(currentElementTextContent))
                {
                    currentElementTextContent = clearTextContent(currentElementTextContent);
                    var splittedLine = currentElementTextContent.Trim(new char[] { ' ', '\0' }).Split(new char[] { ' ' });
                    var isLineShort = splittedLine.Length < 6;
                    Dictionary<Regex, BlockType> regExs = new Dictionary<Regex, BlockType>
                    {
                        {
                            new Regex(@"personal|info(rmation)?|about me|личн(ые)?|данные",    RegexOptions.IgnoreCase), BlockType.Personal },
                        {
                            new Regex(@"skills?|PROGRAMMING LANGUAGES|профессиональн(ые)?|навыки",       RegexOptions.IgnoreCase), BlockType.Skill },
                        {
                            new Regex(@"education|training|academics|образование",        RegexOptions.IgnoreCase), BlockType.Education },
                        {
                            new Regex(@"^work&|experience|history|опыт",           RegexOptions.IgnoreCase), BlockType.Experience },
                        {
                            new Regex(@"additional|summary",                  RegexOptions.IgnoreCase), BlockType.Additional }
                    };
                    var nonWord = new Regex(@"^\W", RegexOptions.IgnoreCase);
                    var punctuationRegExp = new Regex(@"\w\s?(\W\s\w|[-,.;])");
                    if (!nonWord.IsMatch(currentElementTextContent) && isLineShort && !punctuationRegExp.IsMatch(currentElementTextContent))
                    {
                        foreach (var regExAndBlockType in regExs)
                        {
                            if (regExAndBlockType.Key.IsMatch(currentElementTextContent))
                            {
                                if (prevBlock != regExAndBlockType.Value)
                                {
                                    if (logicalBlock.RelatedInformation.Count != 0)
                                    {
                                        if (blocksMeet.ContainsKey(regExAndBlockType.Value))
                                        {
                                            blocksMeet[regExAndBlockType.Value] = true;
                                        }
                                        prevBlock = regExAndBlockType.Value;
                                        content.Blocks.Add(logicalBlock);
                                        logicalBlock = new LogicalBlock();
                                        logicalBlock.BlockType = regExAndBlockType.Value;
                                    }
                                }
                            }
                        }
                    }
                    logicalBlock.RelatedInformation.Add(currentElementTextContent);
                }
            }
            if (logicalBlock.RelatedInformation.Count != 0)
            {
                content.Blocks.Add(logicalBlock);
            }
            if (!blocksMeet[BlockType.Personal])
            {
                var personalBlockRegExp = new Regex(@"Skype|Mail|Phone|Number|Name|Скайп|Почта|Телефон|Номер|Имя|Ім'я|Пошта", RegexOptions.IgnoreCase);
                foreach (var block in content.Blocks)
                {
                    foreach (var info in block.RelatedInformation)
                    {
                        if (personalBlockRegExp.IsMatch(info))
                        {
                            if (block.BlockType == BlockType.None)
                            {
                                blocksMeet[BlockType.Personal] = true;
                                block.BlockType = BlockType.Personal;
                            }
                        }
                    }
                }
            }
            return content;
        }
        private static string clearTextContent(string currentElem)
        {
            var clearedElem = (new Regex(@"\&").Replace(currentElem, "and"));
            clearedElem = Regex.Replace(clearedElem, "\x200f", "");
            clearedElem = Regex.Replace(clearedElem, "$rlm;", "");
            var lineBytes = Encoding.UTF8.GetBytes(clearedElem);
            if (lineBytes[0] == 226 && lineBytes[1] == 128 && lineBytes[2] == 143)
            {
                lineBytes = lineBytes.Skip(3).ToArray();
                lineBytes = lineBytes.Reverse().Skip(3).Reverse().ToArray();
                clearedElem = Encoding.UTF8.GetString(lineBytes);
            }
            return clearedElem;
        }
    }
}
