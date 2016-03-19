using BotLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotWebApi.Models
{
    public interface IBotContext
    {
        List<Candidate> Candidates { get; }
        List<Vacancy> Vacancies { get; }
    }
}