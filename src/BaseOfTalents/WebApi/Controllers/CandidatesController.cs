using Domain.Entities;
using Domain.Repositories;
using WebApi.DTO.DTOModels;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.DTO.DTOService.Abstract;

namespace WebApi.Controllers
{
    public class CandidatesController : BoTController<Candidate, CandidateDTO>
    {
        public CandidatesController(ICandidateRepository candidateRepository, ICandidateDTOService candidateDTOService)
        {
            _repo = candidateRepository;
            _dtoService = candidateDTOService;
        }

        [Route("api/candidates/{candidateId}/vacancies")]
        [HttpGet]
        public HttpResponseMessage VacanciesProgress(int candidateId)
        {
            HttpResponseMessage response;
            var foundedCandidate = _repo.Get(candidateId);
            if (foundedCandidate != null)
            {
                var foundedCandidateDto = _dtoService.ToDTO(foundedCandidate);
                response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = SerializeContent(foundedCandidateDto.VacanciesProgress)
                };
            }
            else
            {
                response = new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            return response;
        }
    }
}
