using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibrary.Entities.Setup
{
    public class VacancyStatus: BaseEntity
    {
        string Title { get; set; }
        string Description { get; set; }
    }
}
