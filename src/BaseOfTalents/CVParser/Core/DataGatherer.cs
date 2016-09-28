using CVParser.Core.GatherStrategies;
using CVParser.CVStructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CVParser.Core
{
    public class DataGatherer
    {
        /// <summary>
        /// Collects the related data from logical block
        /// </summary>
        /// <param name="block">Block that contains BlockType and related information of block</param>
        /// <param name="field">Field that gatherer must to find</param>
        /// <returns>Returns the information that was parsed. Returns empty IEnumerable if there are no so information</returns>
        public static IEnumerable<string> Gather(IEnumerable<LogicalBlock> block, SearchField field)
        {
            return ResolveGatheringStrategyFor(field).Execute(block.Select(x => x.RelatedInformation));
        }

        private static IStrategy ResolveGatheringStrategyFor(SearchField searchField)
        {
            switch (searchField)
            {
                case SearchField.FirstLastName:
                    return new FirstLastNameStrategy();

                case SearchField.BirthDate:
                    return new BirthDateStrategy();

                case SearchField.ExperienceYears:
                    return new ExperienceStrategy();

                case SearchField.PhoneNumbers:
                    return new PhoneNumberStrategy();

                case SearchField.Email:
                    return new EmailStrategy();

                case SearchField.Skype:
                    return new SkypeStrategy();

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
