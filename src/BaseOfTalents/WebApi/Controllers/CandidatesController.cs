using Service.Services;
using Domain.DTO.DTOModels;
using Domain.Entities;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System;

namespace WebApi.Controllers
{
    public class CandidatesController : BoTController<Candidate, CandidateDTO>
    {
        public CandidatesController(IControllerService<Candidate, CandidateDTO> service)
            : base(service)
        {
        }

        public CandidatesController()
        {
        }

        [HttpGet]
        [Route("api/Candidates/")]
        public override IHttpActionResult Get()
        {
            return BadRequest("Get all is prohibited for vacancies. Use /search instead");
        }

        public override IHttpActionResult Add([FromBody]CandidateDTO candidate)
        {
            if (!ModelState.IsValid)
            {
                StringBuilder errorString = new StringBuilder();
                foreach (var error in ModelState.Keys.SelectMany(k => ModelState[k].Errors))
                {
                    errorString.Append(error.ErrorMessage + '\n');
                }
                return BadRequest(errorString.ToString());
            }
            if (candidate.Id != 0)
            {
                return BadRequest();
            }
            var addedCandidate = entityService.Add(candidate);
            return Json(addedCandidate, BOT_SERIALIZER_SETTINGS);
        }

        public override IHttpActionResult Put(int id, [FromBody] CandidateDTO changedEntity)
        {
            if (!ModelState.IsValid)
            {
                StringBuilder errorString = new StringBuilder();
                foreach (var error in ModelState.Keys.SelectMany(k => ModelState[k].Errors))
                {
                    errorString.Append(error.ErrorMessage + '\n');
                }
                return BadRequest(errorString.ToString());
            }
            if (changedEntity.Id != id)
            {
                return BadRequest();
            }
            var updatedCandidate = entityService.Put(changedEntity);
            return Json(updatedCandidate, BOT_SERIALIZER_SETTINGS);
        }
    }
}