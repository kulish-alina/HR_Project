using BotLibrary.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BotData.Abstract
{
    public interface IContext
    {
        IList<Candidate> Candidates { get; set; }
        IList<Vacancy> Vacancies { get; set; }
    }
}
