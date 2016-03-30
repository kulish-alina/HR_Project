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
using BotWebApi.DTO;
using BotWebApi.DTO.DTOModels;

namespace BotWebApi.Controllers
{
    public class VacanciesController : ApiController
    {
        IBotContext _context = new DummyBotContext();

        [HttpGet]
        public HttpResponseMessage All()
        {
            var vacanciesDto = _context.Vacancies.Select(x => DTOService.VacancyToDTO(x));
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(vacanciesDto, Formatting.Indented, new JsonSerializerSettings
                {
                    DateFormatString = "yyyy-MM-dd"
                })),
            };
        }

        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            HttpResponseMessage response;
            var foundedVacancy = _context.Vacancies.FirstOrDefault(x => x.Id == id);
            if (foundedVacancy != null)
            {
                var foundedVacancyDto = DTOService.VacancyToDTO(foundedVacancy);
                response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(foundedVacancyDto, Formatting.Indented, new JsonSerializerSettings
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

        [Route("api/vacancies/{vacancyId}/candidates")]
        [HttpGet]
        public HttpResponseMessage VacanciesProgress(int vacancyId)
        {
            HttpResponseMessage response;
            var foundedVacancy = _context.Vacancies.FirstOrDefault(x => x.Id == vacancyId);
            if (foundedVacancy!=null)
            {
                response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(foundedVacancy.CandidatesProgress, Formatting.Indented, new JsonSerializerSettings
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

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage response;
            var foundedVacancy = _context.Vacancies.FirstOrDefault(x => x.Id == id);
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
            var newVacancyDto = entity.ToObject<VacancyDTO>();
            newVacancyDto.Id = _context.Candidates.Last().Id + 1;
            var newVacancy = DTOService.DTOToVacancy(newVacancyDto);
            _context.Vacancies.Add(newVacancy);
            return new HttpResponseMessage() {
                StatusCode = HttpStatusCode.Created
            };
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]JObject entity)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            var changedVacancyDto = entity.ToObject<VacancyDTO>();
            var foundedVacancy = _context.Vacancies.FirstOrDefault(x => x.Id == id);
            if (foundedVacancy != null)
            {
                var changedVacancy = DTOService.DTOToVacancy(changedVacancyDto);
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

