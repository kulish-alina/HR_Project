using BotLibrary.Entities;
using BotWebApi.DTO;
using BotWebApi.DTO.DTOModels;
using BotWebApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace BotWebApi.Controllers
{
    public class CandidatesController : ApiController
    {
        IBotContext _context = new DummyBotContext();

        public CandidatesController()
        {
        }

        [HttpGet]
        public HttpResponseMessage All()
        {
            var dtoCandidates = _context.Candidates.Select(x => DTOService.CandidateToDTO(x));
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(dtoCandidates, Formatting.Indented, new JsonSerializerSettings
                {
                    DateFormatString = "yyyy-MM-dd"
                })),
            };
        }

        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            HttpResponseMessage response;
            var foundedCandidate = _context.Candidates.FirstOrDefault(x => x.Id == id);
            if (foundedCandidate!=null)
            {
                var foundedCandidateDto = DTOService.CandidateToDTO(foundedCandidate);
                response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(foundedCandidateDto, Formatting.Indented, new JsonSerializerSettings
                    {
                        DateFormatString = "yyyy-MM-dd"
                    }))
                };
            }
            else
            {
                response = new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            return response;
        }

        [Route("api/candidates/{candidateId}/vacancies")]
        [HttpGet]
        public HttpResponseMessage VacanciesProgress(int candidateId)
        {
            HttpResponseMessage response;
            var foundedCandidate = _context.Candidates.FirstOrDefault(x => x.Id == candidateId);
            if (foundedCandidate!=null)
            {
                var foundedCandidateDto = DTOService.CandidateToDTO(foundedCandidate);
                response = new HttpResponseMessage()
                {

                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(foundedCandidateDto.VacanciesProgress, Formatting.Indented, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        DateFormatString = "yyyy-MM-dd"
                    }))
                };
            }
            else
            {
                response = new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            return response;
        }


        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage response;
            var foundedCandidate = _context.Candidates.First(x => x.Id == id);
            if(foundedCandidate != null)
            { 
                _context.Candidates.Remove(foundedCandidate);
                if (_context.Candidates.Any(x => x != foundedCandidate)) 
                {
                    response = new HttpResponseMessage(HttpStatusCode.OK);
                }
                else
                {
                    response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                response = new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            return response;
       }
       
        [HttpPost]
        public HttpResponseMessage Add([FromBody]JObject entity)
       {
            var newCandidateDto = entity.ToObject<CandidateDTO>();
            newCandidateDto.Id = _context.Candidates.Last().Id + 1;
            var newCandidate = DTOService.DTOToCandidate(newCandidateDto);
            _context.Candidates.Add(newCandidate);
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Created,
                Content = new StringContent(JsonConvert.SerializeObject(_context.Candidates.Last(), Formatting.Indented, new JsonSerializerSettings
                {
                    DateFormatString = "yyyy-MM-dd"
                }))
            };
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]JObject entity)
        {
            var changedCandidateDto = entity.ToObject<CandidateDTO>();
            var foundedCandidate = _context.Candidates.FirstOrDefault(x => x.Id == id);
            HttpResponseMessage response = new HttpResponseMessage();
            if (foundedCandidate != null)
            {
                var changedCandidate = DTOService.DTOToCandidate(changedCandidateDto);
                _context.Candidates.Remove(foundedCandidate);
                _context.Candidates.Add(changedCandidate);
                _context.Candidates.OrderBy(x => x.Id);
                response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }
            return response;
        }

    }

}
