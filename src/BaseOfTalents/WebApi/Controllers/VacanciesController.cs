using System.Linq;
using Domain.Entities;
using Domain.Repositories;
using Domain.DTO.DTOModels;
using System.Web.Http;
using Data.Infrastructure;
using Data.Infrastructure;
using System.Net.Http;
using Data.EFData.Extentions;
using Newtonsoft.Json.Linq;
using System.Text;
using Domain.Entities.Enum.Setup;
using Domain.Entities.Setup;
using WebApi.DTO.DTOService;

namespace WebApi.Controllers
{
    public class VacanciesController : BoTController<Vacancy, VacancyDTO>
    {
        public VacanciesController(IDataRepositoryFactory repoFatory, IUnitOfWork unitOfWork, IErrorRepository errorRepo)
            : base (repoFatory, unitOfWork, errorRepo)
        {

        }
        public VacanciesController()
        {

        }

        public override IHttpActionResult Add(HttpRequestMessage request, [FromBody]VacancyDTO vacancy)
        {
            var _vacancyRepo = _repoFactory.GetDataRepository<Vacancy>(request);

            return CreateResponse(request, () =>
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
                else
                {
                    if (vacancy.Id != 0)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        Vacancy _vacancy = new Vacancy();
                        _vacancy.Update(vacancy, _repoFactory.GetDataRepository<Level>(request), _repoFactory.GetDataRepository<Location>(request), _repoFactory.GetDataRepository<Skill>(request), _repoFactory.GetDataRepository<Tag>(request));
                        _vacancyRepo.Add(_vacancy);
                        _unitOfWork.Commit();
                        return Json(DTOService.ToDTO<Vacancy, VacancyDTO>(_vacancy), BOT_SERIALIZER_SETTINGS);

                    }
                }
            });
        }

    }
}

