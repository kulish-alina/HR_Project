using System.Collections.Generic;

namespace DAL.LoggerCore
{
    /// <summary>
    /// Contains the settings for Logger 
    /// </summary>
    public class LogConfig
    {
        public static List<string> FieldToLog
        {
            get
            {
                return fieldToLog;
            }
        }

        private static List<string> fieldToLog = new List<string>
        {
            "FirstName",
            "LastName",
            "PositionDesired",
            "CityId",
            "VacanciesProgress",
            "Title",
            "Description",
            "EndDate",
            "DepartmentId",
            "ResponsibleId",
            "StatesInfo"
        };
    }
}
