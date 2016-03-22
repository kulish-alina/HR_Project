using BotWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using BotLibrary.Entities;

namespace BotWebApi.Controllers
{
    public class VacanciesController : ApiController
    {
        IBotContext _context = new DummyBotContext();

        [HttpGet]
        public HttpResponseMessage All()
        {
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(_context.Vacancies, Formatting.Indented, new JsonSerializerSettings
                {
                    DateFormatString = "dd/mm/yyyy",
                })),
            };
        }

        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            HttpResponseMessage response;
            if (_context.Vacancies.Any(x => x.Id == id))
            {
                var foundedVacancy = _context.Vacancies.First(x => x.Id == id);
                response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(foundedVacancy, Formatting.Indented, new JsonSerializerSettings
                    {
                        DateFormatString = "dd/mm/yyyy",
                    }))
                };
            }
            else
            {
                response = new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            return response;
        }

        [Route("api/vacancies/{vacancyId}/candidates")]
        [HttpGet]
        public HttpResponseMessage VacanciesProgress(int vacancyId)
        {
            HttpResponseMessage response;
            if (_context.Vacancies.Any(x => x.Id == vacancyId))
            {
                var foundedVacancy = _context.Vacancies.First(x => x.Id == vacancyId);
                response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(foundedVacancy.CandidatesProgress, Formatting.Indented, new JsonSerializerSettings
                    {
                        DateFormatString = "dd/mm/yyyy",
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
            var foundedVacancy = _context.Vacancies.First(x => x.Id == id);
            if (foundedVacancy != null)
            {
                _context.Vacancies.Remove(foundedVacancy);
                if (_context.Vacancies.Any(x => x != foundedVacancy))
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
            HttpResponseMessage response = new HttpResponseMessage();
            var newVacancy = entity.ToObject<Vacancy>();
            newVacancy.Id = _context.Candidates.Last().Id + 1;
            _context.Vacancies.Add(newVacancy);
            response.StatusCode = HttpStatusCode.Created;
            return response;
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]JObject entity)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            var changedVacancy = entity.ToObject<Vacancy>();
            var foundedVacancy = _context.Vacancies.First(x => x.Id == id);
            if (foundedVacancy != null)
            {
                _context.Vacancies.Remove(foundedVacancy);
                _context.Vacancies.Add(changedVacancy);
                _context.Vacancies.OrderBy(x => x.Id);
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

