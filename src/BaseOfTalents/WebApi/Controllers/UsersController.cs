using Service.Services;
using Domain.DTO.DTOModels;
using Domain.Entities;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class UsersController : BoTController<User, UserDTO>
    {
        public UsersController(IControllerService<User, UserDTO> service)
            : base(service)
        {
        }

        public override IHttpActionResult Add([FromBody]UserDTO user)
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
            if (user.Id != 0)
            {
                return BadRequest();
            }
            var addedUser = entityService.Add(user);
            return Json(addedUser, BOT_SERIALIZER_SETTINGS);
        }

        public override IHttpActionResult Put(int id, [FromBody] UserDTO changedUser)
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
            if (changedUser.Id != id)
            {
                return BadRequest();
            }
            var updatedUser = entityService.Put(changedUser);
            return Json(updatedUser, BOT_SERIALIZER_SETTINGS);
        }
    }
}