using BotWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace BotWebApi.Controllers
{
    public class VacanciesController : ApiController
    {
        IBotContext _context = new DummyBotContext();

        public string GetVacancies()
        {
            var JsonVacancies = JsonConvert.SerializeObject(_context.Vacancies);
            return JsonVacancies;
        }
    }
}
