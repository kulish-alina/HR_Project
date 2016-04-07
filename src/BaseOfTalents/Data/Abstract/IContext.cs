using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Setup;
using System.Linq.Expressions;

namespace Data.Abstract
{
    public interface IContext
    {
        IEnumerable<Candidate> Candidates { get; set; }
        IEnumerable<Vacancy> Vacancies { get; set; }
    }
}
