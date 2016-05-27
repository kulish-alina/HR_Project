using System.Net.Http;
using System.Web.Http;
using Domain.DTO.DTOModels;
using Service.Services;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApi.Controllers
{
    public class FilesController : ApiController
    {
        FileService fileService;

        protected static JsonSerializerSettings BOT_SERIALIZER_SETTINGS = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public FilesController(FileService service)
        {

        }

        [HttpPost]
        [Route("api/files")]
        public IHttpActionResult Add(HttpRequestMessage request)
        {
            var uploadedFile = fileService.Add(request, HttpContext.Current);
            return Json(uploadedFile, BOT_SERIALIZER_SETTINGS);
        }
    }
}