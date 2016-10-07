using System.Collections.Generic;

namespace CVParser.Core.GatherStrategies
{
    public interface IStrategy
    {
        /// <summary>
        /// Collects the related data from logical block
        /// </summary>
        /// <param name="information">Information that will be disassembled</param>
        /// <returns>Returns the information that was parsed. Returns empty IEnumerable if there are no so information</returns>
        IEnumerable<string> Execute(IEnumerable<IEnumerable<string>> information);
    }
}
