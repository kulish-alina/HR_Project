using CVParserSeeSharp.CVStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CVParserSeeSharp
{
    public class LogicalPredictor
    {
        public static KeyValuePair<string, double> Predict(LogicalBlock block)
        {
            Dictionary<string, double> logicalBlocksChances = new Dictionary<string, double>();
            logicalBlocksChances.Add("personal", 0);
            logicalBlocksChances.Add("skill", 0);
            logicalBlocksChances.Add("experience", 0);
            logicalBlocksChances.Add("education", 0);
            logicalBlocksChances.Add("additional", 0);

            var wordsMeetCount = 0;
            double commonChanceValue = 0;

            var personalBlockRegExp = new Regex(@"Skype|Mail|Phone|Number|Name", RegexOptions.IgnoreCase);
            var personalKeyWordsAverageChance = 0.4;

            foreach (var info in block.RelatedInformation)
            {
                if (personalBlockRegExp.IsMatch(info))
                {
                    wordsMeetCount++;
                }
            }
            commonChanceValue = wordsMeetCount * personalKeyWordsAverageChance;
            logicalBlocksChances["personal"] = commonChanceValue;

            wordsMeetCount = 0;

            var skillBlockRegExp = new Regex(@"[Ss]kill");
            var skillKeyWordsAverageChance = 0.324;

            foreach (var info in block.RelatedInformation)
            {
                if (skillBlockRegExp.IsMatch(info))
                {
                    wordsMeetCount++;
                }
            }
            commonChanceValue = wordsMeetCount * skillKeyWordsAverageChance;
            logicalBlocksChances["skill"] = commonChanceValue;

            wordsMeetCount = 0;

            var experienceBlockRegExp = new Regex(@"project|role|job|tool|experience", RegexOptions.IgnoreCase);
            var experienceKeyWordsAverageChance = 0.257;
            foreach (var info in block.RelatedInformation)
            {
                if (experienceBlockRegExp.IsMatch(info))
                {
                    wordsMeetCount++;
                }
            }
            commonChanceValue = wordsMeetCount * experienceKeyWordsAverageChance;
            logicalBlocksChances["experience"] = commonChanceValue;

            wordsMeetCount = 0;

            var educationBlockRegExp = new Regex(@"University|[Ee]ducation|^(National)$");
            var educationKeyWordsAverageChance = 0.37;
            foreach (var info in block.RelatedInformation)
            {
                if (educationBlockRegExp.IsMatch(info))
                {
                    wordsMeetCount++;
                }
            }
            commonChanceValue = wordsMeetCount * educationKeyWordsAverageChance;
            logicalBlocksChances["education"] = commonChanceValue;

            double sumProbabilityChance = 0;
            foreach (var kvp in logicalBlocksChances)
            {
                if (kvp.Value != 0)
                {
                    sumProbabilityChance += kvp.Value;
                }
            }
            Dictionary<string, double> suspendedLogicalBlockChances = new Dictionary<string, double>();
            foreach (var kvp in logicalBlocksChances)
            {
                double suspendedChance = 0;
                if (kvp.Value != 0 && sumProbabilityChance != 0)
                {
                    suspendedChance = Math.Round((kvp.Value / sumProbabilityChance), 2);
                }
                suspendedLogicalBlockChances.Add(kvp.Key, suspendedChance);
            }
            var predictedBlock = suspendedLogicalBlockChances.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, y => y.Value).First();
            if (predictedBlock.Value == 0)
            {
                return new KeyValuePair<string, double>("additional", 1);
            }
            else
            {
                return predictedBlock;
            }
        }
    }
}
