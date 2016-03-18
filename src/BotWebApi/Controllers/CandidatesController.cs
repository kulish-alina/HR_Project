using BotLibrary.Entities;
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

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage response;
            var foundedCandidate = _context.Candidates.First(x => x.Id == id);
            if(foundedCandidate != null)
            {
                response = new HttpResponseMessage(HttpStatusCode.Accepted);
                _context.Candidates.Remove(foundedCandidate);
            }
            else
            {
                response = new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return response;
       }
       
        [HttpPut]
        public HttpResponseMessage Put(string entity)
        {
            HttpResponseMessage response;
            var newCandidate = JsonConvert.DeserializeObject<Candidate>(entity);
            _context.Candidates.Add(newCandidate);
            //make connections
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

    }
}
