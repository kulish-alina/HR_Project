using CVParser.Core;
using CVParser.CVStructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CVParser.Parser
{
    public class CVParser
    {
        private static Dictionary<SearchField, BlockType[]> blockWhereToSeekField = new Dictionary<SearchField, BlockType[]>
        {
            {
                SearchField.FirstLastName,
                new [] { BlockType.Personal, BlockType.None }
            },
            {
                SearchField.BirthDate,
                new [] { BlockType.Personal, BlockType.None }
            },
            {
                SearchField.ExperienceYears,
                new [] { BlockType.Experience }
            },
            {
                SearchField.Email,
                new [] { BlockType.Personal, BlockType.None }
            },
            {
                SearchField.PhoneNumbers,
                new [] { BlockType.Personal, BlockType.None }
            },
            {
                SearchField.Skype,
                new [] { BlockType.Personal, BlockType.None }
            }
        };


        /// <summary>
        /// Parses the content of given file
        /// </summary>
        /// <param name="path">Local path to the file</param>
        /// <returns>Returns the information that successfuly parsed</returns>
        /// <exception cref="ArgumentException">Thrown when file is not avalaible to parse (not docx, doc, odt)</exception>
        public static ParseResult Parse(string path)
        {

            var cvFileInfo = new FileInfo(path);
            ITextFileConverter converter = resolveConverterImplementation(cvFileInfo.Extension);
            var textElements = converter.Convert(cvFileInfo);
            var dividedContent = ContentDivider.Divide(textElements);
            ParseResult parseResult = new ParseResult()
            {
                Text = textElements.ToList()
            };
            Dictionary<string, SearchField> getheredInformation = new Dictionary<string, SearchField>();

            foreach (var fieldObject in Enum.GetValues(typeof(SearchField)))
            {
                var field = (SearchField)fieldObject;
                var blocksToSeek = resolveBlockByField(dividedContent, field);
                var gatheredList = DataGatherer.Gather(blocksToSeek, field);
                if (gatheredList.Any())
                {
                    getheredInformation.Add(gatheredList.FirstOrDefault(), field);
                }
            }
            foreach (var result in getheredInformation)
            {
                switch (result.Value)
                {
                    case SearchField.FirstLastName:
                        parseResult.FirstName = result.Key.Split(new char[] { ' ', '_' }).FirstOrDefault();
                        parseResult.LastName = result.Key.Split(new char[] { ' ', '_' }).Where(x => !String.IsNullOrEmpty(x)).Skip(1).FirstOrDefault();
                        break;
                    case SearchField.BirthDate:
                        parseResult.BirthDate = result.Key;
                        break;
                    case SearchField.ExperienceYears:
                        parseResult.ExperienceYears = result.Key;
                        break;
                    case SearchField.PhoneNumbers:
                        parseResult.PhoneNumber = result.Key;
                        break;
                    case SearchField.Email:
                        parseResult.Email = result.Key;
                        break;
                    case SearchField.Skype:
                        parseResult.Skype = result.Key;
                        break;
                    default:
                        break;
                }
            }
            return parseResult;
        }

        private static IEnumerable<LogicalBlock> resolveBlockByField(Content dividedContent, SearchField field)
        {
            return dividedContent.Blocks.Where(x => blockWhereToSeekField[field].Any(y => y == x.BlockType));

        }

        /// <summary>
        /// Resolves the type of converter
        /// </summary>
        /// <param name="extension">File extension</param>
        /// <returns>Returns the converter object wrapped with interface</returns>
        private static ITextFileConverter resolveConverterImplementation(string extension)
        {
            switch (extension)
            {
                case ".docx":
                    {
                        return new DocxToStringListConverter();
                    };
                case ".doc":
                    {
                        return new DocToStringListConverter();
                    };
                case ".odt":
                    {
                        return new OdtToStringListConverter();
                    }
                case ".pdf":
                    {
                        return new PdfToStringListConverter();
                    }
                default:
                    {
                        throw new ArgumentException("Given file is not avalaible to parse");
                    }
            }
        }
    }
}
