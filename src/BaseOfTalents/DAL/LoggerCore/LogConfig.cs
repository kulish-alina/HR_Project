using System.Collections.Generic;

namespace DAL.LoggerCore
{
    /// <summary>
    /// Contains the settings for Logger 
    /// </summary>
    public class LogConfig
    {
        public static List<LogginField> FieldsToLog
        {
            get
            {
                return fieldsToLog;
            }
        }

        private static List<LogginField> fieldsToLog = new List<LogginField>
        {
            new LogginField
            {
                DomainFieldName = "FirstName",
                DTOFieldName = "FirstName"
            },
            new LogginField
            {
                DomainFieldName = "LastName",
                DTOFieldName = "LastName"
            },
            new LogginField
            {
                DomainFieldName = "PositionDesired",
                DTOFieldName = "PositionDesired"
            },
            new LogginField
            {
                DomainFieldName = "CityId",
                DTOFieldName = "CityId"
            },
            new LogginField
            {
                DomainFieldName = "Title",
                DTOFieldName = "Title"
            },
            new LogginField
            {
                DomainFieldName = "Progress",
                DTOFieldName = "Progress"
            },
            new LogginField
            {
                DomainFieldName = "Description",
                DTOFieldName = "Description"
            },
            new LogginField
            {
                DomainFieldName = "EndDate",
                DTOFieldName = "EndDate"
            },
            new LogginField
            {
                DomainFieldName = "DepartmentId",
                DTOFieldName = "DepartmentId"
            },
            new LogginField
            {
                DomainFieldName = "ResponsibleId",
                DTOFieldName = "ResponsibleId"
            },
            new LogginField
            {
                DomainFieldName = "State",
                DTOFieldName = "State"
            },
            new LogginField
            {
                DomainFieldName = "Levels",
                DTOFieldName = "LevelIds"
            },new LogginField
            {
                DomainFieldName = "Cities",
                DTOFieldName = "CityIds"
            }
        };
    }
}
