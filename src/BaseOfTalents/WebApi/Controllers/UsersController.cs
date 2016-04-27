using Data.EFData.Extentions;
using Data.Infrastructure;
using Domain.DTO.DTOModels;
using Domain.Entities;
using Domain.Entities.Enum.Setup;
using Domain.Entities.Setup;
using Domain.Repositories;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class UsersController : BoTController<User, UserDTO>
    {
        public UsersController(IDataRepositoryFactory repoFatory, IUnitOfWork unitOfWork, IErrorRepository errorRepo)
            : base (repoFatory, unitOfWork, errorRepo)
        {

        }

        public override IHttpActionResult Add(HttpRequestMessage request, [FromBody]UserDTO user)
        {
            var _userRepo = _repoFactory.GetDataRepository<User>(request);

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
                    if (user.Id != 0)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        User _user = new User();
                        _user.Update(user/*, _repoFactory.GetDataRepository<Skill>(request), _repoFactory.GetDataRepository<Tag>(request)*/);
                        _userRepo.Add(_user);
                        _unitOfWork.Commit();
                        return Ok();
                    }
                }
            });
        }

        public override IHttpActionResult Put(HttpRequestMessage request, int id, [FromBody] UserDTO changedUser)
        {
            var _userRepo = _repoFactory.GetDataRepository<User>(request);

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
                    if (changedUser.Id != id)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        User _user = _userRepo.Get(id);
                        _user.Update(changedUser/*, _repoFactory.GetDataRepository<Skill>(request), _repoFactory.GetDataRepository<Tag>(request)*/);
                        _userRepo.Update(_user);
                        _unitOfWork.Commit();
                        return Json(_user, BOT_SERIALIZER_SETTINGS);
                    }
                }
            });
        }

    }
}
