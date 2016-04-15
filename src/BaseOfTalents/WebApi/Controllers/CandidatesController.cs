using Domain.Entities;
using Domain.Repositories;
using WebApi.DTO.DTOModels;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.DTO.DTOService;

namespace WebApi.Controllers
{
    public class CandidatesController : BoTController<Candidate, CandidateDTO>
    {
        public CandidatesController(ICandidateRepository candidateRepository)
        {
            _repo = candidateRepository;
        }

        [Route("api/candidates/{candidateId}/vacancies")]
        [HttpGet]
        public IHttpActionResult VacanciesProgress(int candidateId)
        {
            var foundedCandidate = _repo.Get(candidateId);
            if (foundedCandidate != null)
            {
                var foundedCandidateDto = DTOService.ToDTO<Candidate, CandidateDTO>(foundedCandidate);
                return Json(foundedCandidateDto);
            }
            return NotFound();
        }
    }
}
