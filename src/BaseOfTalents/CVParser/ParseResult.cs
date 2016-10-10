using System;
using System.Collections.Generic;

namespace CVParser
{
    public class ParseResult
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public string ExperienceYears { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Skype { get; set; }
        public List<string> Text { get; set; }

        public double CalculateParseResultPercent()
        {
            double maxSumOfWeights = 2.2;
            double actualSumOfWeights = 0;

            if (!String.IsNullOrEmpty(FirstName))
            {
                actualSumOfWeights += 0.375;
            }
            if (!String.IsNullOrEmpty(LastName))
            {
                actualSumOfWeights += 0.375;
            }
            if (!String.IsNullOrEmpty(ExperienceYears))
            {
                actualSumOfWeights += 0.75;
            }
            if (!String.IsNullOrEmpty(Skype))
            {
                actualSumOfWeights += 0.25;
            }
            if (!String.IsNullOrEmpty(Email))
            {
                actualSumOfWeights += 0.15;
            }
            if (!String.IsNullOrEmpty(PhoneNumber))
            {
                actualSumOfWeights += 0.15;
            }
            if (!String.IsNullOrEmpty(BirthDate))
            {
                actualSumOfWeights += 0.15;
            }
            var percentage = Math.Round(actualSumOfWeights / maxSumOfWeights, 2);
            return percentage;
        }

        public override string ToString()
        {
            return CalculateParseResultPercent().ToString();
        }
    }
}
