using DAL.DTO;
using System.Collections.Generic;


namespace DAL.Services
{
    public class CVParserService
    {
        public object Parse(string localPath)
        {
            var parseResult = CVParser.Parser.CVParser.Parse(localPath);
            return new
            {
                FirstName = parseResult.FirstName,
                LastName = parseResult.LastName,
                PhoneNumbers = new List<PhoneNumberDTO> { new PhoneNumberDTO { Number = parseResult.PhoneNumber } },
                ExperienceYears = parseResult.ExperienceYears,
                Email = parseResult.Email,
                Skype = parseResult.Skype,
                BirthDate = parseResult.BirthDate,
                Text = parseResult.Text
            };
        }
    }
}
