using BotLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotWebApi.Models
{
    public interface IBotContext
    {
        IQueryable<Candidate> Candidates { get; }
        IQueryable<Vacancy> Vacancies { get; }
    }
}