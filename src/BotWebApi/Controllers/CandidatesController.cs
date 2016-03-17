using BotWebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BotWebApi.Controllers
{
    [EnableCors(origins: "http://localhost:53031/", headers: "*", methods: "*")]
    public class CandidatesController : ApiController
    {
        IBotContext _context = new DummyBotContext();

        [HttpGet]
        public HttpResponseMessage All()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(_context.Candidates)),
            };
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return response;
        }

        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            HttpResponseMessage response;
            var foundedCandidate = _context.Candidates.First(x => x.Id == id);
            if (foundedCandidate != null)
            {
                response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(foundedCandidate))
                };
            }
            else
            {
                response = new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return response;
        }

        public HttpResponseMessage Remove(int id)
        {
            HttpResponseMessage response;
            var foundedCandidate = _context.Candidates.First(x => x.Id == id);
            if(foundedCandidate != null)
            {
                response = new HttpResponseMessage(HttpStatusCode.OK);
                _context.Candidates.ToList().Remove(foundedCandidate);
                int k = 0;
            }
            else
            {
                response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return response;
        }
        public HttpResponseMessage Options()
        {
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        }

    }
}
