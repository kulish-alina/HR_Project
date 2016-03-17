using BotWebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace BotWebApi.Controllers
{
    public class CandidatesController : ApiController
    {
        IBotContext _context = new DummyBotContext();

        public HttpResponseMessage GetCandidates()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(_context.Candidates)),
            };
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return response;
        }
    }
}
