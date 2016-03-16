using BotWebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BotWebApi.Controllers
{
    public class CandidatesController : ApiController
    {
        IBotContext _context = new DummyBotContext();

        public string GetCandidates()
        {
            var JsonCandidates = JsonConvert.SerializeObject(_context.Candidates);
            return JsonCandidates;
        }
    }
}
