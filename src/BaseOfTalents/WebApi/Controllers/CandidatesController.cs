using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Setup;
using Domain.Repositories;
using WebApi.DTO;
using WebApi.DTO.DTOModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace WebApi.Controllers
{
    public class CandidatesController : ApiController
    {
        ICandidateRepository _candidateRepository;

        public CandidatesController(ICandidateRepository candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }

        [HttpGet]
        public HttpResponseMessage All()
        {
            var candidates = _candidateRepository.GetAll().ToList();
            var dtoCandidates = candidates.Select(x => DTOService.CandidateToDTO(x)).ToList<CandidateDTO>();
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
            var foundedCandidate = _candidateRepository.Get(id);
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
            var foundedCandidate = _candidateRepository.Get(candidateId);
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
            var foundedCandidate = _candidateRepository.Get(id);
            if(foundedCandidate != null)
            {
                _candidateRepository.Remove(foundedCandidate);
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
            var newCandidateDto = entity.ToObject<CandidateDTO>();
            var newCandidate = DTOService.DTOToCandidate(newCandidateDto);
            _candidateRepository.Add(newCandidate);
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Created,
                Content = new StringContent(JsonConvert.SerializeObject(_candidateRepository.GetAll().Last(), Formatting.Indented, new JsonSerializerSettings
                {
                    DateFormatString = "yyyy-MM-dd"
                }))
            };
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]JObject entity)
        {
            var changedCandidateDto = entity.ToObject<CandidateDTO>();
            HttpResponseMessage response = new HttpResponseMessage();
            if (changedCandidateDto != null)
            {
                var changedCandidate = DTOService.DTOToCandidate(changedCandidateDto);
                _candidateRepository.Update(changedCandidate);
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
