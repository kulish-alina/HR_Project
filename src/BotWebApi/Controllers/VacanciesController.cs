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
using BotData.DumbData.Repositories;
using BotLibrary.Repositories;

namespace BotWebApi.Controllers
{
    public class VacanciesController : ApiController
    {
        IVacancyRepository _vacancyRepository;

        public VacanciesController(IVacancyRepository vacancyRepository)
        {
            _vacancyRepository = vacancyRepository;
        } 

        [HttpGet]
        public HttpResponseMessage All()
        {
            var vacanciesDto = _vacancyRepository.GetAll().Select(x => DTOService.VacancyToDTO(x));
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
            var foundedVacancy = _vacancyRepository.FindBy(x => x.Id == id).FirstOrDefault();
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
            var foundedVacancy = _vacancyRepository.FindBy(x => x.Id == vacancyId).FirstOrDefault();
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
            var foundedVacancy = _vacancyRepository.FindBy(x => x.Id == id).FirstOrDefault();
            if (foundedVacancy != null)
            {
                _vacancyRepository.Remove(foundedVacancy);
                response = new HttpResponseMessage(HttpStatusCode.OK);
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
            var newVacancy = DTOService.DTOToVacancy(newVacancyDto);
            _vacancyRepository.Add(newVacancy);
            return new HttpResponseMessage() {
                StatusCode = HttpStatusCode.Created
            };
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]JObject entity)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            var changedVacancyDto = entity.ToObject<VacancyDTO>();
            if (changedVacancyDto != null)
            {
                var changedVacancy = DTOService.DTOToVacancy(changedVacancyDto);
                _vacancyRepository.Update(changedVacancy);
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

